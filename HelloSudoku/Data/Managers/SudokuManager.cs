using System;
using HelloSudoku.Data.Interfaces;
using HelloSudoku.Models;

namespace HelloSudoku.Data.Managers
{
    public class SudokuManager : IGame
    {
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

        

        public SudokuManager(DBContext ctx, int UserId)
        {
            this.ctx = ctx;
            this.currentGame = ctx.GameInfo.Where(s => s.UserId == UserId).First();
        }


        public void ChangeUser(int UserId)
        {
            this.currentGame = ctx.GameInfo.Where(s => s.UserId == UserId).First();
        }

        #region Update Game Data
        // main 
        public void UpdateGameDataInDb(string grid, string finGrid, bool gameStatus)
        {
            currentGame.FinalGrid = finGrid;
            currentGame.Grid = grid;
            currentGame.GameStatus = gameStatus;
            ctx.SaveChanges();
        }

        public void UpdateGameDataInDb(int[,] grid, int[,] finGrid, bool gameStatus)
        {
            UpdateGameDataInDb(grid == null ? "" : GetStringFromGrid(grid),
                               finGrid == null ? "" : GetStringFromGrid(finGrid),
                               gameStatus);

        }

        public void UpdateGameDataInDb(string[,] grid, string[,] finGrid, bool gameStatus)
        {
            UpdateGameDataInDb(grid == null ? "" : GetStringFromGrid(grid),
                               finGrid == null ? "" : GetStringFromGrid(finGrid),
                               gameStatus);

        }

        #endregion Update Game Data

        #region Grids Transformations
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

