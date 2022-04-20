using System.Threading.Tasks;
using DreamPoeBot.Common;
using FollowBot.SimpleEXtensions.Global;

namespace FollowBot.SimpleEXtensions.Positions
{
    public class WalkablePosition : WorldPosition
    {
        public string Name { get; }
        public virtual bool Initialized { get; set; }

        protected int Step;
        protected int Radius;

        public WalkablePosition(string name, Vector2i vector, int step = 10, int radius = 30) : base(vector)
        {
            Name = name;
            Step = step;
            Radius = radius;
        }

        public WalkablePosition(string name, int x, int y, int step = 10, int radius = 30)
            : base(x, y)
        {
            Name = name;
            Step = step;
            Radius = radius;
        }

        public void Come()
        {
            if (!Initialized)
                HardInitialize();

            Move.TowardsWalkable(Vector, Name);
        }

        public async Task ComeAtOnce(int distance = 20)
        {
            if (!Initialized)
                HardInitialize();

            await Move.AtOnce(Vector, Name, distance);
        }

        public bool TryCome()
        {
            if (!Initialized && !Initialize())
                return false;

            return Move.Towards(Vector, Name);
        }

        public async Task<bool> TryComeAtOnce(int distance = 20)
        {
            if (!Initialized && !Initialize())
                return false;

            await Move.AtOnce(Vector, Name, distance);
            return true;
        }

        public virtual bool Initialize()
        {
            if (!FindWalkable())
            {
                GlobalLog.Debug($"[WalkablePosition] Fail to find any walkable position for {this}");
                return false;
            }
            Initialized = true;
            return true;
        }

        protected virtual void HardInitialize()
        {
            if (!FindWalkable())
            {
                GlobalLog.Error($"[WalkablePosition] Fail to find any walkable position for {this}");
                var area = World.CurrentArea;
                Travel.RequestNewInstance(area);
                //await Travel.To(area);
                //ErrorManager.ReportCriticalError();
                return;
            }
            Initialized = true;
        }

        protected bool FindWalkable()
        {
            if (PathExists)
                return true;

            GlobalLog.Debug($"[WalkablePosition] {this} is unwalkable.");

            var walkable = FindPositionForMove(this, Step, Radius);

            if (walkable == null)
                return false;

            GlobalLog.Debug($"[WalkablePosition] Walkable position has been found at {walkable.AsVector} ({Vector.Distance(walkable)} away from original position).");
            Vector = walkable;
            return true;
        }

        public override string ToString()
        {
            return $"\"{Name}\" at {Vector} (distance: {Distance})";
        }
    }
}
