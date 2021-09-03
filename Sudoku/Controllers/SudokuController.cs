﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Controllers
{
    public class SudokuController : Controller
    {
        // GET: SudokuController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SudokuController/Details/5
        public ActionResult Details(int id)
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
            grid.SetValue(1, 5, 1, true);

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
            int nbTry = 0;
            try
            {
                nbTry= grid.Resolve();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

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

        // GET: SudokuController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
