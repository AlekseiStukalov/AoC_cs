
using System.Numerics;

namespace Common.Helpers.DataStructures
{
    public class Position : Position<int>
    {
        public Position(Position other) : base(other) { }
        public Position(int x, int y) : base(x, y, 0, is3D: false) { }
        public Position(int x, int y, int z) : base(x, y, z, is3D: true) { }

        /// <param name="posStr">Format:value,value(,value)</param>
        public Position(string posStr) : this(int.MinValue, int.MinValue)
        {
            string[] parts = posStr.Replace(", ",",").ToLower().Split(',');

            X = int.Parse(parts[0]);
            Y = int.Parse(parts[1]);
            if (parts.Length == 3)
            {
                Z = int.Parse(parts[2]);
                Is3D = true;
            }
        }
    }

    public class Position<T> where T : INumber<T>, IEquatable<T> 
    {
        public T X;
        public T Y;
        public T Z;

        protected bool Is3D;

        public Position(Position<T> other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
            Is3D = other.Is3D;
        }

        public Position(T x, T y, T z, bool is3D)
        {
            X = x;
            Y = y;
            Z = z;
            Is3D = is3D;
        }

        public bool IsZero()
        {
            return X == T.Zero && Y == T.Zero && Z == T.Zero;
        }

        public override string ToString()
        {
            string str = $"{X};{Y}";

            if (Is3D)
            {
                str += $";{Z}";
            }

            return str;
        }

        public bool Equals(Position<T>? other)
        {
            if (other == null)
            {
                return false;
            }

            return this.GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Position<T>))
            {
                return false;
            }
            return Equals(obj as Position<T>);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}
