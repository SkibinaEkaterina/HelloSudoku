using System;
using HelloSudoku.Models;
using HelloSudoku.Services;
using HelloSudoku.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using HelloSudoku.Data.Managers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelloSudoku.Controllers
{
    public class SudokuBoardController : Controller
    {

        #region Controller fields/props

        SudokuBoardViewModel _mdl;
        ISudokuGenerator _gen;
        SudokuManager _dbManager;
        int _currentUserId = -1;

        List<SelectListItem> listOfLevels;
        #endregion

        #region Constructor
        public SudokuBoardController(ISudokuGenerator gen, DBContext ctx) 
        {
            this._gen = gen;

            listOfLevels = new List<SelectListItem>()
            { new SelectListItem() { Text = "Низкий", Value = "1"},
              new SelectListItem() { Text = "Средний", Value = "2"},
              new SelectListItem() { Text = "Высокий", Value = "3"}};
            // change User Id
            this._dbManager = new SudokuManager(ctx, _currentUserId);
            }
        #endregion

        /*
         Пользователь нажимает кнопку закончить игру. Выходит и заходит - ему генерируется новая игра.
        Пользователь выходит и заходит - игра должна восстановиться на прежнем месте. (сохранение автоматом или же...?)

         */

        #region Log In page methods
        [HttpGet]
        public IActionResult LogIn()
        {
            // set list of users
            var listOfUsers = (from gm in _dbManager.Games
                              select new SelectListItem() { Text = gm.UserName, Value = gm.UserId.ToString() });

            ViewBag.Users = new SelectList(listOfUsers, "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(int userid)
        {
            if(userid > 0 && _dbManager.Games.Exists(gm => gm.UserId == userid))
            {
                _currentUserId = userid;
            }
            else
            {
                _currentUserId = -1;
            }

            // redirect to Index
            return RedirectToAction("Index", new { id = _currentUserId });
        }
        #endregion

        #region NewGame Page methods

        [HttpGet]
        public IActionResult NewGame()
        {
            // set list of users
            var listOfUsers = (from gm in _dbManager.Games
                               select new SelectListItem() { Text = gm.UserName, Value = gm.UserId.ToString() });

            ViewBag.Users = new SelectList(listOfUsers, "Value", "Text");

            


            ViewBag.Levels = new SelectList(listOfLevels, "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult NewGame(int userid, int level)
        {
            _dbManager.ChangeUser(userid);
            _dbManager.UpdateGameDataInDb("", "", false, level, 0);

            return RedirectToAction("Index", new { id = userid });
        }
        #endregion

        #region Get/Post methods for the main page

        [HttpGet]
        public IActionResult Index(int id = -1)
        {
            _currentUserId = id;
            // load game info from db
            _dbManager.ChangeUser(_currentUserId);

            // check if previous game is complete
            if (_dbManager.currentGame.GameStatus == false)
            {
                StartNewGame();
            }
            else
            {
                LoadExistingGame();
            }

            return View(_mdl);
        }

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
                ModelState.Clear();

                foreach(var el in mdl.sudokuGrid.Where(e => e.value == null))
                {
                    el.value = "";
                }
            }

            return View(mdl);
        }

        #endregion


        // some test
        public IActionResult UserData()
        {
            return View();
        }

        #region Filling methods

        void StartNewGame()
        {
            var curGame = _dbManager.currentGame;

            // generate new
            int[,] grid = new int[9, 9];
            int[,] finalGrid = new int[9, 9];
            _gen.GenerateSudokuGrid(curGame.GameLevel, out finalGrid, out grid);

            _mdl = new SudokuBoardViewModel();
            _mdl.GameLevel = curGame.GameLevel;
            _mdl.GameStatus = true;
            _mdl.UserId = curGame.UserId;
            _mdl.UserName = curGame.UserName;
            _mdl.FillCellsList(grid);
            _mdl.NumberOfMistakes = 0;
            _mdl.GameLevelName = listOfLevels.First(el => el.Value == _mdl.GameLevel.ToString()).Text;

            // update db
            _dbManager.UpdateGameDataInDb(grid, finalGrid, _mdl.GameStatus, curGame.GameLevel, _mdl.NumberOfMistakes);
        }


        void LoadExistingGame()
        {
            var curGame = _dbManager.currentGame;

            _mdl = new SudokuBoardViewModel();
            _mdl.GameLevel = curGame.GameLevel;
            _mdl.GameStatus = curGame.GameStatus == null ? false : (bool)curGame.GameStatus;
            _mdl.UserId = curGame.UserId;
            _mdl.NumberOfMistakes = curGame.NumberOfMistakes;
            _mdl.UserName = curGame.UserName;
            _mdl.GameLevelName = listOfLevels.First(el => el.Value == _mdl.GameLevel.ToString()).Text;

            if (_dbManager.CurrentGrid != null) _mdl.FillCellsList(_dbManager.CurrentGrid);
        }

        #endregion

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

