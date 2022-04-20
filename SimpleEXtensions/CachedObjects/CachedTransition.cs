using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.SimpleEXtensions.Positions;

namespace FollowBot.SimpleEXtensions
{
    public class CachedTransition : CachedObject
    {
        public TransitionType Type { get; }
        public DatWorldAreaWrapper Destination { get; }
        public bool Visited { get; set; }
        public bool LeadsBack { get; set; }
        public string Name { get; }

        public CachedTransition(int id, WalkablePosition position, TransitionType type, DatWorldAreaWrapper destination)
            : base(id, position)
        {
            Type = type;
            Destination = destination;
            Name = Object?.Name;
        }

        public new AreaTransition Object => GetObject() as AreaTransition;
    }
}
