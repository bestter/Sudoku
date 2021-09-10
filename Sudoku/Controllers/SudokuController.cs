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
using BestterSudoku.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace BestterSudoku.Controllers
{
    public class SudokuController : Controller
    {
        // GET: SudokuController
        public ActionResult Index()
        {
            return View();
        }


        // GET: SudokuController/Create
        public ActionResult Create()
        {
            SudokuGrid grid = new();
            grid.Generate();

            return View(grid);
        }


        public ActionResult Resolve()
        {
            SudokuGrid grid = new();

            grid.SetValue(0, 0, 9, true);
            grid.SetValue(0, 3, 1, true);
            grid.SetValue(0, 8, 5, true);

            grid.SetValue(1, 2, 5, true);
            grid.SetValue(1, 4, 9, true);
            grid.SetValue(1, 6, 2, true);
            grid.SetValue(1, 8, 1, true);

            grid.SetValue(2, 0, 8, true);
            grid.SetValue(2, 4, 4, true);

            grid.SetValue(3, 4, 8, true);

            grid.SetValue(4, 3, 7, true);

            grid.SetValue(5, 4, 2, true);
            grid.SetValue(5, 5, 6, true);
            grid.SetValue(5, 8, 9, true);

            grid.SetValue(6, 0, 2, true);
            grid.SetValue(6, 3, 3, true);
            grid.SetValue(6, 8, 6, true);

            grid.SetValue(7, 3, 2, true);
            grid.SetValue(7, 6, 9, true);

            grid.SetValue(8, 2, 1, true);
            grid.SetValue(8, 3, 9, true);
            grid.SetValue(8, 5, 4, true);
            grid.SetValue(8, 6, 5, true);
            grid.SetValue(8, 7, 7, true);


            /*
            grid.SetValue(0, 1, 4, true);
            grid.SetValue(0, 5, 8, true);
            grid.SetValue(0, 6, 7, true);
            grid.SetValue(0, 8, 3, true);

            grid.SetValue(1, 1, 9, true);
            grid.SetValue(1, 2, 3, true);
            grid.SetValue(1, 3, 4, true);
            grid.SetValue(1, 4, 7, true);
            grid.SetValue(1, 7, 8, true);

            grid.SetValue(2, 0, 8, true);
            grid.SetValue(2, 3, 7, true);
            grid.SetValue(2, 7, 6, true);
            grid.SetValue(2, 8, 9, true);

            grid.SetValue(3, 0, 8, true);
            grid.SetValue(3, 3, 7, true);
            grid.SetValue(3, 7, 6, true);
            grid.SetValue(3, 8, 9, true);

            grid.SetValue(4, 0, 6, true);
            grid.SetValue(4, 2, 2, true);
            grid.SetValue(4, 7, 3, true);
            grid.SetValue(4, 8, 7, true);

            grid.SetValue(5, 1, 7, true);
            grid.SetValue(5, 4, 8, true);
            grid.SetValue(5, 6, 2, true);
            grid.SetValue(5, 8, 5, true);

            grid.SetValue(6, 3, 6, true);
            grid.SetValue(6, 4, 3, true);
            grid.SetValue(6, 5, 7, true);
            grid.SetValue(6, 6, 5, true);
            grid.SetValue(6, 7, 4, true);
            grid.SetValue(6, 8, 1, true);

            grid.SetValue(7, 0, 3, true);
            grid.SetValue(7, 1, 6, true);
            grid.SetValue(7, 3, 5, true);
            grid.SetValue(7, 4, 1, true);
            grid.SetValue(7, 5, 2, true);

            grid.SetValue(8, 0, 4, true);
            grid.SetValue(8, 1, 1, true);
            grid.SetValue(8, 5, 2, true);
            */

            /*
            grid.SetValue(0, 0, 4, true);
            grid.SetValue(0, 6, 1, true);

            grid.SetValue(1, 5, 2, true);
            grid.SetValue(1, 8, 7, true);
            
            grid.SetValue(2, 1, 8, true);
            grid.SetValue(2, 2, 3, true);
            grid.SetValue(2, 4, 9, true);

            grid.SetValue(3, 1, 7, true);
            grid.SetValue(3, 5, 6, true);
            grid.SetValue(3, 6, 4, true);

            grid.SetValue(4, 1, 4, true);
            grid.SetValue(5, 5, 1, true);

            grid.SetValue(5, 7, 8, true);
            grid.SetValue(5, 8, 1, true);

            grid.SetValue(7, 0, 5, true);
            grid.SetValue(7, 4, 3, true);
            grid.SetValue(7, 8, 2, true);

            grid.SetValue(8, 0, 1, true);
            grid.SetValue(8, 4, 8, true);
            grid.SetValue(8, 6, 7, true);
            grid.SetValue(8, 8, 9, true);
            */


            Stopwatch stopwatch = Stopwatch.StartNew();
            long nbTry = 0;
            try
            {
                //nbTry = grid.Resolve();
                nbTry = grid.Resolve();
            }
            catch (NotSupportedException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                stopwatch.Stop();
            }

            return View(new ResolveValue(grid, nbTry, stopwatch.Elapsed));
        }


        // POST: SudokuController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SudokuController/Edit/5

        // POST: SudokuController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // POST: SudokuController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
