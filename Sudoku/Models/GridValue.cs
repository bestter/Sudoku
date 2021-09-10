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
    /// <summary>
    /// Grid value
    /// </summary>
    public class GridValue: IEquatable<GridValue>
    {
        public Coordinate Coordinate { get; }

        /// <summary>
        /// Is a definition value
        /// </summary>
        public bool IsDefinition { get; private set; }

        /// <summary>
        /// The actual value
        /// </summary>
        public byte Value { get; private set; }

        public static implicit operator byte(GridValue v) => v.Value;
                
        public GridValue(byte line, byte column)
        {
            Coordinate = new Coordinate(line, column);
            IsDefinition = false;
            Value = 0;
        }

        public void SetValue(byte value, bool isDefinition = false)
        {
            if (IsDefinition)
            {
                throw new NotSupportedException("Cannot change value of a definition field");
            }

            Value = value;
            IsDefinition = isDefinition;
        }

        public override string ToString()
        {
            return $"{Coordinate} {IsDefinition} {Value}";
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Coordinate, IsDefinition, Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is GridValue other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(GridValue other)
        {
            if (other != null)
            {
                return Coordinate.Equals(other.Coordinate) && IsDefinition.Equals(other.IsDefinition) && Value.Equals(other.Value);
            }
            return false;
        }
    }
}
