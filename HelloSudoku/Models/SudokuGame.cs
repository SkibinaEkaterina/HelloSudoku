using System;
using System.ComponentModel.DataAnnotations;


namespace HelloSudoku.Models
{
    public class SudokuGame
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = "User";
        public string? Grid { get; set; }
        public string? FinalGrid { get; set; }

        public bool? GameStatus { get; set; }
        public int GameLevel { get; set; }
        public int NumberOfMistakes { get; set; } = 0;

    }
}

