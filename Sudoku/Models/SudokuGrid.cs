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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BestterSudoku.Models
{
    public class SudokuGrid
    {
        internal static ReadOnlyCollection<byte> Digits = new(new List<byte> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        /// <summary>
        /// Creation time
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }


        private long numberOfCall;

        private readonly GridValue[,] grids;

        public SudokuGrid()
        {
            grids = new GridValue[9, 9];
            for (byte i = 0; i <= 8; i++)
            {
                for (byte j = 0; j <= 8; j++)
                {
                    grids[i, j] = new GridValue(i, j);
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

            GridValue sgvalue = grids[line, column];
            sgvalue.SetValue(value, isDefinition);
        }

        public GridValue[,] GetValues()
        {
            return grids;
        }

        public void Generate()
        {
            RNGCryptoServiceProvider random = new();

            byte[] items = Digits.OrderBy(x => random.GetNextInt32()).ToArray();

            //Middle
            for (int i = 0; i < items.Length; i++)
            {
                int line = i / 3, column = i % 3;
                grids[line + 3, column + 3].SetValue(items[i]);
            }


            //Now do the main algorithm
            for (int line = 0; line <= 8; line++)
            {
                for (int column = 0; column <= 8; column++)
                {
                    IEnumerable<byte> numbersOnLine = GetNumbersOnLine(line);
                    List<byte> availableNumberOnLine = Digits.Except(numbersOnLine).ToList();
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
                        var numbers = Digits.OrderBy(x => x).ToArray();
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

            for (int j = 0; j <= 8; j++)
            {
                byte value = grids[line, j];
                if (value != 0)
                {
                    numbersOnLine.Add(value);
                }
            }
            return numbersOnLine;
        }

        private List<byte> GetNumbersOnColumn(int column)
        {
            List<byte> numbersOnColumn = new();

            for (int i = 0; i <= 8; i++)
            {
                byte value = grids[i, column];
                if (value != 0)
                {
                    numbersOnColumn.Add(value);
                }
            }
            return numbersOnColumn;
        }

        private List<byte> GetNumbersInGrid(int line, int column)
        {
            List<byte> numberInGrid = new();

            for (int i = line; i <= 2 + line; i++)
            {
                for (int j = column; j <= 2 + column; j++)
                {
                    byte value = grids[i, j];
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

            var isNumberInLine = numbersOnLine.Any(n => n == number);
            if (isNumberInLine)
            {
                return false;
            }

            IEnumerable<byte> numbersOnColumn = GetNumbersOnColumn(currentColumn);

            var isNumberInColumn = numbersOnColumn.Any(n => n == number);
            if (isNumberInColumn)
            {
                return false;
            }

            var numbersInGrid = GetNumbersInGrid(currentLine, currentColumn);
            var isNumberInGrid = numbersInGrid.Any(n => n == number);
            if (isNumberInGrid)
            {
                return false;
            }

            return true;
        }


        public long Resolve()
        {
            IsValid(0);
            return numberOfCall;
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


        private bool MissingInLine(byte k, int line)
        {

            for (int j = 0; j < 9; j++)
                if (grids[line, j].Value == k)
                    return false;
            return true;
        }

        private bool MissingInColumn(byte k, int column)
        {

            for (int i = 0; i < 9; i++)
                if (grids[i, column].Value == k)
                    return false;
            return true;
        }

        private bool MissingInBlock(byte k, int i, int j)
        {
            int _i = i - (i % 3), _j = j - (j % 3);  // ou encore : _i = 3*(i/3), _j = 3*(j/3);
            for (i = _i; i < _i + 3; i++)
                for (j = _j; j < _j + 3; j++)
                    if (grids[i, j].Value == k)
                        return false;
            return true;
        }

        private bool IsDefinition(int line, int column)
        {
            GridValue value= grids[line, column];
            return value != null && value.IsDefinition;
        }

        private bool IsValid(int position)
        {            
            if (numberOfCall > Math.Pow(81, 9))
            {
                throw new NotSupportedException($"Too many call {numberOfCall} in {nameof(IsValid)}. Current position {position}");
            }

            numberOfCall++;

            if (position == 9 * 9)
            {
                return true;
            }

            int line = position / 9, column = position % 9;

            if (grids[line, column].Value != 0)
            {
                return IsValid(position + 1);
            }

            //backtracking
            // énumération des valeurs possibles
            for (byte k = 1; k <= 9; k++)
            {
                // Si la valeur est absente, donc autorisée
                if (MissingInLine(k, line) && MissingInColumn(k, column) && MissingInBlock(k, line, column) && !IsDefinition(line, column))
                {
                    // On enregistre k dans la grille
                    grids[line, column].SetValue(k);
                    // On appelle récursivement la fonction estValide(), pour voir si ce choix est bon par la suite
                    if (IsValid(position + 1))
                        return true;  // Si le choix est bon, plus la peine de continuer, on renvoie true :)
                }
            }
            // Tous les chiffres ont été testés, aucun n'est bon, on réinitialise la case
            if (!IsDefinition(line, column))
            {
                grids[line, column].SetValue(0);
            }
            // Puis on retourne false :(
            return false;
        }

    }
}
