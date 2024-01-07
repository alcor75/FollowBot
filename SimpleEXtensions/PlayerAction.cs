using System;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.SimpleEXtensions.Positions;
using AreaTransition = DreamPoeBot.Loki.Game.Objects.AreaTransition;
using Portal = DreamPoeBot.Loki.Game.Objects.Portal;

namespace FollowBot.SimpleEXtensions
{
    public static class PlayerAction
    {
        public static async Task<bool> OpenWaypoint()
        {
            WalkablePosition wpPos;
            if (World.CurrentArea.IsTown)
            {
                wpPos = StaticPositions.GetWaypointPosByAct();
                //var wp = LokiPoe.ObjectManager.Waypoint;
                //wpPos = wp.WalkablePosition();
            }
            else
            {
                var wpObj = LokiPoe.ObjectManager.Waypoint;
                if (wpObj == null)
                {
                    GlobalLog.Error("[OpenWaypoint] Fail to find any Waypoint nearby.");
                    return false;
                }
                wpPos = wpObj.WalkablePosition();
            }

            await EnableAlwaysHighlight();

            await wpPos.ComeAtOnce();

            await Interact(LokiPoe.ObjectManager.Waypoint,
                () => LokiPoe.InGameState.WorldUi.IsOpened ||
                      LokiPoe.InGameState.GlobalWarningDialog.IsBetrayalLeaveZoneWarningOverlayOpen, "wold panel opening");

            if (LokiPoe.InGameState.GlobalWarningDialog.IsBetrayalLeaveZoneWarningOverlayOpen)
            {
                GlobalLog.Info("[CloseBlockingWindows] IsBetrayalLeaveZoneWarningOverlayOpen == true. Click Yes.");
                LokiPoe.InGameState.GlobalWarningDialog.ConfirmDialog();
                await Coroutines.LatencyWait();
            }

            await Wait.SleepSafe(200);
            return LokiPoe.InGameState.WorldUi.IsOpened;
        }
        public static async Task<bool> Interact(NetworkObject obj, Func<bool> success, string desc, int timeout = 3000)
        {
            if (obj == null)
            {
                GlobalLog.Error("[Interact] Object for interaction is null.");
                return false;
            }

            var name = obj.Name;
            GlobalLog.Debug($"[Interact] Now going to interact with \"{name}\".");

            await Coroutines.CloseBlockingWindows();
            await Coroutines.FinishCurrentAction();
            await Wait.LatencySleep();
            if (await Coroutines.InteractWith(obj, name == "Tane Octavius"))
            {
                if (name == "Tane Octavius")
                {
                    await Wait.For(() => LokiPoe.InGameState.SellUi.IsOpened, "Tane Sell Inventory", 100, timeout);
                    return false;
                }
                if (!await Wait.For(success, desc, 100, timeout))
                    return false;

                GlobalLog.Debug($"[Interact] \"{name}\" has been successfully interacted.");
                return true;
            }
            GlobalLog.Error($"[Interact] Fail to interact with \"{name}\".");
            await Wait.SleepSafe(300, 500);
            return false;
        }
        public static async Task<bool> Interact(NetworkObject obj)
        {
            //return await Interact(obj._entity);
            if (obj == null)
            {
                GlobalLog.Error("[Interact] Object for interaction is null.");
                return false;
            }

            var name = obj.Name;
            GlobalLog.Debug($"[Interact] Now going to interact with \"{name}\".");

            await Coroutines.CloseBlockingWindows();
            await Coroutines.FinishCurrentAction();
            await Wait.LatencySleep();

            if (await Coroutines.InteractWith(obj))
            {
                GlobalLog.Debug($"[Interact] \"{name}\" has been successfully interacted.");
                return true;
            }
            GlobalLog.Error($"[Interact] Fail to interact with \"{name}\".");
            await Wait.SleepSafe(100, 200);
            return false;
        }
        public static async Task<bool> Interact(NetworkObject obj, int attempts)
        {
            if (obj == null)
            {
                GlobalLog.Error("[Interact] Object for interaction is null.");
                return false;
            }

            var name = obj.Name;
            GlobalLog.Debug($"[Interact] Now going to interact with \"{name}\".");

            for (int i = 1; i <= attempts; i++)
            {
                if (!LokiPoe.IsInGame || LokiPoe.Me.IsDead)
                    break;

                await Coroutines.CloseBlockingWindows();
                await Coroutines.FinishCurrentAction();
                await Wait.LatencySleep();

                if (await Coroutines.InteractWith(obj))
                {
                    GlobalLog.Debug($"[Interact] \"{name}\" has been successfully interacted.");
                    return true;
                }
                GlobalLog.Error($"[Interact] Fail to interact with \"{name}\". Attempt: {i}/{attempts}.");
                await Wait.SleepSafe(100, 200);
            }
            return false;
        }
        public static async Task<bool> Logout()
        {
            GlobalLog.Debug("[Logout] Now going to log out.");

            var err = LokiPoe.EscapeState.LogoutToTitleScreen();
            if (err != LokiPoe.EscapeState.LogoutError.None)
            {
                GlobalLog.Error($"[Logout] Fail to log out. Error: \"{err}\".");
                return false;
            }
            return await Wait.For(() => LokiPoe.IsInLoginScreen, "log out", 500, 5000);
        }
        public static async Task<bool> TryTo(Func<Task<bool>> action, string desc, int attempts, int interval = 1000)
        {
            for (int i = 1; i <= attempts; ++i)
            {
                if (!LokiPoe.IsInGame || LokiPoe.Me.IsDead)
                    break;

                if (desc != null)
                    GlobalLog.Debug($"[TryTo] {desc} attempt: {i}/{attempts}");

                if (await action())
                    return true;

                await Wait.SleepSafe(interval);
            }
            return false;
        }
        public static async Task<bool> GoToHideout()
        {
            bool res = false;
            if ((World.CurrentArea.IsTown || World.CurrentArea.IsMenagerieArea))
            {
                res = await GoToHideoutViaCommand();
            }
            else
                res = await GoToHideoutViaWaypoint();

            return res;
        }
        private static async Task<bool> GoToHideoutViaCommand()
        {
            // Aggressive protection against exceptions, make sure this will never run unless everything is perfect.
            

            GlobalLog.Debug("[GoToHideoutViaCommand] Now going to hideout via chat command.");

            var areaHash = LokiPoe.LocalData.AreaHash;
            GlobalLog.Debug($"[PlayerAction] areHash: {areaHash}");
            var err = LokiPoe.InGameState.ChatPanel.Commands.hideout();
            if (err != LokiPoe.InGameState.ChatResult.None)
            {
                GlobalLog.Error($"[GoToHideoutViaCommand] Fail to use /hideout command. Fail-safe activated. Error: \"{err}\".");
                return false;
            }

            var changed = await Wait.ForHOChange();
            if (!changed)
            {
                GlobalLog.Error("[GoToHideoutViaCommand] Wait.ForAreaChange failed. Fail-safe activated.");
            }
            else
            {
                GlobalLog.Debug($"[PlayerAction] changed: true");
               
            }
            return changed;
        }
        private static async Task<bool> GoToHideoutViaWaypoint()
        {
            if (!LokiPoe.InGameState.WorldUi.IsOpened)
            {
                if (!await OpenWaypoint())
                {
                    GlobalLog.Error("[GoToHideoutViaWaypoint] Fail to open a waypoint.");
                    return false;
                }
            }

            GlobalLog.Debug("[GoToHideoutViaWaypoint] Now going to take a waypoint to hideout.");

            var areaHash = LokiPoe.LocalData.AreaHash;

            var err = LokiPoe.InGameState.WorldUi.GoToHideout();
            if (err != LokiPoe.InGameState.TakeWaypointResult.None)
            {
                GlobalLog.Error($"[GoToHideoutViaWaypoint] Fail to take a waypoint to hideout. Error: \"{err}\".");
                return false;
            }
            return await Wait.ForHOChange();
        }
        public static async Task<bool> TpToTown(bool forceNewPortal = false, bool repeatUntilInTown = true)
        {
            if (ErrorManager.GetErrorCount("TpToTown") > 5)
            {
                GlobalLog.Debug("[TpToTown] We failed to take a portal to town more than 5 times. Now going to log out.");
                return await Logout();
            }
            GlobalLog.Debug("[TpToTown] Now going to open and take a portal to town.");

            var area = World.CurrentArea;

            if (area.IsTown || area.IsHideoutArea)
            {
                GlobalLog.Error("[TpToTown] We are already in town/hideout.");
                return false;
            }
            if (!area.IsOverworldArea && !area.IsMap && !area.IsCorruptedArea && !area.IsMapRoom && !area.IsTempleOfAtzoatl && area.Name != "Syndicate Hideout")
            {
                GlobalLog.Warn($"[TpToTown] Cannot create portals in this area ({area.Name}). Now going to log out.");
                return await Logout();
            }

            Portal portal;

            if (forceNewPortal || (portal = PortalInRangeOf(70)) == null)
            {
                portal = await CreateTownPortal();
                if (portal == null)
                {
                    GlobalLog.Error("[TpToTown] Fail to create a new town portal. Now going to log out.");
                    return await Logout();
                }
            }
            else
            {
                GlobalLog.Debug($"[TpToTown] There is a ready-to-use portal at a distance of {portal.Distance}. Now going to take it.");
            }

            if (!await TakePortal(portal))
            {
                ErrorManager.ReportError("TpToTown");
                return false;
            }

            var newArea = World.CurrentArea;
            if (repeatUntilInTown && newArea.IsCombatArea)
            {
                GlobalLog.Debug($"[TpToTown] After taking a portal we appeared in another combat area ({newArea.Name}). Now calling TpToTown again.");
                return await TpToTown(forceNewPortal);
            }
            GlobalLog.Debug($"[TpToTown] We have been successfully teleported from \"{area.Name}\" to \"{newArea.Name}\".");
            return true;

            //while (!LokiPoe.InGameState.IsRightPanelShown)
            //{
            //    LokiPoe.Input.SimulateKeyEvent(Keys.I, true, false, false);
            //    await Coroutine.Coroutine.Sleep(16);
            //}
            //return true;
        }
        public static async Task<bool> TakeTransition(AreaTransition transition, bool newInstance = false)
        {
            if (transition == null)
            {
                GlobalLog.Error("[TakeTransition] Transition object is null.");
                return false;
            }

            WalkablePosition pos;
            if (transition.Name == "Shrine of the Winds")
            {
                pos = transition.WalkablePosition(7, 10);
            }
            else if (transition.Name == "The Chamber of Sins Level 2")
            {
                pos = transition.WalkablePosition(2, 5);
            }
            else if (transition.Name == "The Crypt Level 1")
            {
                pos = transition.WalkablePosition(2, 5);
            }
            else if (transition.Name == "Tukohama's Keep")
            {
                pos = transition.WalkablePosition(2, 5);
            }
            else
            {
                pos = transition.WalkablePosition();
            }

            var type = transition.TransitionType;

            GlobalLog.Debug($"[TakeTransition] Now going to enter \"{pos.Name}\".");

            if (transition.Name == "Shrine of the Winds")
            {
                await pos.ComeAtOnce(13);
            }
            else if (transition.Name == "Altar of Hunger")
            {
                await new WalkablePosition("Unstuck Position", new Vector2i(1834, 3001), 2, 5).ComeAtOnce(6);
            }
            else if (transition.Name == "The Chamber of Sins Level 2")
            {
                await pos.ComeAtOnce(9);
            }
            else if (transition.Name == "The Crypt Level 1")
            {
                await pos.ComeAtOnce(9);
            }
            else if (LokiPoe.CurrentWorldArea.Name == "The Sceptre of God" ||
                     LokiPoe.CurrentWorldArea.Name == "The Upper Sceptre of God")
            {
                if (transition.Name == "Tower Rooftop")
                {
                    await new WalkablePosition("Unstuck Position", new Vector2i(2691, 423), 2, 5).ComeAtOnce(6);
                }
                else
                    await pos.ComeAtOnce(13);
            }
            else if (transition.Name == "Tukohama's Keep")
            {
                if (transition.Position == new Vector2i(1022, 350))
                {
                    await new WalkablePosition("Unstuck Position", new Vector2i(1024, 315), 2, 5).ComeAtOnce(8);
                }
                else
                    await new WalkablePosition("Unstuck Position", new Vector2i(1232, 323), 2, 5).ComeAtOnce(8);
            }
            else if (transition.Name == "The Quay")
            {
                await new WalkablePosition("Unstuck Position", new Vector2i(transition.Position.X + 10, transition.Position.Y - 10), 2, 6).ComeAtOnce(6);
            }
            else
            {
                await pos.ComeAtOnce();
            }
            await Coroutines.FinishCurrentAction();
            await Wait.SleepSafe(100);

            var hash = LokiPoe.LocalData.AreaHash;
            var myPos = LokiPoe.MyPosition;

            bool entered = newInstance
                ? await CreateNewInstance(transition)
                : await Interact(transition);

            if ((int)type == (int)TransitionType.Local)
            {
                if (!await Wait.For(() => LokiPoe.Me.HasAura("Grace Period") ||
                                          myPos.Distance(LokiPoe.MyPosition) > 15, "local transition", 500, 5000))
                {
                    GlobalLog.Error($"Grace: " +
                                    $"{LokiPoe.Me.HasAura("Grace Period")}, " +
                                    $"LastPos: {myPos}, CurrentPos: {LokiPoe.Me.Position}, " +
                                    $"Distance: {myPos.Distance(LokiPoe.MyPosition)}");
                    return false;
                }

                await Wait.SleepSafe(250);
            }
            else
            {
                if (!await Wait.ForAreaChange(hash))
                    return false;
            }
            GlobalLog.Debug($"[TakeTransition] \"{pos.Name}\" has been successfully entered.");
            return true;
        }
        public static async Task<bool> TakeTransitionByName(string name, bool newInstance = false)
        {
            var transition = LokiPoe.ObjectManager.Objects.Closest<AreaTransition>(a => a.Name == name);
            return await TakeTransition(transition, newInstance);
        }
        public static async Task<bool> TakeWaypoint(AreaInfo area, bool newInstance = false)
        {
            if (!LokiPoe.InGameState.WorldUi.IsOpened)
            {
                if (!await OpenWaypoint())
                {
                    GlobalLog.Error("[TakeWaypoint] Fail to open a waypoint.");
                    return false;
                }
            }

            GlobalLog.Debug($"[TakeWaypoint] Now going to take a waypoint to {area}");

            var areaHash = LokiPoe.LocalData.AreaHash;

            var err = LokiPoe.InGameState.WorldUi.TakeWaypoint(area.Id, newInstance);
            if (err != LokiPoe.InGameState.TakeWaypointResult.None)
            {
                GlobalLog.Error($"[TakeWaypoint] Fail to take a waypoint to {area}. Error: \"{err}\".");
                return false;
            }
            return await Wait.ForAreaChange(areaHash);
        }
        private static async Task<bool> CreateNewInstance(AreaTransition transition)
        {
            var name = transition.Name;
            if (!await Coroutines.InteractWith(transition, true))
            {
                GlobalLog.Error($"[CreateNewInstance] Fail to interact with \"{name}\" transition.");
                return false;
            }

            if (!await Wait.For(() => LokiPoe.InGameState.InstanceManagerUi.IsOpened, "instance manager opening"))
                return false;

            await Wait.ArtificialDelay();

            GlobalLog.Debug($"[CreateNewInstance] Creating new instance for \"{name}\".");

            var err = LokiPoe.InGameState.InstanceManagerUi.JoinNewInstance();
            if (err != LokiPoe.InGameState.JoinInstanceResult.None)
            {
                GlobalLog.Error($"[CreateNewInstance] Fail to create a new instance. Error: \"{err}\".");
                return false;
            }
            GlobalLog.Debug($"[CreateNewInstance] New instance for \"{name}\" has been successfully created.");
            return true;
        }
        private static Portal PortalInRangeOf(int range)
        {
            return LokiPoe.ObjectManager.Objects
                .Closest<Portal>(p => p.IsPlayerPortal() && p.Distance <= range && p.PathDistance() <= range + 3);
        }
        public static async Task<Portal> CreateTownPortal()
        {
            var portalSkill = LokiPoe.InGameState.SkillBarHud.Skills.FirstOrDefault(s => s.Name == "Portal" && s.IsOnSkillBar);
            if (portalSkill != null)
            {
                await Coroutines.FinishCurrentAction();
                await Wait.SleepSafe(100);
                var err = LokiPoe.InGameState.SkillBarHud.Use(portalSkill.Slot, false);
                if (err != LokiPoe.InGameState.UseResult.None)
                {
                    GlobalLog.Error($"[CreateTownPortal] Fail to cast portal skill. Error: \"{err}\".");
                    return null;
                }
                await Coroutines.FinishCurrentAction();
                await Wait.SleepSafe(100);
            }
            else
            {
                var portalScroll = Inventories.InventoryItems
                    .Where(i => i.Name == CurrencyNames.Portal)
                    .OrderBy(i => i.StackCount)
                    .FirstOrDefault();

                if (portalScroll == null)
                {
                    GlobalLog.Error("[CreateTownPortal] Out of portal scrolls.");
                    return null;
                }

                int itemId = portalScroll.LocalId;

                if (!await Inventories.OpenInventory())
                    return null;

                await Coroutines.FinishCurrentAction();
                await Wait.SleepSafe(100);

                var err = LokiPoe.InGameState.InventoryUi.InventoryControl_Main.UseItem(itemId);
                if (err != UseItemResult.None)
                {
                    GlobalLog.Error($"[CreateTownPortal] Fail to use a Portal Scroll. Error: \"{err}\".");
                    return null;
                }

                await Wait.ArtificialDelay();

                await Coroutines.CloseBlockingWindows();
            }

            Portal portal = null;
            await Wait.For(() => (portal = PortalInRangeOf(40)) != null, "portal spawning");
            return portal;
        }
        public static async Task EnableAlwaysHighlight()
        {
            if (LokiPoe.ConfigManager.IsAlwaysHighlightEnabled)
                return;

            GlobalLog.Info("[EnableAlwaysHighlight] Now enabling always highlight.");
            LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.highlight_toggle, true, false, false);
            await Wait.For(() => LokiPoe.ConfigManager.IsAlwaysHighlightEnabled, "EnableAlwaysHighlight", 10, 100);//.SleepSafe(30);
        }
        public static async Task DisableAlwaysHighlight()
        {
            if (!LokiPoe.ConfigManager.IsAlwaysHighlightEnabled)
                return;

            GlobalLog.Info("[DisableAlwaysHighlight] Now disabling always highlight.");
            LokiPoe.Input.SimulateKeyEvent(LokiPoe.Input.Binding.highlight_toggle, true, false, false);
            await Wait.For(() => !LokiPoe.ConfigManager.IsAlwaysHighlightEnabled, "DisableAlwaysHighlight", 10, 100);//.Sleep(30);
        }
        public static async Task<bool> TakePortal(Portal portal)
        {
            if (portal == null)
            {
                GlobalLog.Error("[TakePortal] Portal object is null.");
                return false;
            }

            var pos = portal.WalkablePosition();
            await DisableAlwaysHighlight();
            await pos.ComeAtOnce();
            await Wait.SleepSafe(200);

            GlobalLog.Debug($"[TakePortal] Now going to take portal to \"{pos.Name}\".");

            var hash = LokiPoe.LocalData.AreaHash;

            if (!LokiPoe.Input.HighlightObject(portal))
                return false;

            if (!await Interact(portal,
            () => !LokiPoe.IsInGame, "loading screen"))
                return false;

            if (!await Wait.ForAreaChange(hash))
                return false;

            GlobalLog.Debug($"[TakePortal] Portal to \"{pos.Name}\" has been successfully taken.");
            return true;
        }

        public static async Task<bool> MoveAway(int min, int max)
        {
            WorldPosition pos = WorldPosition.FindPathablePositionAtDistance(min, max, 5);
            if (pos == null)
            {
                pos = new WorldPosition(LokiPoe.Me.Position);
            }

            Vector2i newPosition = pos.AsVector;
            newPosition += new Vector2i(LokiPoe.Random.Next(-2, 3), LokiPoe.Random.Next(-2, 3));
            if (!Move.Towards(newPosition, "away"))
            {
                ErrorManager.ReportError();
                return false;
            }

            return true;
        }
    }
}
