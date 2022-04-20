using System.Collections.Generic;
using System.Text.RegularExpressions;
using DreamPoeBot.Loki.Game;
using FollowBot.SimpleEXtensions.Positions;

namespace FollowBot.SimpleEXtensions
{
    public static class Tgt
    {
        public static WorldPosition FindFirst(string tgtName)
        {
            var regex = CreateRegex(tgtName);
            var tgtEntries = LokiPoe.TerrainData.TgtEntries;
            for (int i = 0; i < tgtEntries.GetLength(0); i++)
            {
                for (int j = 0; j < tgtEntries.GetLength(1); j++)
                {
                    var entry = tgtEntries[i, j];
                    if (entry == null) continue;
                    if (regex.IsMatch(entry.TgtName))
                    {
                        return new WorldPosition(i * 23, j * 23);
                    }
                }
            }
            return null;
        }

        public static List<WorldPosition> FindAll(string tgtName)
        {
            var regex = CreateRegex(tgtName);
            var positions = new List<WorldPosition>();
            var tgtEntries = LokiPoe.TerrainData.TgtEntries;
            for (int i = 0; i < tgtEntries.GetLength(0); i++)
            {
                for (int j = 0; j < tgtEntries.GetLength(1); j++)
                {
                    var entry = tgtEntries[i, j];
                    if (entry == null) continue;
                    if (regex.IsMatch(entry.TgtName))
                    {
                        positions.Add(new WorldPosition(i * 23, j * 23));
                    }
                }
            }
            return positions;
        }

        public static WorldPosition FindWaypoint()
        {
            var positions = new List<WorldPosition>();
            var tgtEntries = LokiPoe.TerrainData.TgtEntries;
            for (int i = 0; i < tgtEntries.GetLength(0); i++)
            {
                for (int j = 0; j < tgtEntries.GetLength(1); j++)
                {
                    var entry = tgtEntries[i, j];
                    if (entry == null) continue;

                    var name = entry.TgtName;
                    if (name.ContainsIgnorecase("waypoint") && !name.Contains("waypoint_broken"))
                    {
                        positions.Add(new WorldPosition(i * 23, j * 23));
                    }
                }
            }
            if (positions.Count == 0)
            {
                GlobalLog.Error("[FindWaypoint] Fail to find any waypoint tgt.");
                return null;
            }
            foreach (var pos in positions)
            {
                if (pos.PathExists) return pos;
            }
            return positions[0].GetWalkable(10, 20);
        }

        private static Regex CreateRegex(string tgtName)
        {
            return new Regex(Regex.Escape(tgtName).Replace(@"\?", "[0-9]"));
        }
    }
}
