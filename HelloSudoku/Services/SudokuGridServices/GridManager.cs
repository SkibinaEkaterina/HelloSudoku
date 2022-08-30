using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace HelloSudoku.Services
{
    #region GridManager
    public static class GridManager
    {
        //RandomNumberGenerator rand = RandomNumberGenerator.Create();
        static Random rnd = new Random((int)DateTime.Now.Ticks);

        static int nmbOfAreas = 3;
        static int nmbOfLinesInArea = 3;
        static int Nn = nmbOfLinesInArea * nmbOfAreas;


        #region Additional
        private static bool CheckSquareMatrix(int[,] grid)
        {
            if (grid.GetLength(0) != grid.GetLength(1))
            {
                Console.WriteLine("square matrix is expected");
                return false;
            }

            if (grid.GetLength(0) != nmbOfAreas * nmbOfLinesInArea)
            {
                Console.WriteLine("Matrix has invalid parameters.");
            }
            return true;
        }

        private static void SwapElements(ref int a, ref int b)
        {
            int c = 0;
            c = a;
            a = b;
            b = c;
        }
        #endregion


        // Transpose matrix
        public static int[,] Transpose(int[,] grid)
        {
            int N1 = grid.GetLength(0), N2 = grid.GetLength(1);
            var result = new int[N1, N2];

            for (var c = 0; c < N1; c++)
                for (var r = 0; r < N2; r++)
                    result[c, r] = grid[r, c];

            return result;
        }

        // Обмен двух строк в пределах одного района
        public static int[,] SwapRows_small(int[,] grid)
        {
            if (!CheckSquareMatrix(grid)) return grid;

            int numLineArea = rnd.Next(2);
            int[] lineNmbs = Enumerable.Range(0, nmbOfAreas - 1).ToArray();

            lineNmbs = lineNmbs.OrderBy(x => rnd.Next()).ToArray();

            int nmbLine1 = numLineArea * nmbOfLinesInArea + lineNmbs[0];
            int nmbLine2 = numLineArea * nmbOfLinesInArea + lineNmbs[1];

            // swap for all columns
            for (int nc = 0; nc < grid.GetLength(0); nc++)
            {
                SwapElements(ref grid[nmbLine1, nc], ref grid[nmbLine2, nc]);
            }

            return grid;
        }

        // Обмен двух столбцов в пределах одного района
        public static int[,] SwapColumns_small(int[,] grid)
        {
            if (!CheckSquareMatrix(grid)) return grid;

            grid = Transpose(grid);
            grid = SwapRows_small(grid);
            grid = Transpose(grid);

            return grid;
        }

        // Обмен двух районов по горизонтали (swap_rows_area)
        public static int[,] SwapAreasHorizontally(int[,] grid)
        {
            if (!CheckSquareMatrix(grid)) return grid;
            int[] areasNmbs = Enumerable.Range(0, nmbOfAreas - 1).ToArray();

            areasNmbs = areasNmbs.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < nmbOfLinesInArea; i++)
            {
                int nmbLine1 = areasNmbs[0] * nmbOfLinesInArea + i;
                int nmbLine2 = areasNmbs[1] * nmbOfLinesInArea + i;
                for (int nc = 0; nc < grid.GetLength(0); nc++)
                {
                    SwapElements(ref grid[nmbLine1, nc], ref grid[nmbLine2, nc]);
                }
            }

            return grid;
        }

        // Обмен двух районов по вертикали(swap_colums_area)
        public static int[,] SwapAreasVertically(int[,] grid)
        {
            if (!CheckSquareMatrix(grid)) return grid;

            grid = Transpose(grid);
            grid = SwapAreasHorizontally(grid);
            grid = Transpose(grid);

            return grid;
        }

        // Remove cells in final sudoku grid
        public static int[,] RemoveCellAndSolve(int[,] grid, int cmplx)
        {
            int[,] resultGrid = new int[Nn, Nn];
            Array.Copy(grid, resultGrid, grid.GetLength(0) * grid.GetLength(1));

            int[,] flook = new int[Nn, Nn];
            int iterator = 0;
            int difficult = (int)Math.Pow(nmbOfLinesInArea, 4);

            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            engine.ExecuteFile("/Users/ekaterina/Projects/sudokuGenerator/sudokuGenerator/SudokuGenerator/SudokuSolverPy.py", scope);
            dynamic getNmbOfSolutions = scope.GetVariable("getNmbOfSolutions");

            while (iterator < (int)Math.Pow(Nn, 4))
            {
                int i = rnd.Next(0, Nn);
                int j = rnd.Next(0, Nn);

                if (flook[i, j] == 0)
                {
                    iterator += 1;
                    flook[i, j] = 1;

                    int temp = resultGrid[i, j];
                    resultGrid[i, j] = 0;

                    List<List<int>> mtrx = new();
                    for (int k = 0; k < Nn; k++)
                    {
                        List<int> ll = new();
                        for (int m = 0; m < Nn; m++)
                        {
                            ll.Add(resultGrid[k, m]);
                        }
                        mtrx.Add(ll);
                    }

                    difficult -= 1;

                    dynamic result = getNmbOfSolutions(mtrx);

                    if (result != null && result != 1)
                    {
                        resultGrid[i, j] = temp;
                        difficult += 1;
                    }

                    if (difficult == cmplx) break;
                }
            }
            return resultGrid;
        }
    }
    #endregion Matrix generator
}

