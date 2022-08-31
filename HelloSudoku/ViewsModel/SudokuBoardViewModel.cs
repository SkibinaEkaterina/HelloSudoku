﻿using System;
using HelloSudoku.Models;

namespace HelloSudoku.ViewsModel
{
    public class SudokuBoardViewModel
    {
        
        public List<Cell> sudokuGrid { get; set; }
        public bool GameStatus { get; set; }
        public int GameLevel { get; set; }
        public string GameLevelName { get; set; }
        public string changedCellCoordinates { get; set; } = "-";
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int NumberOfMistakes { get; set; }

        public SudokuBoardViewModel()
        {
            sudokuGrid = new List<Cell>();
        }

        #region Fill Cells list methods

        public void FillCellsList(string[,] sgrid)
        {
            int nrows = sgrid.GetLength(0);
            int ncols = sgrid.GetLength(1);
            for (int i = 0; i < nrows; i++)
            {
                for (int j = 0; j < ncols; j++)
                {
                    sudokuGrid.Add(new Cell() { value = sgrid[i, j] == "0" ? "" : sgrid[i, j], XCoordinate = i, YCoordinate = j });
                }
            }
        }

        public void FillCellsList(int[,] sgrid)
        {
            int nrows = sgrid.GetLength(0);
            int ncols = sgrid.GetLength(1);
            for (int i = 0; i < nrows; i++)
            {
                for (int j = 0; j < ncols; j++)
                {
                    sudokuGrid.Add(new Cell() { value = (sgrid[i, j]==0 ? "" : sgrid[i, j].ToString()),
                        XCoordinate = i, YCoordinate = j });
                }
            }
        }

        public string[,] FillGridFromCellsList()
        {
            string[,] grid = new string[9, 9];

            foreach(var el in sudokuGrid)
            {
                grid[el.XCoordinate, el.YCoordinate] = el.value;
            }

            return grid;
        }

        #endregion Fill Cells list methods
    }
}

