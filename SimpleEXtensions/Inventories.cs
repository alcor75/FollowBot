using System.Collections.Generic;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.SimpleEXtensions.Positions;
using Cursor = DreamPoeBot.Loki.Game.LokiPoe.InGameState.CursorItemOverlay;
using InventoryUi = DreamPoeBot.Loki.Game.LokiPoe.InGameState.InventoryUi;
using StashUi = DreamPoeBot.Loki.Game.LokiPoe.InGameState.StashUi;

namespace FollowBot.SimpleEXtensions
{
    public static class Inventories
    {
        public static List<Item> InventoryItems => LokiPoe.InstanceInfo.GetPlayerInventoryItemsBySlot(InventorySlot.Main);
        public static int AvailableInventorySquares => LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Main).AvailableInventorySquares;
        public static async Task<bool> OpenStash()
        {
            if (StashUi.IsOpened)
                return true;

            WalkablePosition stashPos;
            if (World.CurrentArea.IsTown)
            {
                stashPos = StaticPositions.GetStashPosByAct();
                //var stashObj = LokiPoe.ObjectManager.Stash;
                //if (stashObj == null)
                //{
                //    GlobalLog.Error("[OpenStash] Fail to find any Stash nearby.");
                //    return false;
                //}
                //stashPos = stashObj.WalkablePosition();
            }
            else
            {
                var stashObj = LokiPoe.ObjectManager.Stash;
                if (stashObj == null)
                {
                    GlobalLog.Error("[OpenStash] Fail to find any Stash nearby.");
                    return false;
                }
                stashPos = stashObj.WalkablePosition();
            }

            await PlayerAction.EnableAlwaysHighlight();

            await stashPos.ComeAtOnce(35);

            if (!await PlayerAction.Interact(LokiPoe.ObjectManager.Stash, () => StashUi.IsOpened && StashUi.StashTabInfo != null, "stash opening"))
                return false;

            await Wait.SleepSafe(LokiPoe.Random.Next(200, 400));
            await Wait.Sleep(100);
            return true;
        }

        public static async Task<bool> OpenInventory()
        {
            if (InventoryUi.IsOpened && !LokiPoe.InGameState.PurchaseUi.IsOpened && !LokiPoe.InGameState.SellUi.IsOpened)
                return true;

            await Coroutines.CloseBlockingWindows();

            LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.open_inventory_panel, true, false, false);

            if (!await Wait.For(() => InventoryUi.IsOpened, "inventory panel opening"))
                return false;

            await Wait.ArtificialDelay();
            await Wait.Sleep(20);
            return true;
        }


        #region Extension methods

        public static async Task<bool> PlaceItemFromCursor(this InventoryControlWrapper inventory, Vector2i pos)
        {
            var cursorItem = Cursor.Item;
            if (cursorItem == null)
            {
                GlobalLog.Error("[PlaceItemFromCursor] Cursor item is null.");
                return false;
            }

            GlobalLog.Debug($"[PlaceItemFromCursor] Now going to place \"{cursorItem.Name}\" from cursor to {pos}.");

            //apply item on another item, if we are in VirtualUse mode
            if (Cursor.Mode == LokiPoe.InGameState.CursorItemModes.VirtualUse)
            {
                var destItem = inventory.Inventory.FindItemByPos(pos);
                if (destItem == null)
                {
                    GlobalLog.Error("[PlaceItemFromCursor] Destination item is null.");
                    return false;
                }
                int destItemId = destItem.LocalId;
                var applied = inventory.ApplyCursorTo(destItem.LocalId);
                if (applied != ApplyCursorResult.None)
                {
                    GlobalLog.Error($"[PlaceItemFromCursor] Fail to place item from cursor. Error: \"{applied}\".");
                    return false;
                }
                //wait for destination item change, it cannot become null, ID should change
                return await Wait.For(() =>
                {
                    var item = inventory.Inventory.FindItemByPos(pos);
                    return item != null && item.LocalId != destItemId;
                }, "destination item change");
            }

            //in other cases, place item to empty inventory slot or swap it with another item
            int cursorItemId = cursorItem.LocalId;
            var placed = inventory.PlaceCursorInto(pos.X, pos.Y, true);
            if (placed != PlaceCursorIntoResult.None)
            {
                GlobalLog.Error($"[PlaceItemFromCursor] Fail to place item from cursor. Error: \"{placed}\".");
                return false;
            }

            //wait for cursor item change, if we placed - it should become null, if we swapped - ID should change
            if (!await Wait.For(() =>
            {
                var item = Cursor.Item;
                return item == null || item.LocalId != cursorItemId;
            }, "cursor item change")) return false;

            await Wait.ArtificialDelay();

            return true;
        }

        #endregion
    }
}
