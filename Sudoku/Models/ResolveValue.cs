using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Models
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
