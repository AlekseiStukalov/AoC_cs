using Common.Helpers.Enums;
using Common.Helpers.DataStructures;

namespace Common.Helpers
{
    public class Traveller : IEquatable<Traveller>
    {
        public Position Pos;

        public Direction CurrentDirection;
        public int WalkedDistance;
        public int WayWeight;

        public Traveller(Position? pos)
        {
            Pos = pos ?? new Position(0, 0);
        }

        public Traveller()
        {
            Pos = new Position(0, 0);
        }

        public Traveller GetNextTraveller(int xOffset, int yOffset)
        {
            return new Traveller(new Position(this.Pos.X + xOffset, this.Pos.Y + yOffset))
            {
                WalkedDistance = this.WalkedDistance + 1
            };
        }

        public override int GetHashCode()
        {
            return Pos.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Traveller))
            {
                return false;
            }
            return Equals(obj as Traveller);
        }

        public bool Equals(Traveller? other)
        {
            if (other == null)
                return false;

            return other.GetHashCode() == this.GetHashCode();
        }
    }
}

