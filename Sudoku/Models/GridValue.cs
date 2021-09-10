/*
 This file is part of BestterSudoku.

    BestterSudoku is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Foobar is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BestterSudoku.  If not, see <https://www.gnu.org/licenses/>
 */

using System;

namespace BestterSudoku.Models
{
    public class GridValue: IEquatable<GridValue>, IComparable, IComparable<GridValue>
    {
        public Coordinate Coordinate { get; }
        public byte Value { get; }

        public GridValue(byte x, byte y, byte value)
        {
            Coordinate = new Coordinate(x, y);
            Value = value;
        }

        public GridValue(Coordinate coordinate, byte value)
        {
            Coordinate = coordinate;
            Value = value;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Coordinate, Value);
        }

        public override string ToString()
        {
            return $"{Coordinate} {Value}";
        }

        public bool Equals(GridValue other)
        {
            if (other != null)
            {
                return Coordinate.Equals(other.Coordinate) && Value.Equals(other.Value);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            var other = obj as GridValue;
            if (other != null)
            {
                return Equals(other);
            }
            return false;
        }

        public int CompareTo(object obj)
        {
            var other = obj as GridValue;
            if (other != null)
            {
                return CompareTo(other);
            }
            return -1;
        }

        public int CompareTo(GridValue other)
        {
            if (other != null)
            {
                int compareValue = Coordinate.CompareTo(other.Coordinate);
                if (compareValue == 0)
                {
                    compareValue = Value.CompareTo(other.Value);
                }
                return compareValue;
            }
            return -1;
        }

        public static bool operator ==(GridValue left, GridValue right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(GridValue left, GridValue right)
        {
            return !(left == right);
        }

        public static bool operator <(GridValue left, GridValue right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        public static bool operator <=(GridValue left, GridValue right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(GridValue left, GridValue right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(GridValue left, GridValue right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
