using System;
namespace HelloSudoku.Services
{
    internal static class SudokuGridTools
    {
        static int nmbOfAreas = 3;
        static int nmbOfLinesInArea = 3;

        public static int[,] GetBaseGrid()
        {
            int[,] grid = new int[nmbOfAreas * nmbOfLinesInArea, nmbOfAreas * nmbOfLinesInArea];

            for (int i = 0; i < nmbOfAreas * nmbOfLinesInArea; i++)
            {
                for (int j = 0; j < nmbOfAreas * nmbOfLinesInArea; j++)
                {
                    grid[i, j] = ((i * nmbOfAreas + i / nmbOfLinesInArea + j) % (nmbOfAreas * nmbOfLinesInArea) + 1);
                }
            }

            return grid;
        }

        public static void PrintGrid(int[,] grid)
        {
            string borderLine = new System.Text.StringBuilder("+---".Length * 3).Insert(0, "+---", 9).ToString() + "+";

            // start Print
            for (int i = 0; i < nmbOfAreas * nmbOfLinesInArea; i++)
            {

                Console.WriteLine(borderLine);
                string txtToShow = "|";
                for (int j = 0; j < nmbOfAreas * nmbOfLinesInArea; j++)
                {
                    txtToShow += " " + (grid[i, j] == 0 ? " " : grid[i, j]) + " |";
                }
                Console.WriteLine(txtToShow);
            }

            Console.WriteLine(borderLine);
        }

        public static void PrintGrid2(int[,] grid)
        {
            Console.WriteLine("{");

            // start Print
            for (int i = 0; i < nmbOfAreas * nmbOfLinesInArea; i++)
            {
                string txtToShow = "{";
                for (int j = 0; j < nmbOfAreas * nmbOfLinesInArea; j++)
                {
                    txtToShow += grid[i, j] + (j == nmbOfAreas * nmbOfLinesInArea - 1 ? "" : ", ");
                }
                txtToShow += "}, ";
                Console.WriteLine(txtToShow);
            }

            Console.WriteLine("}");
        }

        // public string SerializeGridToString()
        // public string DeSerializeGridFromString()
    }
}

