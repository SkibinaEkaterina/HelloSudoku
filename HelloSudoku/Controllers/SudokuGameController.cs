using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HelloSudoku.Models;
using Microsoft.AspNetCore.Mvc;
using HelloSudoku.Services;
using HelloSudoku.ViewsModel;
using HelloSudoku;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloSudoku.Controllers
{
    public class SudokuGameController : Controller
    {
        

        ISudokuGenerator _gen;
        SudokuGameViewModel vm;
        DBContext ctx;
        SudokuGame currentGame;

        public SudokuGameController(ISudokuGenerator gen, DBContext ctx)
        {
            this._gen = gen;
            vm = new SudokuGameViewModel();
            this.ctx = ctx;
        }

        /*[HttpGet]
        public IActionResult Index()
        {
            GetCurrentGame(1);

            if (currentGame.GameStatus == false)
            {
                int[,] grid = new int[9, 9];
                int[,] finalGrid = new int[9, 9];
                _gen.GenerateSudokuGrid(vm.GameLevel, out finalGrid, out grid);

                vm = new SudokuGameViewModel(grid, finalGrid);
                vm.GameInProcess = true;

                // update db
                currentGame.Grid = SudokuGameViewModel.GetStringFromGrid(vm.Grid);
                currentGame.FinalGrid = SudokuGameViewModel.GetStringFromGrid(vm.FinalGrid);
                currentGame.GameStatus = true;
                ctx.SaveChanges();

            }
            else {
                vm = new SudokuGameViewModel(SudokuGameViewModel.GetGridFromString(currentGame.Grid),
                            SudokuGameViewModel.GetGridFromString(currentGame.FinalGrid));
                vm.GameInProcess = (bool)currentGame.GameStatus;
            }

            return View(vm);
        }

        private void GetCurrentGame(int id)
        {
            // Get
            currentGame = ctx.GameInfo.Where(s => s.UserId == id).First();
        }

        [HttpPost]
        public IActionResult Index(string[] mtrx)
        {
            


            return View(vm);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}

