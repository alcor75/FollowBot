using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;

namespace FollowBot.SimpleEXtensions.Global
{
    public static class ResurrectionLogic
    {
        public static async Task Execute()
        {
            if (!await Resurrect(true))
            {
                if (!await Resurrect(false))
                {
                    GlobalLog.Error("[ResurrectionLogic] Resurrection failed. Now going to logout.");
                    if (!await Logout())
                    {
                        GlobalLog.Error("[ResurrectionLogic] Logout failed. Now stopping the bot because it cannot continue.");
                        BotManager.Stop();
                        return;
                    }
                }
            }
            GlobalLog.Info("[Events] Player resurrected.");
            var move = LokiPoe.InGameState.SkillBarHud.LastBoundMoveSkill;
            LokiPoe.InGameState.SkillBarHud.Use(move.Slots.Last(), true);
            Utility.BroadcastMessage(null, Events.Messages.PlayerResurrected);

        }

        private static async Task<bool> Resurrect(bool toCheckpoint, int attempts = 3)
        {
            GlobalLog.Debug($"[Resurrect] Now going to resurrect to {(toCheckpoint ? "checkpoint" : "town")}.");

            if (!await Wait.For(() => LokiPoe.InGameState.ResurrectPanel.IsOpened, "ResurrectPanel opening"))
                return false;

            await Wait.SleepSafe(100);

            await Wait.ArtificialDelay();

            for (int i = 1; i <= attempts; ++i)
            {
                GlobalLog.Debug($"[Resurrect] Attempt: {i}/{attempts}");

                if (!LokiPoe.IsInGame)
                {
                    GlobalLog.Debug("[Resurrect] Now exiting this logic because we are no longer in game.");
                    return true;
                }
                if (!LokiPoe.Me.IsDead)
                {
                    GlobalLog.Debug("[Resurrect] Now exiting this logic because we are no longer dead.");
                    return true;
                }

                var err = toCheckpoint
                    ? LokiPoe.InGameState.ResurrectPanel.ResurrectToCheckPoint()
                    : LokiPoe.InGameState.ResurrectPanel.ResurrectToTown();

                if (err == LokiPoe.InGameState.ResurrectResult.None)
                {
                    if (!await Wait.For(AliveInGame, "resurrection", 200, 5000))
                        continue;

                    GlobalLog.Debug("[Resurrect] Player has been successfully resurrected.");
                    await Wait.SleepSafe(250);
                    return true;
                }
                GlobalLog.Error($"[Resurrect] Fail to resurrect. Error: \"{err}\".");
                await Wait.SleepSafe(1000, 1500);
            }
            GlobalLog.Error("[Resurrect] All resurrection attempts have been spent.");
            return false;
        }

        private static async Task<bool> Logout(int attempts = 5)
        {
            for (int i = 1; i <= attempts; ++i)
            {
                GlobalLog.Debug($"[Logout] Attempt: {i}/{attempts}");

                if (!LokiPoe.IsInGame)
                {
                    GlobalLog.Debug("[Logout] Now exiting this logic because we are no longer in game.");
                    return true;
                }
                if (!LokiPoe.Me.IsDead)
                {
                    GlobalLog.Debug("[Logout] Now exiting this logic because we are no longer dead.");
                    return true;
                }

                var err = LokiPoe.EscapeState.LogoutToTitleScreen();
                if (err == LokiPoe.EscapeState.LogoutError.None)
                {
                    if (!await Wait.For(() => LokiPoe.IsInLoginScreen, "log out", 200, 5000))
                        continue;

                    GlobalLog.Debug("[Logout] Player has been successfully logged out.");
                    return true;
                }
                GlobalLog.Error($"[Logout] Fail to log out. Error: \"{err}\".");
                await Wait.SleepSafe(2000, 3000);
            }
            GlobalLog.Error("[Logout] All logout attempts have been spent.");
            return false;
        }

        private static bool AliveInGame()
        {
            if (LokiPoe.IsInLoginScreen)
            {
                GlobalLog.Error("[Resurrect] Disconnected while waiting for resurrection.");
                return true;
            }
            return !LokiPoe.Me.IsDead;
        }
    }
}
