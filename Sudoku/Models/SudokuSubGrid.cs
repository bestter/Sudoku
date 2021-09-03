using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class SubGridValue
    {
        public bool IsDefinition { get; }
        public byte Value { get; }

        public static implicit operator byte(SubGridValue v) => v.Value;
        public static implicit operator SubGridValue(byte b) => new(b, false);

        public SubGridValue(byte value, bool isDefinition = false)
        {
            IsDefinition = isDefinition;
            Value = value;
        }

        public SubGridValue()
        {
            IsDefinition = false;
            Value = 0;
        }
    }

    /// <summary>
    /// Sudoku sub grid
    /// </summary>
    public class SudokuSubGrid : IEquatable<SudokuSubGrid>
    {
        internal static ReadOnlyCollection<byte> Digits = new(new List<byte> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        readonly SubGridValue[,] subGrid;

        Coordinate Position { get; }

        public SudokuSubGrid(byte x, byte y)
        {
            subGrid = new SubGridValue[3, 3];
            Position = new Coordinate(x, y);
        }

        public override string ToString()
        {
            return Position.ToString();
        }


        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SudokuSubGrid other)
            {
                return Equals(other);
            }
            return false;

        }

        public bool Equals(SudokuSubGrid other)
        {
            return Position.Equals(other.Position);
        }

        /// <summary>
        /// Did the grid has empty value
        /// </summary>
        public bool HasEmptyValue
        {
            get
            {
                bool hasEmptyValue = false;
                for (int i = 0; i <= 2; i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        if (GetValue(i, j) == 0)
                        {
                            hasEmptyValue = true;
                            break;
                        }
                    }
                }
                return hasEmptyValue;
            }
        }

        /// <summary>
        /// Set value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public void SetValue(int x, int y, byte value, bool isDefinition = false)
        {
            if (x < 0 || x > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(x), x, $"{nameof(x)} must be between 0 and 2");
            }
            if (y < 0 || y > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(y), y, $"{nameof(y)} must be between 0 and 2");
            }
            if (value < 1 || value > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be between 1 and 9");
            }

            subGrid[x, y] = new SubGridValue(value, isDefinition);
        }

        public SubGridValue GetValue(int x, int y)
        {
            return subGrid[x, y];
        }

        public void Fill(IEnumerable<byte> values)
        {
            byte[] data = values.ToArray();

            for (int i = 0; i < subGrid.GetLength(0); i++)
            {
                for (int j = 0; j < subGrid.GetLength(1); j++)
                {
                    subGrid[i, j] = data[(j * 3) + i];
                }
            }
        }


    }
}
