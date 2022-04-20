using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using FollowBot.SimpleEXtensions.Positions;

namespace FollowBot.SimpleEXtensions.Global
{
    public class ComplexExplorer
    {
        private const int MaxTransitionAttempts = 5;
        public const string LocalTransitionEnteredMessage = "explorer_local_transition_entered_event";

        public static event Action LocalTransitionEntered;

        private static readonly Interval LogInterval = new Interval(1000);

        private static readonly List<SettingsProvider> SettingsProviders = new List<SettingsProvider>();

        private CachedTransition _transition;
        private WorldPosition _levelAnchor;
        private readonly GridExplorer _explorer;
        private readonly Dictionary<WorldPosition, GridExplorer> _explorers;

        // to prevent previous level explorer from ticking while we are taking transition to next level 
        private bool _disableTick;

        // to detect external exploration interruption (chicken, disconnect) in multilevel areas
        private WorldPosition _myLastPos;


        public ExplorationSettings Settings { get; }
        public bool Finished { get; private set; }

        public GridExplorer BasicExplorer => Settings.BasicExploration ? _explorer : _explorers[_levelAnchor];

        public ComplexExplorer()
        {
            Settings = ProvideSettings();
            Settings.LogProperties();
            if (Settings.BasicExploration)
            {
                _explorer = CreateExplorer();
            }
            else
            {
                _explorers = new Dictionary<WorldPosition, GridExplorer>();
                InitNewLevel();
            }
        }

        public async Task<bool> Execute()
        {
            if (Finished)
                return false;

            if (Settings.BasicExploration)
            {
                if (!BasicExploration())
                    Finished = true;
            }
            else
            {
                if (!await ComplexExploration())
                    Finished = true;
            }
            return true;
        }

        private bool BasicExploration()
        {
            var explorer = BasicExplorer;

            if (!explorer.HasLocation)
                return false;

            var location = explorer.Location;
            if (LogInterval.Elapsed)
            {
                var distance = LokiPoe.MyPosition.Distance(location);
                var percent = Math.Round(explorer.PercentComplete, 1);
                GlobalLog.Debug($"[ComplexExplorer] Exploring to the location {location} ({distance}) [{percent} %].");
            }
            if (!PlayerMoverManager.MoveTowards(location))
            {
                GlobalLog.Error($"[ComplexExplorer] MoveTowards failed for {location}. Adding this location to ignore list.");
                explorer.Ignore(location);
            }
            return true;
        }

        private async Task<bool> ComplexExploration()
        {
            if (_myLastPos.Distance >= 100 && !_myLastPos.PathExists)
            {
                GlobalLog.Error("[ComplexExplorer] Cannot pathfind to my last position. Now resetting current level and all visited transitions.");
                HandleExternalJump();
            }

            _myLastPos = new WorldPosition(LokiPoe.MyPosition);

            if (Settings.FastTransition)
            {
                if (_transition != null || (_transition = FrontTransition) != null)
                {
                    await EnterTransition();
                    return true;
                }
            }
            if (!BasicExploration())
            {
                if (_transition != null)
                {
                    await EnterTransition();
                    return true;
                }
                _transition = FrontTransition;
                if (_transition == null)
                {
                    if (Settings.Backtracking)
                    {
                        if ((_transition = BackTransition) != null)
                            return true;
                    }
                    GlobalLog.Warn("[ComplexExplorer] Out of area transitions. Now finishing the exploration.");
                    return false;
                }
            }
            return true;
        }

        private async Task EnterTransition()
        {
            var pos = _transition.Position;
            if (pos.IsFar)
            {
                if (!pos.TryCome())
                {
                    GlobalLog.Debug($"[ComplexExplorer] Fail to move to {pos}. Marking this transition as unwalkable.");
                    _transition.Unwalkable = true;
                    _transition = null;
                }
                return;
            }
            var transitionObj = _transition.Object;
            if (transitionObj == null)
            {
                GlobalLog.Error("[ComplexExplorer] Unknown error. There is no transition near cached position.");
                _transition.Ignored = true;
                _transition = null;
                return;
            }
            if (!transitionObj.IsTargetable)
            {
                if (transitionObj.Metadata.Contains("sarcophagus_transition"))
                {
                    if (await HandleSarcophagus() && transitionObj.Fresh().IsTargetable)
                        return;
                }
                if (_transition.InteractionAttempts >= MaxTransitionAttempts)
                {
                    GlobalLog.Error("[ComplexExplorer] Area transition did not become targetable. Now ignoring it.");
                    _transition.Ignored = true;
                    _transition = null;
                    return;
                }
                var attempts = ++_transition.InteractionAttempts;
                GlobalLog.Debug($"[ComplexExplorer] Waiting for \"{pos.Name}\" to become targetable ({attempts}/{MaxTransitionAttempts})");
                await Wait.SleepSafe(1000);
                return;
            }

            _disableTick = true;
            if (!await PlayerAction.TakeTransition(transitionObj))
            {
                var attempts = ++_transition.InteractionAttempts;
                GlobalLog.Error($"[ComplexExplorer] Fail to enter {pos}. Attempt: {attempts}/{MaxTransitionAttempts}");
                if (attempts >= MaxTransitionAttempts)
                {
                    GlobalLog.Error("[ComplexExplorer] All attempts to enter an area transition have been spent. Now ignoring it.");
                    _transition.Ignored = true;
                    _transition = null;
                }
                else
                {
                    await Wait.SleepSafe(500);
                }
            }
            else
            {
                PostEnter();

                GlobalLog.Info("[ComplexExplorer] LocalTransitionEntered event.");
                LocalTransitionEntered?.Invoke();
                Utility.BroadcastMessage(this, LocalTransitionEnteredMessage);

                if (Settings.OpenPortals)
                {
                    GlobalLog.Info("[ComplexExplorer] Opening a portal.");
                    await PlayerAction.CreateTownPortal();
                }
            }
            _disableTick = false;
        }

        private void PostEnter()
        {
            _transition.Visited = true;
            _transition = null;
            ResetLevelAnchor();
            MarkBackTransition();
            Blacklist.Reset();
        }

        private static void MarkBackTransition()
        {
            CombatAreaCache.Tick(); //ensure that area transition list is updated

            var backTransition = CombatAreaCache.Current.AreaTransitions
                .Where(a => a.Type == TransitionType.Local && !a.LeadsBack && !a.Visited && a.Position.Distance <= 50)
                .OrderBy(a => a.Position.DistanceSqr)
                .FirstOrDefault();

            if (backTransition == null)
            {
                GlobalLog.Debug("[ComplexExplorer] No back transition detected.");
                return;
            }
            backTransition.LeadsBack = true;
            GlobalLog.Debug($"[ComplexExplorer] Marking {backTransition.Position} as back transition.");
        }

        private void HandleExternalJump()
        {
            _transition = null;
            foreach (var t in CombatAreaCache.Current.AreaTransitions)
            {
                t.Visited = false;
            }
            // Current explorer is ticking on a wrong level, reset it.
            _disableTick = true;
            BasicExplorer.Reset();
            ResetLevelAnchor();
            _disableTick = false;
        }

        private static async Task<bool> HandleSarcophagus()
        {
            var sarcophagus = LokiPoe.ObjectManager.Objects.Find(o => o.Metadata.Contains("sarcophagus_door"));
            if (sarcophagus == null)
            {
                GlobalLog.Error("[ComplexExplorer] There is no sarcophagus.");
                return false;
            }
            if (!sarcophagus.IsTargetable)
            {
                GlobalLog.Error("[ComplexExplorer] Sarcophagus is not targetable.");
                return false;
            }
            return await PlayerAction.Interact(sarcophagus, () => !sarcophagus.Fresh().IsTargetable, "Sarcophagus interaction");
        }

        private void ResetLevelAnchor()
        {
            var anchors = _explorers.Keys;
            foreach (var anchor in anchors)
            {
                if (anchor.PathExists)
                {
                    GlobalLog.Debug($"[ComplexExplorer] Resetting anchor point to existing one {anchor}");
                    _levelAnchor = anchor;
                    _myLastPos = new WorldPosition(LokiPoe.MyPosition);
                    return;
                }
            }
            InitNewLevel();
        }

        private void InitNewLevel()
        {
            _levelAnchor = new WorldPosition(LokiPoe.MyPosition);
            _myLastPos = _levelAnchor;
            _explorers.Add(_levelAnchor, CreateExplorer());
            GlobalLog.Debug($"[ComplexExplorer] Creating new level anchor {_levelAnchor}");
        }

        private GridExplorer CreateExplorer()
        {
            var explorer = new GridExplorer
            {
                AutoResetOnAreaChange = false,
                TileKnownRadius = Settings.TileKnownRadius,
                TileSeenRadius = Settings.TileSeenRadius
            };
            explorer.Start();
            return explorer;
        }

        public void Tick()
        {
            if (_disableTick)
                return;

            BasicExplorer.Tick();
        }

        public static bool AddSettingsProvider(string ownerId, Func<ExplorationSettings> getSettings, ProviderPriority priority)
        {
            if (!SettingsProviders.Exists(s => s.OwnerId == ownerId))
            {
                SettingsProviders.Add(new SettingsProvider(ownerId, getSettings, priority));
                GlobalLog.Info($"[ComplexExplorer] {ownerId} settings provider has been added.");
                return true;
            }
            return false;
        }

        public static bool RemoveSettingsProvider(string ownerId)
        {
            var index = SettingsProviders.FindIndex(s => s.OwnerId == ownerId);
            if (index >= 0)
            {
                SettingsProviders.RemoveAt(index);
                GlobalLog.Info($"[ComplexExplorer] {ownerId} settings provider has been removed.");
                return true;
            }
            return false;
        }

        public static void ResetSettingsProviders()
        {
            SettingsProviders.Clear();
        }

        private static ExplorationSettings ProvideSettings()
        {
            foreach (var provider in SettingsProviders.OrderBy(p => p.Priority))
            {
                var settings = provider.GetSettings();
                if (settings != null)
                {
                    GlobalLog.Info($"[ComplexExplorer] Exploration settings provided by {provider.OwnerId}.");
                    return settings;
                }
            }
            GlobalLog.Info("[ComplexExplorer] Exploration settings was not provided. Using default.");
            return new ExplorationSettings();
        }

        private CachedTransition FrontTransition
        {
            get
            {
                if (Settings.PriorityTransition != null)
                {
                    var transition = CombatAreaCache.Current.AreaTransitions
                        .FirstOrDefault(t =>
                            !t.Ignored && !t.Unwalkable &&
                            t.Type == TransitionType.Local && !t.Visited && !t.LeadsBack &&
                            t.Position.Name == Settings.PriorityTransition);

                    if (transition != null)
                        return transition;
                }
                return CombatAreaCache.Current.AreaTransitions.ClosestValid(t => t.Type == TransitionType.Local && !t.Visited && !t.LeadsBack);
            }
        }

        private static CachedTransition BackTransition => CombatAreaCache.Current.AreaTransitions
            .Where(t => t.Type == TransitionType.Local && !t.Visited)
            .ClosestValid();

        private class SettingsProvider
        {
            public readonly string OwnerId;
            public readonly Func<ExplorationSettings> GetSettings;
            public readonly ProviderPriority Priority;

            public SettingsProvider(string ownerId, Func<ExplorationSettings> getSettings, ProviderPriority priority)
            {
                OwnerId = ownerId;
                GetSettings = getSettings;
                Priority = priority;
            }
        }
    }

    public class ExplorationSettings
    {
        public const int DefaultTileKnownRadius = 7;
        public const int DefaultTileSeenRadius = 4;

        public bool BasicExploration { get; set; }
        public bool FastTransition { get; set; }
        public bool Backtracking { get; set; }
        public bool OpenPortals { get; set; }
        public string PriorityTransition { get; set; }
        public int TileKnownRadius { get; set; }
        public int TileSeenRadius { get; set; }

        public ExplorationSettings
        (
            bool basicExploration = true,
            bool fastTransition = false,
            bool backtracking = false,
            bool openPortals = false,
            string priorityTransition = null,
            int tileKnownRadius = DefaultTileKnownRadius,
            int tileSeenRadius = DefaultTileSeenRadius
        )
        {
            BasicExploration = basicExploration;
            FastTransition = fastTransition;
            Backtracking = backtracking;
            OpenPortals = openPortals;
            PriorityTransition = priorityTransition;
            TileKnownRadius = tileKnownRadius;
            TileSeenRadius = tileSeenRadius;
        }
    }

    public enum ProviderPriority
    {
        High,
        Normal,
        Low
    }
}
