using DreamPoeBot.Common;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;

namespace FollowBot.SimpleEXtensions.CachedObjects
{
    public class CachedItem
    {
        public string Name { get; }
        public string FullName { get; }
        public string Metadata { get; }
        public string Class { get; }
        public CompositeItemType Type { get; }
        public Rarity Rarity { get; }
        public int Quality { get; }
        public int ItemLevel { get; }
        public int StackCount { get; }
        public bool IsIdentified { get; }
        public Vector2i Size { get; }
        public int SkillGemLevel { get; }

        public CachedItem(Item item)
        {
            Name = item.Name;
            FullName = item.FullName;
            Metadata = item.Metadata;
            Class = item.Class;
            Type = item.CompositeType;
            Rarity = item.Rarity;
            Quality = item.Quality;
            ItemLevel = item.ItemLevel;
            StackCount = item.StackCount;
            IsIdentified = item.IsIdentified;
            Size = item.Size;
            SkillGemLevel = item.SkillGemLevel;
        }
    }
}
