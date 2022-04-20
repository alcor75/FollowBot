using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using FollowBot.SimpleEXtensions;

namespace FollowBot.Helpers
{
    public static class PartyHelper
    {

        public static List<string> PartyPortal = new List<string>()
        {
            "portal pls",
            "Portal ples",
            "new portal pls",
            "can i have a portal?",
            "new gate pls",
            "bro give me a port",
            "door pls",
            "port",
            "Port pls",
            "1 to teleport",
        };

        public static async Task<bool> HandlePartyInvite()
        {
            if (LokiPoe.InGameState.NotificationHud.NotificationList.Where(x => x.IsVisible).ToList().Count > 0)
            {
                FollowBot.Log.WarnFormat($"[FollowBot] Visible Notifications: {LokiPoe.InGameState.NotificationHud.NotificationList.Where(x => x.IsVisible).ToList().Count}");
                LokiPoe.InGameState.ProcessNotificationEx isPartyRequestToBeAccepted = (x, y) =>
                {
                    var res = y == LokiPoe.InGameState.NotificationType.Party && (string.IsNullOrEmpty(FollowBotSettings.Instance.InviteWhiteList) || FollowBotSettings.Instance.InviteWhiteList.Contains(x.CharacterName));
                    FollowBot.Log.WarnFormat($"[FollowBot] Detected {y.ToString()} request from char: {x.CharacterName} [AccountName: {x.AccountName}] Accepting? {res}");
                    return res;
                };

                var anyVis = LokiPoe.InGameState.NotificationHud.NotificationList.Any(x => x.IsVisible);
                if (anyVis)
                {
                    await Wait.Sleep(500);
                }
                var ret = LokiPoe.InGameState.NotificationHud.HandleNotificationEx(isPartyRequestToBeAccepted);
                FollowBot.Log.WarnFormat($"[HandlePartyInvite] Result: {ret}");
                await Coroutines.LatencyWait();
                if (ret == LokiPoe.InGameState.HandleNotificationResult.Accepted) return true;
            }
            return false;
        }
        
        public static async Task<bool> LeaveParty()
        {

            if (!LokiPoe.InGameState.ChatPanel.IsOpened)
                LokiPoe.InGameState.ChatPanel.ToggleChat();

            if (!LokiPoe.InGameState.ChatPanel.IsOpened) return false;
            LokiPoe.InGameState.ChatPanel.Chat("/kick " + LokiPoe.Me.Name);
            await Coroutines.LatencyWait();

            if (LokiPoe.InGameState.ChatPanel.IsOpened)
                LokiPoe.InGameState.ChatPanel.ToggleChat();

            return true;
        }

        public static async Task<bool> GoToPartyHideOut(string name)
        {
            await Coroutines.CloseBlockingWindows();
            await Coroutines.LatencyWait();

            LokiPoe.InGameState.PartyHud.OpenContextMenu(name);
            var ret = LokiPoe.InGameState.ContextMenu.VisitHideout();
            await Coroutines.LatencyWait();
            await Coroutines.ReactionWait();
            if (ret != LokiPoe.InGameState.ContextMenuResult.None)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> FastGotoPartyZone(string name)
        {
            await Coroutines.CloseBlockingWindows();
            await Coroutines.LatencyWait();

            var ret = LokiPoe.InGameState.PartyHud.FastGoToZone(name);
            await Coroutines.LatencyWait();
            await Coroutines.ReactionWait();
            if (ret != LokiPoe.InGameState.FastGoToZoneResult.None)
            {
                GlobalLog.Error($"[FastGotoPartyZone] Returned Error: {ret}");
                return false;
            }
            if (LokiPoe.InGameState.GlobalWarningDialog.IsOpened)
                LokiPoe.InGameState.GlobalWarningDialog.ConfirmDialog();
            return true;
        }
    }
}
