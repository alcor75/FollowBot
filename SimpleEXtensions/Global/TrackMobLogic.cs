using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.Objects;

namespace FollowBot.SimpleEXtensions.Global
{
    public static class TrackMobLogic
    {
        private const int MaxKillAttempts = 20;
        private static readonly Interval LogInterval = new Interval(1000);

        public static CachedObject CurrentTarget;

        static TrackMobLogic()
        {
            Events.AreaChanged += (sender, args) => CurrentTarget = null;
        }

        public static async Task<bool> Execute(int range = -1)
        {
            var cachedMonsters = CombatAreaCache.Current.Monsters;

            if (CurrentTarget == null)
            {
                CurrentTarget = range == -1
                    ? cachedMonsters.ClosestValid()
                    : cachedMonsters.ClosestValid(m => m.Position.Distance <= range);

                if (CurrentTarget == null)
                    return false;
            }

            if (Blacklist.Contains(CurrentTarget.Id))
            {
                GlobalLog.Debug("[TrackMobLogic] Current target is in global blacklist. Now abandoning it.");
                CurrentTarget.Ignored = true;
                CurrentTarget = null;
                return true;
            }

            var pos = CurrentTarget.Position;
            if (pos.IsFar || pos.IsFarByPath)
            {
                if (LogInterval.Elapsed)
                {
                    GlobalLog.Debug($"[TrackMobTask] Cached monster locations: {cachedMonsters.Valid().Count()}");
                    GlobalLog.Debug($"[TrackMobTask] Moving to {pos}");
                }
                if (!PlayerMoverManager.MoveTowards(pos))
                {
                    GlobalLog.Error($"[TrackMobTask] Fail to move to {pos}. Marking this monster as unwalkable.");
                    CurrentTarget.Unwalkable = true;
                    CurrentTarget = null;
                }
                return true;
            }

            var monsterObj = CurrentTarget.Object as Monster;

            // Untested fix to not wait on a captured beast. Will be changed once confirmed issue is solved.
            //if (monsterObj == null || monsterObj.IsDead || (Loki.Game.LokiPoe.InstanceInfo.Bestiary.IsActive && (monsterObj.HasBestiaryCapturedAura || monsterObj.HasBestiaryDisappearingAura)))

            if (monsterObj == null || monsterObj.IsDead)
            {
                cachedMonsters.Remove(CurrentTarget);
                CurrentTarget = null;
            }
            else
            {
                var attempts = ++CurrentTarget.InteractionAttempts;
                if (attempts > MaxKillAttempts)
                {
                    GlobalLog.Error("[TrackMobTask] All attempts to kill current monster have been spent. Now ignoring it.");
                    CurrentTarget.Ignored = true;
                    CurrentTarget = null;
                    return true;
                }
                GlobalLog.Debug($"[TrackMobTask] Alive monster is nearby, this is our {attempts}/{MaxKillAttempts} attempt to kill it.");
                await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(200);
            }
            return true;
        }
    }
}
