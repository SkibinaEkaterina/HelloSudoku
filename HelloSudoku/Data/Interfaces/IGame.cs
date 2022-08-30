using System;
using HelloSudoku.Models;

namespace HelloSudoku.Data.Interfaces
{
    public interface IGame
    {
        SudokuGame currentGame { get; set; }

        void UpdateGameDataInDb(string grid, string finGrid, bool gameStatus);
    }
}

