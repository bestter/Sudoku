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
using BestterSudoku.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BestterSudoku.Models
{
    public class SudokuGrid
    {
        /// <summary>
        /// Creation time
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        readonly SudokuSubGrid[,] subGrids;

        public SudokuGrid()
        {
            subGrids = new SudokuSubGrid[3, 3];
            for (byte i = 0; i <= 2; i++)
            {
                for (byte j = 0; j <= 2; j++)
                {
                    subGrids[i, j] = new SudokuSubGrid(i, j);
                }
            }

        }

        public void SetValue(int line, int column, byte value, bool isDefinition = false)
        {
            if (line < 0 || line > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(line), line, $"{nameof(line)} must be between 0 and 8");
            }
            if (column < 0 || column > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(column), column, $"{nameof(column)} must be between 0 and 8");
            }
            if (value < 1 || value > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be between 1 and 9");
            }

            int gridLine = line / 3;
            int gridColumn = column / 3;
            var subGrid = subGrids[gridLine, gridColumn];

            var lineOffSet = line - (3 * gridLine);
            var columnOffSet = column - (3 * gridColumn);
            subGrid.SetValue(lineOffSet, columnOffSet, value, isDefinition);
        }

        public SudokuSubGrid[,] GetValues()
        {
            return subGrids;
        }

        public void Generate()
        {
            RNGCryptoServiceProvider random = new();

            byte[] items = SudokuSubGrid.Digits.OrderBy(x => random.GetNextInt32()).ToArray();
            subGrids[1, 1].Fill(items);

            //Now do the main algorithm
            for (int line = 0; line <= 8; line++)
            {
                for (int column = 0; column <= 8; column++)
                {
                    IEnumerable<byte> numbersOnLine = GetNumbersOnLine(line);
                    List<byte> availableNumberOnLine = SudokuSubGrid.Digits.Except(numbersOnLine).ToList();
                    List<byte> numbersOnColumn = GetNumbersOnColumn(column);
                    List<byte> availableNumberOnColumn = availableNumberOnLine.Except(numbersOnColumn).ToList();

                    List<byte> numbersOnGrid = GetNumbersInGrid(line, column);

                    var availableNumber = availableNumberOnColumn.Except(numbersOnGrid).OrderBy(x => random.GetNextInt32()).ToList();

                    bool isFound = false;
                    foreach (byte number in availableNumber)
                    {
                        if (IsNumberAvailable(number, line, column))
                        {
                            SetValue(line, column, number, true);
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        var numbers = SudokuSubGrid.Digits.OrderBy(x => x).ToArray();
                        foreach (byte number in numbers)
                        {
                            if (IsNumberAvailable(number, line, column))
                            {
                                SetValue(line, column, number, true);
                                isFound = true;
                                break;
                            }
                        }
                    }
                    Debug.Assert(isFound, $"Impossible to set data in line {line} column {column}");
                }
            }
            CreationTime = DateTimeOffset.Now;
        }

        private List<byte> GetNumbersOnLine(int line)
        {
            List<byte> numbersOnLine = new();
            int gridLine = line / 3;

            for (int grid = 0; grid <= 2; grid++)
            {
                var subGrid = subGrids[gridLine, grid];
                for (int j = 0; j <= 2; j++)
                {
                    var offSet = line - (3 * gridLine);
                    byte value = subGrid.GetValue(offSet, j);
                    if (value != 0)
                    {
                        numbersOnLine.Add(value);
                    }
                }
            }
            return numbersOnLine;
        }

        private List<byte> GetNumbersOnColumn(int column)
        {
            List<byte> numbersOnColumn = new();
            int gridColumn = column / 3;

            for (int grid = 0; grid <= 2; grid++)
            {
                var subGrid = subGrids[grid, gridColumn];
                for (int j = 0; j <= 2; j++)
                {
                    var offSet = column - (3 * gridColumn);
                    byte value = subGrid.GetValue(j, offSet);
                    if (value != 0)
                    {
                        numbersOnColumn.Add(value);
                    }
                }
            }
            return numbersOnColumn;
        }

        private List<byte> GetNumbersInGrid(int line, int column)
        {
            List<byte> numberInGrid = new();
            var currentGrid = subGrids[line / 3, column / 3];
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    byte value = currentGrid.GetValue(i, j);
                    if (value != 0)
                    {
                        numberInGrid.Add(value);
                    }
                }
            }
            return numberInGrid;
        }

        private bool IsNumberAvailable(byte number, int currentLine, int currentColumn)
        {
            IEnumerable<byte> numbersOnLine = GetNumbersOnLine(currentLine);
            System.Diagnostics.Debug.WriteLine(nameof(numbersOnLine) + " " + EnumerateArray(numbersOnLine));

            var isNumberInLine = numbersOnLine.Any(n => n == number);
            if (isNumberInLine)
            {
                return false;
            }

            IEnumerable<byte> numbersOnColumn = GetNumbersOnColumn(currentColumn);
            System.Diagnostics.Debug.WriteLine(nameof(numbersOnColumn) + " " + EnumerateArray(numbersOnColumn));

            var isNumberInColumn = numbersOnColumn.Any(n => n == number);
            if (isNumberInColumn)
            {
                return false;
            }

            var numbersInGrid = GetNumbersInGrid(currentLine, currentColumn);
            System.Diagnostics.Debug.WriteLine(nameof(numbersInGrid) + " " + " " + EnumerateArray(numbersInGrid));

            var isNumberInGrid = numbersInGrid.Any(n => n == number);
            if (isNumberInGrid)
            {
                return false;
            }

            System.Diagnostics.Debug.WriteLine(number.ToString() + Environment.NewLine);
            return true;
        }

        public int Resolve()
        {
            int nbTry = 0;
            while (HasEmptyValue && nbTry < 10)
            {
                for (int line = 0; line <= 8; line++)
                {
                    for (int column = 0; column <= 8; column++)
                    {
                        List<byte> numbersOnLine = GetNumbersOnLine(line);
                        List<byte> availableNumberOnLine = SudokuSubGrid.Digits.Except(numbersOnLine).ToList();
                        List<byte> numbersOnColumn = GetNumbersOnColumn(column);
                        List<byte> availableNumberOnColumn = availableNumberOnLine.Except(numbersOnColumn).ToList();

                        List<byte> numbersOnGrid = GetNumbersInGrid(line, column);

                        var availableNumbers = availableNumberOnColumn.Except(numbersOnGrid).OrderBy(x => x).ToList();

                        if (availableNumbers.Count == 1)
                        {
                            var number = availableNumbers[0];
                            //Safety
                            if (IsNumberAvailable(number, line, column))
                            {
                                SetValue(line, column, number);
                                break;
                            }
                        }
                        else if (availableNumbers.Count > 1)
                        {
                            foreach (var number in availableNumbers)
                            {
                                bool okToAdd = false;
                                List<byte> numbersOnPreviousLine = new();
                                List<byte> numbersOnNextLine = new();
                                List<byte> numbersOnPreviousColumn = new();
                                List<byte> numbersOnNextColumn = new();

                                if (line == 0)
                                {
                                    numbersOnPreviousLine.AddRange(SudokuSubGrid.Digits);
                                }
                                if (line > 0)
                                {
                                    numbersOnPreviousLine.AddRange(GetNumbersOnLine(line - 1));
                                }


                                if (line < 8)
                                {
                                    numbersOnNextLine.AddRange(GetNumbersOnLine(line + 1));
                                }
                                if (line == 8)
                                {
                                    numbersOnNextLine.AddRange(SudokuSubGrid.Digits);
                                }

                                if (column == 0)
                                {
                                    numbersOnPreviousColumn.AddRange(SudokuSubGrid.Digits);
                                }
                                if (column > 0)
                                {
                                    numbersOnPreviousColumn.AddRange(GetNumbersOnColumn(column - 1));
                                }

                                if (column < 8)
                                {
                                    numbersOnNextColumn.AddRange(GetNumbersOnColumn(column + 1));
                                }

                                if (column == 8)
                                {
                                    numbersOnNextColumn.AddRange(SudokuSubGrid.Digits);
                                }

                                if (numbersOnPreviousLine.Any(n => n == number) &&
                                    numbersOnNextLine.Any(n => n == number) &&
                                    numbersOnPreviousColumn.Any(n => n == number) &&
                                    numbersOnNextColumn.Any(n => n == number)
                                    )
                                {
                                    okToAdd = true;
                                }

                                //if (!okToAdd)
                                //{

                                //}


                                if (okToAdd)
                                {
                                    if (IsNumberAvailable(number, line, column))
                                    {
                                        SetValue(line, column, number);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
                nbTry++;
            }


            return nbTry;
        }

        /// <summary>
        /// Did the any subgrid has empty value
        /// </summary>
        private static string EnumerateArray(IEnumerable<byte> array)
        {
            StringBuilder strb = new();
            foreach (byte value in array)
            {
                strb.AppendFormat("{0}\t", value);
            }
            return strb.ToString().Trim();
        }

        public bool HasEmptyValue
        {
            get
            {
                bool hasEmptyValue = false;
                for (int i = 0; i <= 2; i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        if (subGrids[i, j].HasEmptyValue)
                        {
                            hasEmptyValue = true;
                            break;
                        }
                    }
                }
                return hasEmptyValue;
            }
        }
    }
}
