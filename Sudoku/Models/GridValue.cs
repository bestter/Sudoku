﻿/*
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

namespace BestterSudoku.Models
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
