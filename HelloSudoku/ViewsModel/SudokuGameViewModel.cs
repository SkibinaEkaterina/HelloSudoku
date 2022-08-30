using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloSudoku.ViewsModel
{
    public class SudokuGameViewModel 
    {
        public int userID { get; set; }
        public bool GameInProcess { get; set; }
        public string[,] Grid { get; set; }

        [BindProperty]
        public string[,] FinalGrid { get; set; }

        public int GameLevel { get; set; } = 1;

        public SudokuGameViewModel()
        {
            Grid = new string[9, 9];
            FinalGrid = new string[9, 9];
        }
        public SudokuGameViewModel(int[,] grid, int[,] finGrid)
        {
            Grid = GetStringGrid(grid);
            FinalGrid = GetStringGrid(finGrid);
        }

        public SudokuGameViewModel(string[,] grid, string[,] finGrid)
        {
            Grid = grid;
            FinalGrid = finGrid;
        }

        string[,] GetStringGrid(int[,] iGrid)
        {
            int nRows = iGrid.GetLength(0);
            int nCols = iGrid.GetLength(1);
            string[,] sGrid = new string[nRows, nCols];

            for(int i=0; i<nRows;i++)
            {
                for(int j=0;j<nCols; j++)
                {
                    sGrid[i, j] = iGrid[i, j]==0 ? "" : iGrid[i, j].ToString();
                }
            }

            return sGrid;
        }

       

       

    }
}

