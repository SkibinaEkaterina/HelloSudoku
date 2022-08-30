using System;
namespace HelloSudoku.Services
{
    public interface ISudokuGenerator
    {
        

        /// <summary>
        /// Generates 2 grids for Sudoku game.
        /// </summary>
        /// <param name="BaseGrid">Grid with all cells filled.</param>
        /// <param name="PlayerGrid">Grid to display.</param>
        void GenerateSudokuGrid(int Level, out int[,] BaseGrid, out int[,] PlayerGrid);
    }
}

