using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;

namespace FollowBot.SimpleEXtensions
{
    public static class Wait
    {
        //private static Stopwatch _runtimePause = Stopwatch.StartNew();
        //private static double _genericPauseFactor = 0.01;
        //private static double _townPauseFactor = 0.01;
        public static async Task<bool> For(Func<bool> condition, string desc, int step = 100, int timeout = 3000)
        {
            return await For(condition, desc, () => step, timeout);
        }
        public static async Task<bool> For(Func<bool> condition, string desc, Func<int> step, int timeout = 3000)
        {
            if (condition())
                return true;

            var timer = Stopwatch.StartNew();
            while (timer.ElapsedMilliseconds < timeout)
            {
                GlobalLog.Debug($"[WaitFor] Waiting for {desc} ({Math.Round(timer.ElapsedMilliseconds / 1000f, 2)}/{timeout / 1000f})");
                if (condition())
                    return true;
            }
            GlobalLog.Error($"[WaitFor] Wait for {desc} timeout.");
            return false;
        }
        public static async Task Sleep(int ms)
        {
            var timeout = Math.Max(LatencyTracker.Current, ms);
            await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(timeout);
        }
        public static async Task SleepSafe(int ms)
        {
            var timeout = Math.Max(LatencyTracker.Current, ms);
            await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(timeout);
        }
        public static async Task SleepSafe(int min, int max)
        {
            int latency = LatencyTracker.Current;
            if (latency > max)
            {
                await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(latency);
            }
            else
            {
                await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(LokiPoe.Random.Next(min, max + 1));
            }
        }
        public static async Task LatencySleep()
        {
            var timeout = Math.Max((int)(LatencyTracker.Current * 1.15), 25);
            GlobalLog.Debug($"[LatencySleep] {timeout} ms.");
            await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(timeout);
        }
        public static async Task ArtificialDelay()
        {
            var ms = LokiPoe.Random.Next(15, 40);
            GlobalLog.Debug($"[ArtificialDelay] Now waiting for {ms} ms.");
            await DreamPoeBot.Loki.Coroutine.Coroutine.Sleep(ms);
        }
        public static async Task<bool> ForAreaChange(uint areaHash, int timeout = 60000)
        {
            if (await For(() => LokiPoe.StateManager.IsAreaLoadingStateActive, "loading screen", 100, 3000))
            {
                return await For(() => LokiPoe.IsInGame, "is ingame", 200, timeout);
            }

            return false;
            //return await For(() => ExilePather.AreaHash != areaHash, "area change", 500, timeout);
        }
        public static async Task<bool> ForHOChange(int timeout = 60000)
        {
            if (await For(() => LokiPoe.StateManager.IsAreaLoadingStateActive, "loading screen", 100, 3000))
            {
                return await For(() => LokiPoe.IsInGame, "is ingame", 200, timeout);
            }

            return false;
        }

    }
}
