using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.BotFramework;
using DreamPoeBot.Loki.Controllers;
using FollowBot.SimpleEXtensions.CachedObjects;
using FollowBot.SimpleEXtensions.Global;
using FollowBot.SimpleEXtensions.Positions;

namespace FollowBot.SimpleEXtensions.CommonTasks
{
	public class LootItemTask : ITask
	{
		private const int MaxItemPickupAttempts = 10;
		private static readonly Interval LogInterval = new Interval(1000);

        private CachedWorldItem _item;

        public async Task<bool> Run()
        {
            if (!FollowBotSettings.Instance.ShouldLoot) return false;

			if (!World.CurrentArea.IsCombatArea)
			{
                return false;
			}

            List<CachedWorldItem> items = CombatAreaCache.Current.Items;
            List<CachedWorldItem> validItems = new List<CachedWorldItem>();

            var allItems = items.FindAll(i => !i.Ignored && !i.Unwalkable);
            if (allItems.Count == 0)
            {
                return false;
            }

            var refPos = FollowBot.Leader != null ? FollowBot.Leader.Position : LokiPoe.Me.Position;

			validItems = allItems.FindAll(i => i.Position.AsVector.Distance(refPos) <= FollowBotSettings.Instance.MaxLootDistance);
			var itToFarCount = allItems.Count - validItems.Count;
            

            if (validItems.Count == 0)
            {
                if (itToFarCount > 0)
                {
                    GlobalLog.Warn($"[LootItemTask] {itToFarCount} will be ignored B/C they are to far away. (Increase MaxLootDistance or bring the Leader closerto them.)");
                }
				return false;
            }

			if (_item == null)
			{
                _item = validItems.OrderBy(i => i.Position.DistanceSqr).First();
			}

            //if (_item.Position.AsVector.Distance(FollowBot.Leader != null
            //        ? FollowBot.Leader.Position
            //        : LokiPoe.Me.Position) > FollowBotSettings.Instance.MaxLootDistance)
            //{
            //    _item = null;
            //    return true;
            //}

            if (!CanFit(_item.Size, Inventories.AvailableInventorySquares))
            {
                GlobalLog.Warn($"[LootItemTask] No room in inventory for {_item.Position.Name}");
                _item.Ignored = true;
                _item = null;
                return true;
            }
			await Coroutines.CloseBlockingWindows();

			WalkablePosition pos = _item.Position;

			if (pos.Distance > 30 || pos.PathDistance > 34)
			{
				if (LogInterval.Elapsed)
				{
					GlobalLog.Debug($"[LootItemTask] Items to pick up: {validItems.Count}");
					GlobalLog.Debug($"[LootItemTask] Moving to {pos}");
				}

                // Cast Phase run if we have it.
                FollowBot.PhaseRun();

				if (!PlayerMoverManager.MoveTowards(pos))
				{
					if (_item.Object != null)
					{
						GlobalLog.Error($"[LootItemTask] Fail to move to {pos}. Marking this item as unwalkable.");
						_item.Unwalkable = true;
						_item = null;
					}
					else
					{
						GlobalLog.Error($"[LootItemTask] Fail to move to {pos}. item Object is null, removing item from the cache and reevaluating it.");
						CombatAreaCache.Current.RemoveItemFromCache(_item);
						_item = null;
					}
				}
				return true;
			}
			WorldItem itemObj = _item.Object;
			if (itemObj == null)
			{
				items.Remove(_item);
				_item = null;
				return true;
			}

            int attempts = ++_item.InteractionAttempts;
			if (attempts > MaxItemPickupAttempts)
			{
				if (_item.Position.Name == CurrencyNames.Mirror)
				{
					string errorName = "[LootItemTask] Fail to pick up the Mirror of Kalandra!!!!!!";
					GlobalLog.Error(errorName);
                }
				else
				{
					GlobalLog.Error("[LootItemTask] All attempts to pick up an item have been spent. Now ignoring it.");
					_item.Ignored = true;
					_item = null;
				}
				return true;
			}

			if (attempts % 2 == 0)
			{
				await PlayerAction.DisableAlwaysHighlight();
			}

            await PlayerAction.EnableAlwaysHighlight();

			GlobalLog.Debug($"[LootItemTask] Now picking up {pos}");

			CachedItem cached = new CachedItem(itemObj.Item);

			int minTimeout = 400;
			int timeout = Math.Max(LatencyTracker.Average * 2, minTimeout);

			LokiPoe.ProcessHookManager.ClearAllKeyStates();
            if (await FastInteraction(itemObj))
            {
                //await Coroutines.LatencyWait();
                if (await Wait.For(() => _item.Object == null, "item pick up", 5, timeout))
                {
                    items.Remove(_item);
                    _item = null;
                    GlobalLog.Info($"[Events] Item looted ({cached.Name})");
                    Utility.BroadcastMessage(this, Events.Messages.ItemLootedEvent, cached);
                }
                return true;
            }
			return true;
        }

        private static async Task<bool> FastInteraction(WorldItem item)
        {
            if (item == null) return false;
            var label = item.WorldItemLabel;
            //if (label.Coordinate.X < LokiPoe.ClientWindowInfo.Client.Left ||
            //    label.Coordinate.Y < LokiPoe.ClientWindowInfo.Client.Top) return false;

            //if (label.Coordinate.X + label.Size.X > LokiPoe.ClientWindowInfo.Client.Right ||
            //    label.Coordinate.Y + label.Size.Y > LokiPoe.ClientWindowInfo.Client.Bottom * 0.85) return false;
            var found = false;
            var point = Vector2i.Zero;
            bool useHighlight = false;
            bool useBound = false;
			if (LokiPoe.Input.Binding.KeyPickup == LokiPoe.ConfigManager.KeyPickupType.UseHighlightKey)
            {
                //GlobalLog.Info($"[FastInteraction] pressing UseHighlightKey Key [{LokiPoe.Input.Binding.highlight_combo.Modifier} + {LokiPoe.Input.Binding.highlight_combo.Key}]");
				LokiPoe.ProcessHookManager.SetKeyState(LokiPoe.Input.Binding.highlight_combo.Key, -32768, LokiPoe.Input.Binding.highlight_combo.Modifier);
                useHighlight = true;
                await Wait.SleepSafe(15);
            }
            if (LokiPoe.Input.Binding.KeyPickup == LokiPoe.ConfigManager.KeyPickupType.UseBoundKey)
            {
                //GlobalLog.Info($"[FastInteraction] pressing UseBoundKey [{LokiPoe.Input.Binding.enable_key_pickup_combo.Modifier} + {LokiPoe.Input.Binding.enable_key_pickup_combo.Key}]");
				LokiPoe.ProcessHookManager.SetKeyState(LokiPoe.Input.Binding.enable_key_pickup_combo.Key, -32768, LokiPoe.Input.Binding.enable_key_pickup_combo.Modifier);
                useBound = true;
				await Wait.SleepSafe(15);
			}

			for (int i = 1; i < 6; i++)
            {
                point = new Vector2i((int)(label.Coordinate.X + (label.Size.X / 7) * i), (int)(label.Coordinate.Y + (label.Size.Y / 2)));
                MouseManager.SetMousePosition(point, false);
                await Wait.SleepSafe(15);
				if (GameController.Instance.Game.IngameState.FrameUnderCursor == item.Entity.Address)
                {
					found = true;
                    break;
                }
            }

            if (!found)
            {
				if (useHighlight)
                {
                    LokiPoe.ProcessHookManager.SetKeyState(LokiPoe.Input.Binding.highlight_combo.Key, 0, LokiPoe.Input.Binding.highlight_combo.Modifier);
                    await Wait.SleepSafe(15);
				}

                if (useBound)
                {
                    LokiPoe.ProcessHookManager.SetKeyState(LokiPoe.Input.Binding.enable_key_pickup_combo.Key, 0, LokiPoe.Input.Binding.enable_key_pickup_combo.Modifier);
                    await Wait.SleepSafe(15);
				}
				return false;
            }
            MouseManager.ClickLMB(point.X, point.Y);
			await Wait.SleepSafe(15, 25);
			if (useHighlight)
            {
                LokiPoe.ProcessHookManager.SetKeyState(LokiPoe.Input.Binding.highlight_combo.Key, 0, LokiPoe.Input.Binding.highlight_combo.Modifier);
                await Wait.SleepSafe(15);
			}

            if (useBound)
            {
                LokiPoe.ProcessHookManager.SetKeyState(LokiPoe.Input.Binding.enable_key_pickup_combo.Key, 0, LokiPoe.Input.Binding.enable_key_pickup_combo.Modifier);
                await Wait.SleepSafe(15);
			}
            return true;
		}
		private static async Task<bool> MoveAway(int min, int max)
		{
			WorldPosition pos = WorldPosition.FindPathablePositionAtDistance(min, max, 5);
			if (pos == null)
			{
				GlobalLog.Debug("[LootItemTask] Fail to find any pathable position at distance.");
				return false;
			}
			await Move.AtOnce(pos, "distant position", 10);
			return true;
		}

		private static bool CanFit(Vector2i size, int availableSquares)
		{
            return LokiPoe.InstanceInfo.GetPlayerInventoryBySlot(InventorySlot.Main).CanFitItem(size);
		}

		public MessageResult Message(Message message)
		{
			string id = message.Id;
			if (id == Events.Messages.AreaChanged)
			{
				_item = null;
                return MessageResult.Processed;
			}
			if (id == "GetCurrentItem")
			{
				message.AddOutput(this, _item);
				return MessageResult.Processed;
			}
			if (id == "SetCurrentItem")
			{
				_item = message.GetInput<CachedWorldItem>();
				return MessageResult.Processed;
			}
			if (id == "ResetCurrentItem")
			{
				_item = null;
				return MessageResult.Processed;
			}

			return MessageResult.Unprocessed;
		}

        #region Unused interface methods

		public async Task<LogicResult> Logic(Logic logic)
		{
			return LogicResult.Unprovided;
		}

		public void Tick()
		{
		}

		public void Start()
		{
		}

		public void Stop()
		{
		}

		public string Name => "LootItemTask";
		public string Description => "Task that handles item looting.";
		public string Author => "Alcor75 Original idea by EXvault";
		public string Version => "3.0";

		#endregion
	}
}
