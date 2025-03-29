namespace Common.Helpers.DataStructures
{
	public class PosVector : Position<double>
	{
        public static int CreationsCount = 0;
        public PosVector(PosVector other) : base(other) { CreationsCount++; }

        public PosVector(double x, double y) : base(x, y, 0, is3D: false) { CreationsCount++; }

        public PosVector(double x, double y, double z) : base(x, y, z, is3D: true) { CreationsCount++; }

        public static PosVector operator +(PosVector left, PosVector right)
        {
            if (left.Is3D && right.Is3D)
            {
                return new PosVector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
            }
            else
            {
                return new PosVector(left.X + right.X, left.Y + right.Y);
            }
        }

        public void Add(PosVector other)
        {
            X += other.X;
            Y += other.Y;
            if (Is3D && other.Is3D)
            {
                Z += other.Z;
            }
            else
            {
                Console.WriteLine("Warning. Adding 2d and 3d");
            }
        }

        public static PosVector operator -(PosVector left, PosVector right)
        {
            if (left.Is3D && right.Is3D)
            {
                return new PosVector(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
            }
            else
            {
                return new PosVector(left.X - right.X, left.Y - right.Y);
            }
        }

        public virtual PosVector GetAbs()
        {
            if (Is3D)
            {
                return new PosVector(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
            }
            else
            {
                return new PosVector(Math.Abs(X), Math.Abs(Y));
            }
        }

        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }

        public PosVector GetNormalized()
        {
           double length = GetLength();
            if (Is3D)
            {
                return new PosVector(X / length, Y / length, Z / length);
            }
            else
            {
                return new PosVector(X / length, Y / length);
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is PosVector))
            {
                return false;
            }
            return Equals(obj as PosVector);
        }

        public bool Equals(PosVector? other) => base.Equals(other);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
    }
}

