using System.Text;

namespace Common.Helpers.DataStructures
{
	public class MapNode: IEquatable<MapNode>
	{
        private int Hash;

        public int DeixtraMark;
        public int DeixtraArrayIndex;

        public int Weight;
        public Position Pos;
        public string Name;
        public char Symb;
        public List<MapNode> Neighbours;
        public bool IsVisited;
        public readonly bool HasCoordinates;

        public MapNode(Position pos, int weight = 1, bool calcHashFromPos = false, char symb = '\n')
        {
            HasCoordinates = true;
            Weight = weight;
            Name = "";
            Symb = symb;
            Pos = new Position(pos.X, pos.Y);
            IsVisited = false;
            Neighbours = new List<MapNode>();

            Hash = calcHashFromPos ? CalcHashFromPos() : HashCode.Combine(Pos.GetHashCode(), Weight);
        }

        public MapNode(string name, int weight = 1)
        {
            HasCoordinates = false;
            Weight = weight;
            Name = name;
            Pos = new Position(0, 0);
            IsVisited = false;
            Neighbours = new List<MapNode>();

            Hash = HashCode.Combine(Weight, Name);
            
        }

        public bool Equals(MapNode? other)
        {
            if (other == null)
            {
                return false;
            }

            return this.GetHashCode() == other.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder s = new();

            s.Append(HasCoordinates ? $"{Pos}/" : "");
            s.Append(string.IsNullOrEmpty(Name) ? "" : $"{Name}/");
            s.Append(Weight == int.MinValue ? "" : $"{Weight}/");
            s.Append($"{IsVisited}/{Neighbours.Count}");
            
            return s.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is MapNode))
            {
                return false;
            }
            return Equals(obj as MapNode);
        }

        public override int GetHashCode()
        {
            return Hash;
        }

        private int CalcHashFromPos()
        {
            return Pos.X * 10000 + Pos.Y;
        }
    }
}

