using System;
using HelloSudoku.Models;
using HelloSudoku.Services;
using HelloSudoku.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using HelloSudoku.Data.Managers;

namespace HelloSudoku.Controllers
{
    public class SudokuBoardController : Controller
    {
        SudokuBoardViewModel _mdl;
        ISudokuGenerator _gen;
        SudokuManager _dbManager;

        int _currentUserId;

        public SudokuBoardController(ISudokuGenerator gen, DBContext ctx) 
        {
            this._gen = gen;
            _currentUserId = 1;

            // change User Id
            this._dbManager = new SudokuManager(ctx, _currentUserId);
        }

        /*
         Пользователь нажимает кнопку закончить игру. Выходит и заходит - ему генерируется новая игра.
        Пользователь выходит и заходит - игра должна восстановиться на прежнем месте. (сохранение автоматом или же...?)
         
         */



        [HttpGet]
        public IActionResult Index()
        {
            // load game info from db
            _dbManager.ChangeUser(_currentUserId);

            var curGame = _dbManager.currentGame;

            // прошлая игра закончилась или новый пользователь.
            if (curGame.GameStatus == false)
            {
                // generate new
                int[,] grid = new int[9, 9];
                int[,] finalGrid = new int[9, 9];
                _gen.GenerateSudokuGrid(1, out finalGrid, out grid);

                _mdl = new SudokuBoardViewModel();
                _mdl.GameLevel = curGame.GameLevel;
                _mdl.GameStatus = true;
                _mdl.UserId = curGame.UserId;
                _mdl.FillCellsList(grid);
                _mdl.NumberOfMistakes = 0;

                // update db
                _dbManager.UpdateGameDataInDb(grid, finalGrid, _mdl.GameStatus, curGame.GameLevel, _mdl.NumberOfMistakes);

            }
            else
            {
                _mdl = new SudokuBoardViewModel();
                _mdl.GameLevel = curGame.GameLevel;
                _mdl.GameStatus = curGame.GameStatus == null ? false : (bool)curGame.GameStatus;
                _mdl.UserId = curGame.UserId;
                _mdl.NumberOfMistakes = curGame.NumberOfMistakes;

                if (_dbManager.CurrentGrid != null) _mdl.FillCellsList(_dbManager.CurrentGrid);
                
            }

            return View(_mdl);
        }
        // @onkeyup = "SubmitValidCellValue(event, this.id)",
        // @onblur = "this.form.submit()"

        [HttpPost]
        public IActionResult Index(SudokuBoardViewModel mdl)
        {
            // "sudokuGrid_0__value"
            try
            {

                var curGame = _dbManager.currentGame;


                int idx = GetCellIndx(mdl.changedCellCoordinates);
                int i = mdl.sudokuGrid[idx].XCoordinate, j = mdl.sudokuGrid[idx].YCoordinate;


                if (mdl.sudokuGrid[idx].value != _dbManager.CurrentFinalGrid[i, j])
                {
                    mdl.sudokuGrid[idx].value = "";
                    mdl.NumberOfMistakes += 1;

                    _dbManager.currentGame.NumberOfMistakes += 1;
                    _dbManager.SaveChanges();
                    ModelState.Clear();
                }
                else
                {
                   
                    _dbManager.UpdateGameDataInDb_InGame(mdl.FillGridFromCellsList());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                mdl.changedCellCoordinates = "-";
            }

            return View(mdl);
        }

        public IActionResult UserData()
        {
            return View();
        }

        #region Additional methods
        int GetCellIndx(string str)
        {
            string[] ss = str.Split("_");
            if(ss != null && ss.Length > 0)
            {
                int n;
                if(!int.TryParse(ss[1], out n))
                {
                    return -1;
                }
                return n;
            }
            return -1;
        }
        #endregion

    }
}

