using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.Class;
using Newtonsoft.Json;

namespace FollowBot
{
    public class FollowBotSettings : JsonSettings
    {
        private static FollowBotSettings _instance;
        public static FollowBotSettings Instance => _instance ?? (_instance = new FollowBotSettings());

        private FollowBotSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name, "FollowBot.json"))
        {
            if (DefensiveSkills == null)
                DefensiveSkills = SetupDefaultDefensiveSkills();
            if (Flasks == null)
                Flasks = SetupDefaultFlasks();
        }

        private bool _ignoreHiddenAuras;

        [DefaultValue(false)]
        public bool IgnoreHiddenAuras
        {
            get { return _ignoreHiddenAuras; }
            set
            { _ignoreHiddenAuras = value; NotifyPropertyChanged(() => IgnoreHiddenAuras); }
        }
        
        private string _acceptedManualInviteNames;
        private int _followDistance;
        private int _maxfollowDistance;
        private int _maxCombatDistance;
        private int _maxLootDistance;

        #region Party Role
        private bool _shouldKill;
        private bool _shouldLoot;
        private bool _useStalkerSentinel;
        private bool _dontPortOutofMap;
        private bool _shouldFollow = true;
        #endregion

        #region Defence
        private ObservableCollection<DefensiveSkillsClass> _defensiveSkills;
        private ObservableCollection<FlasksClass> _flasks;
        #endregion

        #region Gems
        private bool _gemDebugStatements;
        private bool _levelAllGems;
        private bool _levelOffhandOnly;
        private ObservableCollection<string> _globalNameIgnoreList;

        #endregion

        [DefaultValue("")]
        public string InviteWhiteList
        {
            get { return _acceptedManualInviteNames; }
            set
            { _acceptedManualInviteNames = value; NotifyPropertyChanged(() => InviteWhiteList); }
        }
        [DefaultValue(15)]
        public int FollowDistance
        {
            get { return _followDistance; }
            set
            { _followDistance = value; NotifyPropertyChanged(() => FollowDistance); }
        }
        [DefaultValue(25)]
        public int MaxFollowDistance
        {
            get { return _maxfollowDistance; }
            set
            { _maxfollowDistance = value; NotifyPropertyChanged(() => MaxFollowDistance); }
        }
        [DefaultValue(40)]
        public int MaxCombatDistance
        {
            get { return _maxCombatDistance; }
            set
            { _maxCombatDistance = value; NotifyPropertyChanged(() => MaxCombatDistance); }
        }
        [DefaultValue(40)]
        public int MaxLootDistance
        {
            get { return _maxLootDistance; }
            set
            { _maxLootDistance = value; NotifyPropertyChanged(() => MaxLootDistance); }
        }

        #region Party Role

        [DefaultValue(true)]
        public bool ShouldKill
        {
            get { return _shouldKill; }
            set
            { _shouldKill = value; NotifyPropertyChanged(() => ShouldKill); }
        }
        [DefaultValue(false)]
        public bool ShouldLoot
        {
            get { return _shouldLoot; }
            set
            { _shouldLoot = value; NotifyPropertyChanged(() => ShouldLoot); }
        }
        [DefaultValue(false)]
        public bool UseStalkerSentinel
        {
            get { return _useStalkerSentinel; }
            set
            { _useStalkerSentinel = value; NotifyPropertyChanged(() => UseStalkerSentinel); }
        }
        [DefaultValue(false)]
        public bool DontPortOutofMap
        {
            get { return _dontPortOutofMap; }
            set
            { _dontPortOutofMap = value; NotifyPropertyChanged(() => DontPortOutofMap); }
        }
        [JsonIgnore]
        public bool ShouldFollow
        {
            get { return _shouldFollow; }
            set
            { _shouldFollow = value; NotifyPropertyChanged(() => ShouldFollow); }
        }
        #endregion

        #region Defence Skills
        public ObservableCollection<DefensiveSkillsClass> DefensiveSkills
        {
            get => _defensiveSkills ;//?? (_defensiveSkills = new ObservableCollection<DefensiveSkillsClass>());
            set
            {
                _defensiveSkills = value;
                NotifyPropertyChanged(() => DefensiveSkills);
            }
        }

        #endregion

        #region Flasks
        public ObservableCollection<FlasksClass> Flasks
        {
            get => _flasks ;//?? (_flasks = new ObservableCollection<FlasksClass>());
            set
            {
                _flasks = value;
                NotifyPropertyChanged(() => Flasks);
            }
        }
        private ObservableCollection<DefensiveSkillsClass> SetupDefaultDefensiveSkills()
        {
            ObservableCollection<DefensiveSkillsClass> skills = new ObservableCollection<DefensiveSkillsClass>();

            skills.Add(new DefensiveSkillsClass(false, "Vaal Molten Shell", false, 0, 0,false));
            skills.Add(new DefensiveSkillsClass(false, "Vaal Discipline", false, 0, 0, false));
            skills.Add(new DefensiveSkillsClass(false, "Molten Shell", false, 0, 0, false));
            skills.Add(new DefensiveSkillsClass(false, "Steelskin", false, 0, 0, false));
            return skills;
        }
        private ObservableCollection<FlasksClass> SetupDefaultFlasks()
        {
            ObservableCollection<FlasksClass> flasks = new ObservableCollection<FlasksClass>();

            flasks.Add(new FlasksClass(false, 1, false, false, 0, 0));
            flasks.Add(new FlasksClass(false, 2, false, false, 0, 0));
            flasks.Add(new FlasksClass(false, 3, false, false, 0, 0));
            flasks.Add(new FlasksClass(false, 4, false, false, 0, 0));
            flasks.Add(new FlasksClass(false, 5, false, false, 0, 0));
            return flasks;
        }

        #endregion

        #region SkillGems
        /// <summary>
		/// Should the plugin log debug statements?
		/// </summary>
		[DefaultValue(false)]
        public bool GemDebugStatements
        {
            get
            {
                return _gemDebugStatements;
            }
            set
            {
                if (value.Equals(_gemDebugStatements))
                {
                    return;
                }
                _gemDebugStatements = value;
                NotifyPropertyChanged(() => GemDebugStatements);
            }
        }
        [DefaultValue(false)]
        public bool LevelOffhandOnly
        {
            get
            {
                return _levelOffhandOnly;
            }
            set
            {
                if (value.Equals(_levelOffhandOnly))
                {
                    return;
                }
                _levelOffhandOnly = value;
                NotifyPropertyChanged(() => LevelOffhandOnly);
            }
        }
        [DefaultValue(false)]
        public bool LevelAllGems
        {
            get
            {
                return _levelAllGems;
            }
            set
            {
                if (value.Equals(_levelAllGems))
                {
                    return;
                }
                _levelAllGems = value;
                NotifyPropertyChanged(() => LevelAllGems);
            }
        }
        /// <summary>
		/// A list of skillgem names to ignore from leveling.
		/// </summary>
		public ObservableCollection<string> GlobalNameIgnoreList
        {
            get
            {
                return _globalNameIgnoreList ?? (_globalNameIgnoreList = new ObservableCollection<string>());
            }
            set
            {
                if (value.Equals(_globalNameIgnoreList))
                {
                    return;
                }
                _globalNameIgnoreList = value;
                NotifyPropertyChanged(() => GlobalNameIgnoreList);
            }
        }

        /// <summary>
		/// A list of SkillGemEntry for the user's skillgems.
		/// </summary>
		[JsonIgnore]
        public ObservableCollection<SkillGemEntry> UserSkillGemsInOffHands
        {
            get
            {
                //using (LokiPoe.AcquireFrame())
                //{
                ObservableCollection<SkillGemEntry> skillGemEntries = new ObservableCollection<SkillGemEntry>();

                if (!LokiPoe.IsInGame)
                {
                    return skillGemEntries;
                }

                foreach (Inventory inv in UsableOffInventories)
                {
                    foreach (Item item in inv.Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (item.Components.SocketsComponent == null)
                        {
                            continue;
                        }

                        for (int idx = 0; idx < item.SocketedGems.Length; idx++)
                        {
                            Item gem = item.SocketedGems[idx];
                            if (gem == null)
                            {
                                continue;
                            }

                            skillGemEntries.Add(new SkillGemEntry(gem.Name, inv.PageSlot, idx));
                        }
                    }
                }
                return skillGemEntries;
                //}
            }
        }
        /// <summary>
        /// A list of SkillGemEntry for the user's skillgems.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<SkillGemEntry> UserSkillGems
        {
            get
            {
                //using (LokiPoe.AcquireFrame())
                //{
                ObservableCollection<SkillGemEntry> skillGemEntries = new ObservableCollection<SkillGemEntry>();

                if (!LokiPoe.IsInGame)
                {
                    return skillGemEntries;
                }

                foreach (Inventory inv in UsableInventories)
                {
                    foreach (Item item in inv.Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (item.Components.SocketsComponent == null)
                        {
                            continue;
                        }

                        for (int idx = 0; idx < item.SocketedGems.Length; idx++)
                        {
                            Item gem = item.SocketedGems[idx];
                            if (gem == null)
                            {
                                continue;
                            }

                            skillGemEntries.Add(new SkillGemEntry(gem.Name, inv.PageSlot, idx));
                        }
                    }
                }
                return skillGemEntries;
                //}
            }
        }
        public void UpdateGlobalNameIgnoreList()
        {
            NotifyPropertyChanged(() => GlobalNameIgnoreList);
        }
        public class SkillGemEntry
        {
            public string Name;
            public InventorySlot InventorySlot;
            public int SocketIndex;

            public string SerializationString { get; private set; }

            public SkillGemEntry(string name, InventorySlot slot, int socketIndex)
            {
                Name = name;
                InventorySlot = slot;
                SocketIndex = socketIndex;
                SerializationString = string.Format("{0} [{1}: {2}]", Name, InventorySlot, SocketIndex);
            }

            public Item InventoryItem
            {
                get
                {
                    return UsableInventories.Where(ui => ui.PageSlot == InventorySlot)
                        .Select(ui => ui.Items.FirstOrDefault())
                        .FirstOrDefault();
                }
            }

            public Item SkillGem
            {
                get
                {
                    Item item = InventoryItem;
                    if (item == null || item.Components.SocketsComponent == null)
                    {
                        return null;
                    }

                    Item sg = item.SocketedGems[SocketIndex];
                    if (sg == null)
                    {
                        return null;
                    }

                    if (sg.Name != Name)
                    {
                        return null;
                    }

                    return sg;
                }
            }
        }
        private static IEnumerable<Inventory> UsableInventories => new[]
        {
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.LeftHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.RightHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffLeftHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffRightHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Head),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Chest),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Gloves),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Boots),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.LeftRing),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.RightRing),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Neck)
        };
        private static IEnumerable<Inventory> UsableOffInventories => new[]
        {
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffLeftHand),
            LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.OffRightHand),
        };
        #endregion

        #region ChatCommands

        private string _TeleportToLeaderChatCommand;
        private string _stopFollowChatCommand;
        private string _startFollowChatCommand;
        private string _stopLootChatCommand;
        private string _startLootChatCommand;
        private string _stopAttackChatCommand;
        private string _startAttackChatCommand;
        private string _stopSentinelChatCommand;
        private string _startSentinelChatCommand;
        private string _stopAutoTeleportChatCommand;
        private string _startAutoTeleportChatCommand;

        [DefaultValue("Tele")]
        public string TeleportToLeaderChatCommand
        {
            get
            {
                return _TeleportToLeaderChatCommand;
            }
            set
            {
                _TeleportToLeaderChatCommand = value;
                NotifyPropertyChanged(() => TeleportToLeaderChatCommand);
            }
        }

        [DefaultValue("StopF")]
        public string StopFollowChatCommand
        {
            get
            {
                return _stopFollowChatCommand;
            }
            set
            {
                _stopFollowChatCommand = value;
                NotifyPropertyChanged(() => StopFollowChatCommand);
            }
        }

        [DefaultValue("StartF")]
        public string StartFollowChatCommand
        {
            get
            {
                return _startFollowChatCommand;
            }
            set
            {
                _startFollowChatCommand = value;
                NotifyPropertyChanged(() => StartFollowChatCommand);
            }
        }

        [DefaultValue("StopL")]
        public string StopLootChatCommand
        {
            get
            {
                return _stopLootChatCommand;
            }
            set
            {
                _stopLootChatCommand = value;
                NotifyPropertyChanged(() => StopLootChatCommand);
            }
        }

        [DefaultValue("StartL")]
        public string StartLootChatCommand
        {
            get
            {
                return _startLootChatCommand;
            }
            set
            {
                _startLootChatCommand = value;
                NotifyPropertyChanged(() => StartLootChatCommand);
            }
        }

        [DefaultValue("StopA")]
        public string StopAttackChatCommand
        {
            get
            {
                return _stopAttackChatCommand;
            }
            set
            {
                _stopAttackChatCommand = value;
                NotifyPropertyChanged(() => StopAttackChatCommand);
            }
        }

        [DefaultValue("StartA")]
        public string StartAttackChatCommand
        {
            get
            {
                return _startAttackChatCommand;
            }
            set
            {
                _startAttackChatCommand = value;
                NotifyPropertyChanged(() => StartAttackChatCommand);
            }
        }

        [DefaultValue("StopD")]
        public string StopSentinelChatCommand
        {
            get
            {
                return _stopSentinelChatCommand;
            }
            set
            {
                _stopSentinelChatCommand = value;
                NotifyPropertyChanged(() => StopSentinelChatCommand);
            }
        }

        [DefaultValue("StartD")]
        public string StartSentinelChatCommand
        {
            get
            {
                return _startSentinelChatCommand;
            }
            set
            {
                _startSentinelChatCommand = value;
                NotifyPropertyChanged(() => StartSentinelChatCommand);
            }
        }

        [DefaultValue("StopP")]
        public string StopAutoTeleportChatCommand
        {
            get
            {
                return _stopAutoTeleportChatCommand;
            }
            set
            {
                _stopAutoTeleportChatCommand = value;
                NotifyPropertyChanged(() => StopAutoTeleportChatCommand);
            }
        }

        [DefaultValue("StartP")]
        public string StartAutoTeleportChatCommand
        {
            get
            {
                return _startAutoTeleportChatCommand;
            }
            set
            {
                _startAutoTeleportChatCommand = value;
                NotifyPropertyChanged(() => StartAutoTeleportChatCommand);
            }
        }

        #endregion

        #region Overlay

        private bool _enableOverlay;
        private bool _drawInBackground;
        private bool _drawMobs;
        private bool _drawCorpses;
        private int _fps;
        private int _overlayXCoord;
        private int _overlayYCoord;
        private int _overlayTransparency;

        [DefaultValue(false)]
        public bool EnableOverlay
        {
            get => _enableOverlay;
            set
            {
                if (value == _enableOverlay) return;
                _enableOverlay = value;
                NotifyPropertyChanged(() => EnableOverlay);
            }
        }
        [DefaultValue(false)]
        public bool DrawInBackground
        {
            get => _drawInBackground;
            set
            {
                if (value == _drawInBackground) return;
                _drawInBackground = value;
                NotifyPropertyChanged(() => DrawInBackground);
            }
        }
        [DefaultValue(false)]
        public bool DrawMobs
        {
            get => _drawMobs;
            set
            {
                if (value == _drawMobs) return;
                _drawMobs = value;
                NotifyPropertyChanged(() => DrawMobs);
            }
        }
        [DefaultValue(false)]
        public bool DrawCorpses
        {
            get => _drawCorpses;
            set
            {
                if (value == _drawCorpses) return;
                _drawCorpses = value;
                NotifyPropertyChanged(() => DrawCorpses);
            }
        }

        [DefaultValue(30)]
        public int FPS
        {
            get => _fps;
            set
            {
                if (value == _fps) return;
                _fps = value;
                if (OverlayWindow.Instance != null)
                    OverlayWindow.Instance.SetFps(_fps);
                NotifyPropertyChanged(() => FPS);
            }
        }

        [DefaultValue(15)]
        public int OverlayXCoord
        {
            get => _overlayXCoord;
            set
            {
                if (value == _overlayXCoord) return;
                _overlayXCoord = value;
                NotifyPropertyChanged(() => OverlayXCoord);
            }
        }

        [DefaultValue(70)]
        public int OverlayYCoord
        {
            get => _overlayYCoord;
            set
            {
                if (value == _overlayYCoord) return;
                _overlayYCoord = value;
                NotifyPropertyChanged(() => OverlayYCoord);
            }
        }

        [DefaultValue(70)]
        public int OverlayTransparency
        {
            get => _overlayTransparency;
            set
            {
                if (value == _overlayTransparency) return;
                _overlayTransparency = value;
                if (OverlayWindow.Instance != null)
                    OverlayWindow.Instance.SetTransparency(_overlayTransparency);
                NotifyPropertyChanged(() => OverlayTransparency);
            }
        }

        #endregion
    }
}

    