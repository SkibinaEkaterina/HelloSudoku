using System;
namespace HelloSudoku.Services
{
    public class SudokuSuperGenerator : ISudokuGenerator
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

        int[,] _baseGrid;
        int[,] _puzzleGrid;

        public SudokuSuperGenerator()
        {
            _baseGrid = SudokuGridTools.GetBaseGrid();
            _puzzleGrid = SudokuGridTools.GetBaseGrid();
        }

        /// <summary>
        /// Main methods generating sudoku. out params: sudoku field and filled grid.
        /// </summary>
        /// <param name="Level"></param>
        /// <param name="BaseGrid"></param>
        /// <param name="PlayerGrid"></param>
        public void GenerateSudokuGrid(int Level, out int[,] BaseGrid, out int[,] PlayerGrid)
        {
            BaseGrid = new int[9, 9];
            PlayerGrid = new int[9, 9];

            this._gameLevel = Level;

            if (GenerateSudokuGrid())
            {
                BaseGrid = _baseGrid;
                PlayerGrid = _puzzleGrid;
            }
        }

        /// <summary>
        /// Main methods generating sudoku field and its complete verision.
        /// </summary>
        /// <returns>returns true if sudoku grids successfully generated. if some error - returns false.</returns>
        public bool GenerateSudokuGrid()
        {
            try
            {
                ShuffleGrid();
                Array.Copy(_baseGrid, _puzzleGrid, _baseGrid.GetLength(0) * _baseGrid.GetLength(1));
                _puzzleGrid = GridManager.RemoveCellAndSolve(_baseGrid, _sudokuComplexity);
            }
            catch (Exception e)
            {
                Console.WriteLine("'{0}' Exception caught.", e);
                return false;
            }
            return true;
        }

        void ShuffleGrid()
        {
            foreach (int nStep in GenerateRandomArrayOfIntegers(5, 500))
            {
                var del = GetMatrixChangeMethod(nStep);
                if (del != null) _baseGrid = del.Invoke(_baseGrid);
            }
        }

        #region Additional methods
        Func<int[,], int[,]> GetMatrixChangeMethod(int n, bool printInCns = false)
        {
            switch (n)
            {
                case 1:
                default:
                    if (printInCns) Console.WriteLine("Transpose");
                    return GridManager.Transpose;
                case 2:
                    if (printInCns) Console.WriteLine("SwapRows_small");
                    return GridManager.SwapRows_small;
                case 3:
                    if (printInCns) Console.WriteLine("SwapColumns_small");
                    return GridManager.SwapColumns_small;
                case 4:
                    if (printInCns) Console.WriteLine("SwapAreasHorizontally");
                    return GridManager.SwapAreasHorizontally;
                case 5:
                    if (printInCns) Console.WriteLine("SwapAreasVertically");
                    return GridManager.SwapAreasVertically;
            }
        }

        // Generates array of nmb numbers in range (1,Max).
        private int[] GenerateRandomArrayOfIntegers(int Max, int nmb)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int[] result = Enumerable.Repeat(0, nmb).Select(i => rnd.Next(1, Max)).ToArray();
            return result;
        }

        #endregion Additional methods

    }
}

