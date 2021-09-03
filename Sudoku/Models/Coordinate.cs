using System;

namespace Sudoku.Models
{
    public class Coordinate: IEquatable<Coordinate>, IComparable, IComparable<Coordinate>
    {
        public byte X { get; }
        public byte Y { get; }

        public Coordinate (byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public override int GetHashCode()
        {
            return (new { A = X, B = Y }).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(Coordinate other)
        {
            if (other != null)
            {
                return X.Equals(other.X) && Y.Equals(other.Y);
            }
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj is Coordinate other)
            {
                return CompareTo(other);
            }
            return 999;
        }

        public int CompareTo(Coordinate other)
        {
            if (other != null)
            {
                int returnValue = X.CompareTo(other.X);
                if (returnValue == 0)
                {
                    returnValue = Y.CompareTo(other.Y);
                }
                return returnValue;
            }
            return 999;
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        public static bool operator <(Coordinate left, Coordinate right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Coordinate left, Coordinate right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Coordinate left, Coordinate right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Coordinate left, Coordinate right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}
