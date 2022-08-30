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

                // update db
                _dbManager.UpdateGameDataInDb(grid, finalGrid, _mdl.GameStatus);

            }
            else
            {
                _mdl = new SudokuBoardViewModel();
                _mdl.GameLevel = curGame.GameLevel;
                _mdl.GameStatus = curGame.GameStatus == null ? false : (bool)curGame.GameStatus;
                _mdl.UserId = curGame.UserId;
                if(_dbManager.CurrentGrid != null) _mdl.FillCellsList(_dbManager.CurrentGrid);
                
            }

            return View(_mdl);
        }

        [HttpPost]
        public IActionResult Index(SudokuBoardViewModel mdl)
        {

            _dbManager.ChangeUser(_currentUserId);
            mdl.changedCellCoordinates = -11;
            return View(mdl);
        }


       


    }
}

