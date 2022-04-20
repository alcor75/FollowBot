using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;

namespace FollowBot.SimpleEXtensions
{
    public static class Move
    {
        private static readonly Interval LogInterval = new Interval(1000);

        public static bool Towards(Vector2i pos, string destination)
        {
            if (LogInterval.Elapsed)
                GlobalLog.Debug($"[MoveTowards] Moving towards {destination} at {pos} (distance: {LokiPoe.MyPosition.Distance(pos)})");

            if (!PlayerMoverManager.MoveTowards(pos))
            {
                GlobalLog.Error($"[MoveTowards] Fail to move towards {destination} at {pos}");
                return false;
            }
            return true;
        }

        public static void TowardsWalkable(Vector2i pos, string destination)
        {
            if (!Towards(pos, destination))
            {
                GlobalLog.Error($"[MoveTowardsWalkable] Unexpected error. Fail to move towards {destination} at {pos}");
                ErrorManager.ReportError();
            }
        }

        public static async Task AtOnce(Vector2i pos, string destination, int minDistance = 20)
        {
            if (LokiPoe.MyPosition.Distance(pos) <= minDistance)
                return;

            while (LokiPoe.MyPosition.Distance(pos) > minDistance)
            {
                if (LogInterval.Elapsed)
                {
                    await Coroutines.CloseBlockingWindows();
                    GlobalLog.Debug($"[MoveAtOnce] Moving to {destination} at {pos} (distance: {LokiPoe.MyPosition.Distance(pos)})");
                }

                if (!LokiPoe.IsInGame || LokiPoe.Me.IsDead || BotManager.IsStopping)
                    return;
                if (!await OpenDoor())
                {
                    GlobalLog.Error($"[MoveAtOnce] Unable to open door while moving to {destination} at {pos}");
                    ErrorManager.ReportError();
                    continue;
                }
                TowardsWalkable(pos, destination);
                await Wait.Sleep(50);
            }
            await Coroutines.FinishCurrentAction();
        }
        private static async Task<bool> OpenDoor()
        {
            var door = LokiPoe.ObjectManager.Objects.Closest<TriggerableBlockage>(IsClosedDoor);

            if (door == null)
                return true;
            if (await PlayerAction.Interact(door))
            {
                //await Wait.LatencySleep();
                await Wait.For(() => !door.IsTargetable || door.IsOpened, "door opening", 50, 300);
                return true;
            }
            await Wait.SleepSafe(300);
            return false;
        }
        private static bool IsClosedDoor(TriggerableBlockage d)
        {
            return d.IsTargetable && !d.IsOpened && d.Distance <= 25 &&
                   (d.Name == "Door" || d.Metadata == "Metadata/MiscellaneousObjects/Smashable" || d.Metadata.Contains("LabyrinthSmashableDoor"));
        }
    }
}
