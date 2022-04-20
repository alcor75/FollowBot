using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using log4net;

namespace FollowBot
{
    public class LevelGemsTask : ITask
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();
        private readonly WaitTimer _levelWait = WaitTimer.FiveSeconds;
        private bool _needsToUpdate = true;
        private bool _needsToCloseInventory;

        public string Name { get { return "LevelGemsTask"; } }
        public string Description { get { return "This task will Level gems."; } }
        public string Author { get { return "Alcor75"; } }
        public string Version { get { return "0.0.0.1"; } }


        public void Start()
        {

        }
        public void Stop()
        {

        }
        public void Tick()
        {

        }

        public async Task<bool> Run()
        {
            // Don't update while we are not in the game.
            if (!LokiPoe.IsInGame)
            {
                return false;
            }
            // Don't try to do anything when the escape state is active.
            if (LokiPoe.StateManager.IsEscapeStateActive)
            {
                return false;
            }

            // Don't level skill gems if we're dead.
            if (LokiPoe.Me.IsDead)
            {
                return false;
            }
            // Can't level skill gems under this scenario either.
            if (LokiPoe.InGameState.SkillsUi.IsOpened)
            {
                return false;
            }
            // Can't level gems when favor Ui is open
            if (LokiPoe.InGameState.RitualFavorsUi.IsOpened)
            {
                return false;
            }
            // Only check for skillgem leveling at a fixed interval.
            if (!_needsToUpdate && !_levelWait.IsFinished)
            {
                return false;
            }

            // If we have icons on the hud to process.
            if (LokiPoe.InGameState.SkillGemHud.AreIconsDisplayed)
            {
                // If the InventoryUi is already opened, skip this logic and let the next set run.
                if (!LokiPoe.InGameState.InventoryUi.IsOpened)
                {
                    // We need to close blocking windows.
                    await Coroutines.CloseBlockingWindows();

                    // We need to let skills finish casting, because of 2.6 changes.
                    await Coroutines.FinishCurrentAction();
                    await Coroutines.LatencyWait();

                    LokiPoe.InGameState.HandlePendingLevelUpResult res = LokiPoe.InGameState.SkillGemHud.HandlePendingLevelUps(eval);

                    Log.InfoFormat("[LevelGemsTask] SkillGemHud.HandlePendingLevelUps returned {0}.", res);

                    return false;
                }
            }

            if (LokiPoe.InGameState.InventoryUi.IsOpened)
            {
                _needsToCloseInventory = false;
            }
            else
            {
                _needsToCloseInventory = true;
            }
            if (_needsToUpdate)
            {
                // We need the inventory panel open.
                if (!await SimpleEXtensions.Inventories.OpenInventory())
                {
                    Log.ErrorFormat("[LevelGemsTask] OpenInventoryPanel failed.");
                    return false;
                }
            retry:
                // If we have icons on the inventory ui to process.
                // This is only valid when the inventory panel is opened.
                if (LokiPoe.InGameState.InventoryUi.AreIconsDisplayed)
                {
                    LokiPoe.InGameState.HandlePendingLevelUpResult res = LokiPoe.InGameState.InventoryUi.HandlePendingLevelUps(eval);

                    Log.InfoFormat("[LevelGemsTask] InventoryUi.HandlePendingLevelUps returned {0}.", res);
                    if (res == LokiPoe.InGameState.HandlePendingLevelUpResult.GemDismissed ||
                        res == LokiPoe.InGameState.HandlePendingLevelUpResult.GemLeveled)
                    {
                        goto retry;
                    }
                }
            }
            
            // Just wait 5-10s between checks.
            _levelWait.Reset(TimeSpan.FromMilliseconds(LokiPoe.Random.Next(5000, 10000)));

            if (_needsToCloseInventory)
            {
                await Coroutines.CloseBlockingWindows();
                _needsToCloseInventory = false;
            }

            _needsToUpdate = false;
            return false;
        }


        public async Task<LogicResult> Logic(Logic logic)
        {
            return LogicResult.Unprovided;
        }

        public MessageResult Message(Message message)
        {
            bool handled = false;
            if (message.Id == "player_leveled_event")
            {
                _needsToUpdate = true;
                handled = true;
            }
            return handled ? MessageResult.Processed : MessageResult.Unprocessed;
        }

        Func<Inventory, Item, Item, bool> eval = (inv, holder, gem) =>
        {
            // Ignore any "globally ignored" gems. This just lets the user move gems around
            // equipment, without having to worry about where or what it is.
            if (ContainsHelper(gem.Name, gem.SkillGemLevel))
            {
                if (FollowBotSettings.Instance.GemDebugStatements)
                {
                    Log.DebugFormat("[LevelGemsTask] {0}[Lev: {1}] => {2}.", gem.Name, gem.SkillGemLevel, "Is contained in GlobalNameIgnoreList");
                }

                return false;
            }

            // Now look though the list of skillgem strings to level, and see if the current gem matches any of them.
            string ss = string.Format("{0} [{1}: {2}]", gem.Name, inv.PageSlot, holder.GetSocketIndexOfGem(gem));



            ObservableCollection<string> gemsToConsider = new ObservableCollection<string>();
            if (FollowBotSettings.Instance.LevelOffhandOnly)
            {
                ObservableCollection<FollowBotSettings.SkillGemEntry> userSkillGems = FollowBotSettings.Instance.UserSkillGemsInOffHands;
                foreach (FollowBotSettings.SkillGemEntry g in userSkillGems)
                {
                    if (FollowBotSettings.Instance.GemDebugStatements)
                        gemsToConsider.Add(string.Format("{0} [{1}: {2}]", g.Name, g.InventorySlot, g.SocketIndex));
                }
            }
            else if (FollowBotSettings.Instance.LevelAllGems)
            {
                ObservableCollection<FollowBotSettings.SkillGemEntry> userSkillGems = FollowBotSettings.Instance.UserSkillGems;
                foreach (FollowBotSettings.SkillGemEntry g in userSkillGems)
                {
                    if (FollowBotSettings.Instance.GemDebugStatements)
                        gemsToConsider.Add(string.Format("{0} [{1}: {2}]", g.Name, g.InventorySlot, g.SocketIndex));
                }
            }

            foreach (string str in gemsToConsider)
            {
                if (str.Equals(ss, StringComparison.OrdinalIgnoreCase))
                {
                    if (FollowBotSettings.Instance.GemDebugStatements)
                    {
                        Log.DebugFormat("[LevelGemsTask] {0} => {1}.", gem.Name, str);
                    }
                    return true;
                }
            }

            // No match, we shouldn't level this gem.
            return false;
        };
        private static bool ContainsHelper(string name, int level)
        {
            foreach (string entry in FollowBotSettings.Instance.GlobalNameIgnoreList)
            {
                string[] ignoreArray = entry.Split(',');
                if (ignoreArray.Length == 1)
                {
                    if (ignoreArray[0].Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                else
                {
                    if (ignoreArray[0].Equals(name, StringComparison.OrdinalIgnoreCase) && level >= Convert.ToInt32(ignoreArray[1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
