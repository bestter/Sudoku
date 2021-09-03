using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class GridValue
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
    }
}
