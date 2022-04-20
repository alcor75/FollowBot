using System.Threading.Tasks;
using System.Windows.Forms;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using Message = DreamPoeBot.Loki.Bot.Message;

namespace FollowBot.SimpleEXtensions.CommonTasks
{
    public class ClearCursorTask : ITask
    {
        public async Task<bool> Run()
        {
            var mode = LokiPoe.InGameState.CursorItemOverlay.Mode;
            if (mode == LokiPoe.InGameState.CursorItemModes.VirtualMove || mode == LokiPoe.InGameState.CursorItemModes.VirtualUse)
            {
                GlobalLog.Error("[ClearCursorTask] A virtual item is on the cursor. Now pressing Escape to clear it.");

                LokiPoe.Input.SimulateKeyEvent(Keys.Escape, true, false, false);
                await Wait.LatencySleep();
                await Wait.ArtificialDelay();
                return true;
            }

            if (mode == LokiPoe.InGameState.CursorItemModes.None)
                return false;

            var cursorItem = LokiPoe.InGameState.CursorItemOverlay.Item;
            if (cursorItem == null)
            {
                GlobalLog.Error($"[ClearCursorTask] Unexpected error. Cursor mode = \"{mode}\", but there is no item under cursor.");
                ErrorManager.ReportError();
                return true;
            }


            GlobalLog.Error($"[ClearCursorTask] \"{cursorItem.Name}\" is under cursor. Now going to place it into inventory.");

            if (!await Inventories.OpenInventory())
            {
                ErrorManager.ReportError();
                return true;
            }

            if (!LokiPoe.InGameState.InventoryUi.InventoryControl_Main.Inventory.CanFitItem(LokiPoe.InGameState.CursorItemOverlay.ItemSize, out int col, out int row))
            {
                GlobalLog.Error("[ClearCursorTask] There is no space in main inventory. Now stopping the bot because it cannot continue.");
                BotManager.Stop();
                return true;
            }

            if (!await LokiPoe.InGameState.InventoryUi.InventoryControl_Main.PlaceItemFromCursor(new Vector2i(col, row)))
                ErrorManager.ReportError();

            return true;
        }

        #region Unused interface methods

        public MessageResult Message(Message message)
        {
            return MessageResult.Unprocessed;
        }

        public async Task<LogicResult> Logic(Logic logic)
        {
            return LogicResult.Unprovided;
        }

        public void Start()
        {
        }

        public void Tick()
        {
        }

        public void Stop()
        {
        }

        public string Name => "ClearCursorTask";
        public string Description => "This task places any item left on the cursor into the inventory.";
        public string Author => "NotYourFriend original from EXVault";
        public string Version => "1.0";

        #endregion
    }
}
