using System;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;

namespace FollowBot.SimpleEXtensions
{
    public class AreaInfo : IEquatable<AreaInfo>
    {
        public readonly string Id;
        public string Name;

        public bool IsCurrentArea => LokiPoe.LocalData.WorldArea.Id == Id;
        public bool IsWaypointOpened => LokiPoe.InstanceInfo.AvailableWaypoints.ContainsKey(Id);

        public AreaInfo(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static implicit operator AreaInfo(DatWorldAreaWrapper area)
        {
            return new AreaInfo(area.Id, area.Name);
        }

        public bool Equals(AreaInfo other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AreaInfo);
        }

        public static bool operator ==(AreaInfo left, AreaInfo right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (((object)left == null) || ((object)right == null))
                return false;

            return left.Id == right.Id;
        }

        public static bool operator !=(AreaInfo left, AreaInfo right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"\"{Name}\" ({Id})";
        }
    }
}
