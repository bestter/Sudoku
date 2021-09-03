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
    public class ResolveValue
    {
        public SudokuGrid Grid { get; }
        public int NbTry { get; }

        public TimeSpan Elapsed { get; }

        public ResolveValue(SudokuGrid grid, int nbTry, TimeSpan elapsed)
        {
            Grid = grid;
            NbTry = nbTry;
            Elapsed = elapsed;
        }

    }
}
