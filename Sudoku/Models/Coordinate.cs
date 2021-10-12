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

using Newtonsoft.Json;
using System;

namespace BestterSudoku.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Coordinate : IEquatable<Coordinate>, IComparable, IComparable<Coordinate>
    {
        /// <summary>
        /// X
        /// </summary>
        [JsonProperty]
        public byte X { get; }
        
        /// <summary>
        /// Y
        /// </summary>
        [JsonProperty]
        public byte Y { get; }

        public Coordinate(byte x, byte y)
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
            return System.HashCode.Combine(X, Y);
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
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        public static bool operator <(Coordinate left, Coordinate right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Coordinate left, Coordinate right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Coordinate left, Coordinate right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Coordinate left, Coordinate right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
