using System;
namespace HelloSudoku.Services
{
    public class SimpleSudokuGenerator : ISudokuGenerator
    {
        int _gameLevel = 1;
        int _sudokuComplexity
        {
            get
            {
                switch (_gameLevel)
                {
                    case 1:
                    default:
                        return 32;
                    case 2:
                        return 28;
                    case 3:
                        return 24;
                }
            }
        }



        public SimpleSudokuGenerator()
        {
        }

        public void GenerateSudokuGrid(int Level, out int[,] BaseGrid, out int[,] PlayerGrid)
        {
            BaseGrid = new int[9, 9]  { { 9, 8, 1, 5, 6, 7, 2, 3, 4 },
                                        { 6, 5, 7, 2, 3, 4, 8, 9, 1 },
                                        { 3, 2, 4, 8, 9, 1, 5, 6, 7 },
                                        { 5, 4, 6, 1, 2, 3, 7, 8, 9 },
                                        { 8, 7, 9, 4, 5, 6, 1, 2, 3 },
                                        { 2, 1, 3, 7, 8, 9, 4, 5, 6 },
                                        { 7, 6, 8, 3, 4, 5, 9, 1, 2 },
                                        { 1, 9, 2, 6, 7, 8, 3, 4, 5 },
                                        { 4, 3, 5, 9, 1, 2, 6, 7, 8 }};

            PlayerGrid = new int[9, 9] {{ 0, 8, 1, 5, 6, 7, 0, 0, 0 },
                                        { 0, 0, 7, 0, 0, 0, 0, 0, 0 },
                                        { 0, 2, 0, 8, 9, 0, 0, 6, 0 },
                                        { 5, 0, 6, 1, 0, 0, 7, 8, 0 },
                                        { 0, 0, 0, 0, 5, 6, 0, 0, 0 },
                                        { 2, 0, 3, 7, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 4, 0, 0, 0, 2 },
                                        { 0, 9, 0, 0, 7, 8, 0, 4, 5 },
                                        { 4, 0, 0, 9, 0, 0, 6, 7, 8 }};
        }
    }
}
