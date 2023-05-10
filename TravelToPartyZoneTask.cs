using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.Helpers;
using FollowBot.SimpleEXtensions;
using log4net;


namespace FollowBot
{
    class TravelToPartyZoneTask : ITask
    {
        private readonly ILog Log = Logger.GetLoggerInstanceForType();
        private bool _enabled = true;
        private Stopwatch _portalRequestStopwatch = Stopwatch.StartNew();
        private static int _zoneCheckRetry = 0;

        public string Name { get { return "TravelToPartyZone"; } }
        public string Description { get { return "This task will travel to party grind zone."; } }
        public string Author { get { return "NotYourFriend original from Unknown"; } }
        public string Version { get { return "0.0.0.1"; } }
        
        public void Start()
        {
        }
        public void Stop()
        {
        }
        public void Tick()
        {
        }

        public async Task<bool> Run()
        {
            if (!LokiPoe.IsInGame || LokiPoe.Me.IsDead)
            {
                return false;
            }

            await Coroutines.CloseBlockingWindows();
            
            var leader = LokiPoe.InstanceInfo.PartyMembers.FirstOrDefault(x => x.MemberStatus == PartyStatus.PartyLeader);
            if (leader == null) return false;
            var leadername = leader.PlayerEntry.Name;
            if (LokiPoe.InGameState.PartyHud.IsInSameZone(leadername))
            {
                _zoneCheckRetry = 0;
                return false;
            }
            else
            {
                //if (LokiPoe.CurrentWorldArea.IsMap || LokiPoe.CurrentWorldArea.Id.Contains("AfflictionTown") || LokiPoe.CurrentWorldArea.Id.Contains("Delve_"))
                //{
                //    if (FollowBotSettings.Instance.DontPortOutofMap) return false;
                //}

                _zoneCheckRetry ++;
                if (_zoneCheckRetry < 3)
                {
                    await Coroutines.LatencyWait();
                    GlobalLog.Warn($"IsInSameZone returned false for {leadername} retry [{_zoneCheckRetry}/3]");
                    return true;
                }
            }
            //First check the DontPortOutofMap
            var curZone = World.CurrentArea;
            if (!curZone.IsTown && !curZone.IsHideoutArea && FollowBotSettings.Instance.DontPortOutofMap) return false;
            //Then check for Delve portals:
            var delveportal = LokiPoe.ObjectManager.GetObjectsByType<AreaTransition>().FirstOrDefault(x => x.Name == "Azurite Mine" && (x.Metadata == "Metadata/MiscellaneousObject/PortalTransition" || x.Metadata == "Metadata/MiscellaneousObjects/PortalTransition"));
            if (delveportal != null)
            {
                Log.DebugFormat("[{0}] Found walkable delve portal.", Name);
                if (LokiPoe.Me.Position.Distance(delveportal.Position) > 15)
                {
                    var walkablePosition = ExilePather.FastWalkablePositionFor(delveportal, 13);

                    // Cast Phase run if we have it.
                    FollowBot.PhaseRun();

                    Move.Towards(walkablePosition, "moving to delve portal");
                    return true;
                }

                var tele = await Coroutines.InteractWith(delveportal);

                if (!tele)
                {
                    Log.DebugFormat("[{0}] delve portal error.", Name);
                }

                FollowBot.Leader = null;
                return true;
            }
            //Next check for Heist portals:
            var heistportal = LokiPoe.ObjectManager.GetObjectByMetadata("Metadata/Terrain/Leagues/Heist/Objects/MissionEntryPortal");
            if (heistportal != null && heistportal.Components.TargetableComponent.CanTarget)
            {
                Log.DebugFormat("[{0}] Found walkable heist portal.", Name);
                if (LokiPoe.Me.Position.Distance(heistportal.Position) > 20)
                {
                    var walkablePosition = ExilePather.FastWalkablePositionFor(heistportal, 20);

                    // Cast Phase run if we have it.
                    FollowBot.PhaseRun();

                    Move.Towards(walkablePosition, "moving to heist portal");
                    return true;
                }

                var tele = await Coroutines.InteractWith(heistportal);

                if (!tele)
                {
                    Log.DebugFormat("[{0}] heist portal error.", Name);
                }

                FollowBot.Leader = null;
                return true;
            }

            if (leader.PlayerEntry.Area.IsMap || leader.PlayerEntry.Area.IsTempleOfAtzoatl || leader.PlayerEntry.Area.Id.Contains("Expedition"))
            {
                if (!await TakePortal())
                    await Coroutines.ReactionWait();
                return true;
            }
            else if (leader.PlayerEntry.Area.IsLabyrinthArea)
            {
                if (leader.PlayerEntry.Area.Name == "Aspirants' Plaza")
                {
                    await PartyHelper.FastGotoPartyZone(leader.PlayerEntry.Name);
                    return true;
                }

                if (World.CurrentArea.Name == "Aspirants' Plaza")
                {
                    var trans = LokiPoe.ObjectManager.GetObjectByType<AreaTransition>();
                    if (trans == null)
                    {
                        var loc = ExilePather.FastWalkablePositionFor(new Vector2i(363, 423));
                        if (loc != Vector2i.Zero)
                        {
                            Move.Towards(loc, "Bronze Plaque");
                            return true;
                        }
                        else
                        {
                            GlobalLog.Warn($"[TravelToPartyZoneTask] Cant find Bronze Plaque location.");
                            return false;
                        }
                    }

                    if (LokiPoe.Me.Position.Distance(trans.Position) > 20)
                    {
                        var loc = ExilePather.FastWalkablePositionFor(trans.Position, 20);
                        Move.Towards(loc, $"{trans.Name}");
                        return true;
                    }

                    await PlayerAction.Interact(trans);
                    return true;
                }
                else if (World.CurrentArea.IsLabyrinthArea)
                {
                    AreaTransition areatransition = null;
                    areatransition = LokiPoe.ObjectManager.GetObjectsByType<AreaTransition>().OrderBy(x => x.Distance).FirstOrDefault(x => ExilePather.PathExistsBetween(LokiPoe.Me.Position, ExilePather.FastWalkablePositionFor(x.Position, 20)));
                    if (areatransition != null)
                    {
                        Log.DebugFormat("[{0}] Found walkable Area Transition [{1}].", Name, areatransition.Name);
                        if (LokiPoe.Me.Position.Distance(areatransition.Position) > 20)
                        {
                            var walkablePosition = ExilePather.FastWalkablePositionFor(areatransition, 20);

                            // Cast Phase run if we have it.
                            FollowBot.PhaseRun();

                            Move.Towards(walkablePosition, "moving to area transition");
                            return true;
                        }

                        var trans = await PlayerAction.TakeTransition(areatransition);

                        if (!trans)
                        {
                            Log.DebugFormat("[{0}] Areatransition error.", Name);
                        }

                        FollowBot.Leader = null;
                        return true;
                    }
                }
                GlobalLog.Warn($"[TravelToPartyZoneTask] Cant follow the leader in the Labirynt when the lab is already started.");
                return false;
            }

            await PartyHelper.FastGotoPartyZone(leadername);
            return true;
        }

        private async Task<bool> TakePortal()
        {
            var portal = LokiPoe.ObjectManager.GetObjectsByType<Portal>().FirstOrDefault(x => x.IsTargetable);
            if (portal != null)
            {
                if (portal.Position.Distance(LokiPoe.Me.Position) > 18)
                    await Move.AtOnce(portal.Position, "Move to portal");
                if (await Coroutines.InteractWith<Portal>(portal))
                    return true;
                else
                    return false;
            }
            else
            {
                Log.DebugFormat("[{0}] Failed to find portals.", Name);
                return false;
            }
        }

        public async Task<LogicResult> Logic(Logic logic)
        {
            return LogicResult.Unprovided;
        }

        public MessageResult Message(Message message)
        {
            if (message.Id == Events.Messages.AreaChanged)
            {
                _zoneCheckRetry = 0;
                return MessageResult.Processed;
            }
            if (message.Id == "Enable")
            {
                _enabled = true;
                return MessageResult.Processed;
            }
            if (message.Id == "Disable")
            {
                _enabled = false;
                return MessageResult.Processed;
            }
            return MessageResult.Unprocessed;
        }
    }
}