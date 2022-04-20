using System.IO;
using System.Linq;
using System.Reflection;
using DreamPoeBot.Loki.Bot;

namespace FollowBot.SimpleEXtensions
{
    public static class BotStructure
    {
        public const string GetTaskManagerMessage = "GetTaskManager";

        public static readonly string PathToLogs = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");

        public static TaskManager TaskManager
        {
            get
            {
                var bot = BotManager.Current;

                if (bot is ITaskManagerHolder holder)
                    return holder.GetTaskManager();

                GlobalLog.Debug($"[BotStructure] \"{bot.Name}\" does not implement ITaskManagerHolder interface.");

                var msg = new Message(GetTaskManagerMessage);
                bot.Message(msg);
                var taskManager = msg.GetOutput<TaskManager>();

                if (taskManager != null)
                    return taskManager;

                GlobalLog.Error($"[BotStructure] \"{bot.Name}\" does not process \"{GetTaskManagerMessage}\" message.");
                ErrorManager.ReportCriticalError();
                return null;
            }
        }

        public static void AddTask(ITask task, string name, TaskPosition type)
        {
            bool added = false;
            switch (type)
            {
                case TaskPosition.Before:
                    added = TaskManager.AddBefore(task, name);
                    break;

                case TaskPosition.After:
                    added = TaskManager.AddAfter(task, name);
                    break;

                case TaskPosition.Replace:
                    added = TaskManager.Replace(name, task);
                    break;

                default:
                    GlobalLog.Error($"[BotStructure] Unknown task add type \"{type}\".");
                    break;
            }
            if (!added)
            {
                GlobalLog.Error($"[BotStructure] Fail to add \"{name}\".");
                BotManager.Stop();
            }
        }

        public static void RemoveTask(string name)
        {
            if (!TaskManager.Remove(name))
            {
                GlobalLog.Error($"[BotStructure] Fail to remove \"{name}\".");
                BotManager.Stop();
            }
        }

        public static IPlugin GetPlugin(string name)
        {
            return PluginManager.Plugins.FirstOrDefault(p => p.Name == name);
        }

        public static IPlugin GetEnabledPlugin(string name)
        {
            return PluginManager.EnabledPlugins.Find(p => p.Name == name);
        }

        public static bool IsPluginEnabled(string name)
        {
            return PluginManager.EnabledPlugins.Exists(p => p.Name == name);
        }
    }
}
