using System;
using HelloSudoku.Data.Interfaces;
using HelloSudoku.Models;

namespace HelloSudoku.Data.Managers
{
    public class SudokuManager
    {

        #region SudokuManager fields/props
        public SudokuGame currentGame { get; set; }
        DBContext ctx;

        public List<SudokuGame> Games => ctx.GameInfo.ToList();


        public string[,]? CurrentGrid
        {
            get
            {
                if (currentGame == null)
                    return null;
                return GetGridFromString(currentGame.Grid);
            }
            set
            { }
        }

        public string[,]? CurrentFinalGrid
        {
            get
            {
                if (currentGame == null)
                    return null;
                return GetGridFromString(currentGame.FinalGrid);
            }
        }

        #endregion

        #region Constructor
        public SudokuManager(DBContext ctx, int UserId)
        {
            this.ctx = ctx;
            ChangeUser(UserId);
        }
        #endregion

        #region Change user
        public void ChangeUser(int UserId)
        {
            if(UserId != -1)
            {
                currentGame = ctx.GameInfo.Where(s => s.UserId == UserId).First();
            }
            else
            {
                currentGame = ctx.GameInfo.First();
            }
            
        }
        #endregion

        #region Update Game Data methods

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }

        // main 
        public void UpdateGameDataInDb(string grid, string finGrid, bool gameStatus, int GameLevel, int NumberOfMistakes)
        {
            currentGame.FinalGrid = finGrid;
            currentGame.Grid = grid;
            currentGame.GameStatus = gameStatus;
            currentGame.GameLevel = GameLevel;
            currentGame.NumberOfMistakes = NumberOfMistakes;
            ctx.SaveChanges();
        }

        public void UpdateGameDataInDb(int[,] grid, int[,] finGrid, bool gameStatus, int GameLevel, int NumberOfMistakes)
        {
            UpdateGameDataInDb(grid == null ? "" : GetStringFromGrid(grid),
                               finGrid == null ? "" : GetStringFromGrid(finGrid),
                               gameStatus, GameLevel, NumberOfMistakes);

        }

        public void UpdateGameDataInDb(string[,] grid, string[,] finGrid, bool gameStatus, int GameLevel, int NumberOfMistakes)
        {
            UpdateGameDataInDb(grid == null ? "" : GetStringFromGrid(grid),
                               finGrid == null ? "" : GetStringFromGrid(finGrid),
                               gameStatus, GameLevel, NumberOfMistakes);

        }

        // only grid and nmb of mistakes
        public void UpdateGameDataInDb_InGame(string grid)
        {
            currentGame.Grid = grid;

            ctx.SaveChanges();
        }
        public void UpdateGameDataInDb_InGame(string[,] grid)
        {
            currentGame.Grid = grid == null ? "" : GetStringFromGrid(grid);

            ctx.SaveChanges();
        }

        #endregion Update Game Data

        #region Sudoku Grid Transformation methods

        public static string GetStringFromGrid(int[,] gr)
        {
            string str = "";
            int nRows = gr.GetLength(0);
            int nCols = gr.GetLength(1);

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    str += gr[i, j].ToString() + (j == nCols - 1 ? "" : ",");
                }
                str += ";";
            }

            return str;
        }

        public static string[,] GetGridFromString(string str)
        {
            int i = 0, j = 0;
            string[,] NewArray = new string[9, 9];

            foreach (var row in str.Split(";"))
            {
                if (row == "")
                    continue;

                foreach (var el in row.Split(","))
                {
                    NewArray[i, j] = el;
                    j++;
                }
                i++;
                j = 0;
            }

            return NewArray;
        }

        public static string GetStringFromGrid(string[,] gr)
        {
            string str = "";
            int nRows = gr.GetLength(0);
            int nCols = gr.GetLength(1);

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    str += gr[i, j] + (j == nCols - 1 ? "" : ",");
                }
                str += ";";
            }

            return str;
        }

        #endregion Grids Transformations
    }
}

