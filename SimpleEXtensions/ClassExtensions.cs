using System;
using System.Collections;
using System.Collections.Generic;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.SimpleEXtensions.Positions;
using JetBrains.Annotations;

namespace FollowBot.SimpleEXtensions
{
    public static class ClassExtensions
    {
        public static bool EqualsIgnorecase(this string thisStr, string str)
        {
            return thisStr.Equals(str, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnorecase(this string thisStr, string str)
        {
            return thisStr.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static void LogProperties(this object obj)
        {
            var type = obj.GetType();
            var typeName = type.Name;
            foreach (var p in obj.GetType().GetProperties())
            {
                GlobalLog.Info($"[{typeName}] {p.Name}: {p.GetValue(obj) ?? "null"}");
            }
        }

        public static Item FindItemByPos(this DreamPoeBot.Loki.Game.Inventory inventory, Vector2i pos)
        {
            return inventory.Items.Find(i => i.LocationTopLeft == pos);
        }

        public static Item FindItemById(this DreamPoeBot.Loki.Game.Inventory inventory, int id)
        {
            return inventory.Items.Find(i => i.LocalId == id);
        }

        public static Rarity RarityLite(this Item item)
        {
            var mods = item.Components.ModsComponent;
            return mods == null ? Rarity.Normal : mods.Rarity;
        }

        public static WorldPosition WorldPosition(this NetworkObject obj)
        {
            return new WorldPosition(obj.Position);
        }

        public static WalkablePosition WalkablePosition(this NetworkObject obj, int step = 10, int radius = 30)
        {
            return new WalkablePosition(obj.Name, obj.Position, step, radius);
        }

        public static bool PathExists(this NetworkObject obj)
        {
            return ExilePather.PathExistsBetween(LokiPoe.MyPosition, obj.Position);
        }

        public static float PathDistance(this NetworkObject obj)
        {
            return ExilePather.PathDistance(LokiPoe.MyPosition, obj.Position);
        }


        public static bool IsPlayerPortal(this DreamPoeBot.Loki.Game.Objects.Portal p)
        {
            if (!p.IsTargetable)
                return false;

            var m = p.Metadata;
            return m == "Metadata/MiscellaneousObjects/PlayerPortal" ||
                   m == "Metadata/MiscellaneousObjects/MapReturnPortal" ||
                   m == "Metadata/MiscellaneousObjects/MultiplexPortal";
        }

        public static bool LeadsTo(this DreamPoeBot.Loki.Game.Objects.Portal p, AreaInfo area)
        {
            var dest = p.Components.PortalComponent?.Area;
            return dest != null && dest == area;
        }

        public static bool LeadsTo(this DreamPoeBot.Loki.Game.Objects.Portal p, Func<DatWorldAreaWrapper, bool> match)
        {
            var dest = p?.Components?.PortalComponent?.Destination;
            return dest != null && match(dest);
        }

        public static bool LeadsTo(this DreamPoeBot.Loki.Game.Objects.AreaTransition a, AreaInfo area)
        {
            if ((int)a.TransitionType == (int)TransitionType.Local)
                return false;

            var dest = a.Components.AreaTransitionComponent?.Destination;
            return dest != null && dest == area;
        }

        public static bool LeadsTo(this DreamPoeBot.Loki.Game.Objects.AreaTransition a, Func<DatWorldAreaWrapper, bool> match)
        {
            if ((int)a.TransitionType == (int)TransitionType.Local)
                return false;

            var dest = a.Components.AreaTransitionComponent?.Destination;
            return dest != null && match(dest);
        }

        public static T Fresh<T>(this T obj) where T : NetworkObject
        {
            return LokiPoe.ObjectManager.Objects.Find(o => o.Id == obj.Id) as T;
            //return LokiPoe.ObjectManager.Objects.Find(o => o.Position == obj.Position) as T;
        }

        [CanBeNull]
        public static T Closest<T>(this IEnumerable<T> collection) where T : NetworkObject
        {
            T closest = null;
            foreach (var element in collection)
            {
                if (closest == null || element.DistanceSqr < closest.DistanceSqr)
                    closest = element;
            }
            return closest;
        }

        [CanBeNull]
        public static T Closest<T>(this IEnumerable collection) where T : NetworkObject
        {
            T closest = null;
            foreach (var element in collection)
            {
                var typed = element as T;
                if (typed != null)
                {
                    if (closest == null || typed.DistanceSqr < closest.DistanceSqr)
                    {
                        closest = typed;
                    }
                }
            }
            return closest;
        }
        public static List<T> All<T>(this IEnumerable collection) where T : NetworkObject
        {
            List<T> all = new List<T>();
            foreach (var element in collection)
            {
                var typed = element as T;
                if (typed != null)
                {
                    all.Add(typed);
                }
            }
            return all;
        }
        [CanBeNull]
        public static T Closest<T>(this IEnumerable<T> collection, Func<T, bool> match) where T : NetworkObject
        {
            T closest = null;
            foreach (var element in collection)
            {
                if (match(element))
                {
                    if (closest == null || element.DistanceSqr < closest.DistanceSqr)
                        closest = element;
                }
            }
            return closest;
        }

        [CanBeNull]
        public static T Closest<T>(this IEnumerable collection, Func<T, bool> match) where T : NetworkObject
        {
            T closest = null;
            foreach (var element in collection)
            {
                var typed = element as T;
                if (typed != null && match(typed))
                {
                    if (closest == null || typed.DistanceSqr < closest.DistanceSqr)
                        closest = typed;
                }
            }
            return closest;
        }

        [CanBeNull]
        public static T Random<T>(this IEnumerable collection, Func<T, bool> match) where T : NetworkObject
        {
            T closest = null;
            List<T> list = new List<T>();
            foreach (var element in collection)
            {
                var typed = element as T;
                if (typed != null && match(typed))
                {
                    list.Add(typed);
                }
            }
            return list.Count == 0 ? null : list[LokiPoe.Random.Next(list.Count)];
        }

        [CanBeNull]
        public static T FirstOrDefault<T>(this IEnumerable collection) where T : class
        {
            foreach (var element in collection)
            {
                var t = element as T;
                if (t != null)
                    return t;
            }
            return null;
        }

        [CanBeNull]
        public static T FirstOrDefault<T>(this IEnumerable collection, Func<T, bool> match) where T : class
        {
            foreach (var element in collection)
            {
                var t = element as T;
                if (t != null && match(t))
                    return t;
            }
            return null;
        }

        public static bool Any<T>(this IEnumerable collection, Func<T, bool> match) where T : class
        {
            foreach (var element in collection)
            {
                var t = element as T;
                if (t != null && match(t))
                    return true;
            }
            return false;
        }

        public static IEnumerable<T> Where<T>(this IEnumerable collection, Func<T, bool> match) where T : class
        {
            foreach (var element in collection)
            {
                var t = element as T;
                if (t != null && match(t))
                    yield return t;
            }
        }

        public static IEnumerable<T> Valid<T>(this IEnumerable<T> collection) where T : CachedObject
        {
            foreach (var element in collection)
            {
                if (!element.Ignored && !element.Unwalkable)
                    yield return element;
            }
        }

        public static IEnumerable<T> Valid<T>(this IEnumerable<T> collection, Func<T, bool> match) where T : CachedObject
        {
            foreach (var element in collection)
            {
                if (!element.Ignored && !element.Unwalkable && match(element))
                    yield return element;
            }
        }

        [CanBeNull]
        public static T ClosestValid<T>(this IEnumerable<T> collection) where T : CachedObject
        {
            T closest = null;
            foreach (var element in collection)
            {
                if (!element.Ignored && !element.Unwalkable)
                {
                    if (closest == null || element.Position.DistanceSqr < closest.Position.DistanceSqr)
                        closest = element;
                }
            }
            return closest;
        }

        [CanBeNull]
        public static T ClosestValid<T>(this IEnumerable<T> collection, Func<T, bool> match) where T : CachedObject
        {
            T closest = null;
            foreach (var element in collection)
            {
                if (!element.Ignored && !element.Unwalkable && match(element))
                {
                    if (closest == null || element.Position.DistanceSqr < closest.Position.DistanceSqr)
                        closest = element;
                }
            }
            return closest;
        }
    }
}
