using System;
using System.Diagnostics.CodeAnalysis;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using FollowBot.SimpleEXtensions.CachedObjects;
using FollowBot.SimpleEXtensions.Global;

namespace FollowBot.SimpleEXtensions
{
    public static class Events
    {
        public static event Action IngameBotStart;
        public static event EventHandler<AreaChangedArgs> AreaChanged;
        public static event EventHandler<AreaChangedArgs> CombatAreaChanged;
        public static event Action<int> PlayerDied;
        public static event Action PlayerResurrected;
        public static event Action<int> PlayerLeveled;
        public static event Action<CachedItem> ItemLootedEvent;
        public static event Action<CachedItem> ItemStashedEvent;
        public static event EventHandler<ItemsSoldArgs> ItemsSoldEvent;

        private static bool _checkStart;

        private static uint _areaHash;
        private static uint _combatAreaHash;
        private static DatWorldAreaWrapper _area;
        private static DatWorldAreaWrapper _combatArea;

        private static bool _isDead;

        private static string _name;
        private static int _level;

        public static void Start()
        {
            _checkStart = true;
        }

        public static void Tick()
        {
            if (!LokiPoe.IsInGame)
                return;

            if (_checkStart)
            {
                _checkStart = false;
                GlobalLog.Info("[Events] Ingame bot start.");
                Utility.BroadcastMessage(null, Messages.IngameBotStart);
            }

            var newHash = LokiPoe.LocalData.AreaHash;
            if (newHash != _areaHash)
            {
                var oldArea = _area;
                var oldHash = _areaHash;
                _area = World.CurrentArea;
                _areaHash = newHash;
                var areaName = _area.Name;

                GlobalLog.Info($"[Events] Area changed ({oldArea?.Name ?? "null"} -> {areaName})");
                Utility.BroadcastMessage(null, Messages.AreaChanged, oldHash, newHash, oldArea, _area);
                // Change FPS per area.
                if (DreamPoeBot.Loki.GlobalSettings.Instance.IsBackgroundFpsActive)
                {
                    LokiPoe.ClientFunctions.SetBackgroundFps(_area.IsHideoutArea
                        ? 60
                        : DreamPoeBot.Loki.GlobalSettings.Instance.BackgroundFps);
                }

                if (newHash != _combatAreaHash && _area.IsCombatArea)
                {
                    var oldCombatArea = _combatArea;
                    var oldCombatHash = _combatAreaHash;
                    _combatArea = _area;
                    _combatAreaHash = newHash;

                    GlobalLog.Info($"[Events] Combat area changed ({oldCombatArea?.Name ?? "null"} -> {areaName})");
                    Utility.BroadcastMessage(null, Messages.CombatAreaChanged, oldCombatHash, newHash, oldCombatArea, _area);
                }
            }

            var me = LokiPoe.Me;
            if (me.IsDead)
            {
                if (!_isDead)
                {
                    _isDead = true;
                    var cache = CombatAreaCache.Current;
                    ++cache.DeathCount;
                    GlobalLog.Info($"[Events] Player died ({cache.DeathCount})");
                    Utility.BroadcastMessage(null, Messages.PlayerDied, cache.DeathCount);
                }
            }
            else
            {
                _isDead = false;
            }

            var name = me.Name;
            var level = me.Level;
            if (name != _name)
            {
                _name = name;
                _level = level;
            }
            else
            {
                if (level > _level)
                {
                    _level = level;
                    GlobalLog.Info($"[Events] Player leveled ({level})");
                    Utility.BroadcastMessage(null, Messages.PlayerLeveled, level);
                }
            }
        }

        public static void FireEventsFromMessage(Message message)
        {
            switch (message.Id)
            {
                case Messages.IngameBotStart:
                    IngameBotStart?.Invoke();
                    return;

                case Messages.AreaChanged:
                    AreaChanged?.Invoke(null, new AreaChangedArgs(message));
                    return;

                case Messages.CombatAreaChanged:
                    CombatAreaChanged?.Invoke(null, new AreaChangedArgs(message));
                    return;

                case Messages.PlayerDied:
                    PlayerDied?.Invoke(message.GetInput<int>());
                    return;

                case Messages.PlayerResurrected:
                    PlayerResurrected?.Invoke();
                    return;

                case Messages.PlayerLeveled:
                    PlayerLeveled?.Invoke(message.GetInput<int>());
                    return;

                case Messages.ItemLootedEvent:
                    ItemLootedEvent?.Invoke(message.GetInput<CachedItem>());
                    return;

                case Messages.ItemStashedEvent:
                    ItemStashedEvent?.Invoke(message.GetInput<CachedItem>());
                    return;

                case Messages.ItemsSoldEvent:
                    ItemsSoldEvent?.Invoke(null, new ItemsSoldArgs(message));
                    return;
            }
        }

        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        public static class Messages
        {
            public const string IngameBotStart = "ingame_bot_start_event";
            public const string AreaChanged = "area_changed_event";
            public const string CombatAreaChanged = "combat_area_changed_event";
            public const string PlayerDied = "player_died_event";
            public const string PlayerResurrected = "player_resurrected_event";
            public const string PlayerLeveled = "player_leveled_event";
            public const string ItemLootedEvent = "item_looted_event";
            public const string ItemStashedEvent = "item_stashed_event";
            public const string ItemsSoldEvent = "items_sold_event";
        }
    }
}
