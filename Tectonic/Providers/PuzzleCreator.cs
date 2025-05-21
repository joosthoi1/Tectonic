using PuzzleSolver.Models.Puzzles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver.Providers
{
    public static class PuzzleCreator
    {
        public static IGameBoard CreatePuzzle(int x, int y, int[] numbers, int[] groups, string title, PuzzleType puzzleType)
        {
            switch (puzzleType)
            {
                case PuzzleType.Tectonic:
                    return new Tectonic(x, y, numbers, groups, title);
                case PuzzleType.Sudoku:
                    return new Sudoku(numbers, title);
                default:
                    return new Tectonic(x,y,numbers,groups,title);
            }
        }
        public static IGameBoard CreatePuzzle(int x, int y, string title, PuzzleType puzzleType)
        {
            switch (puzzleType)
            {
                case PuzzleType.Tectonic:
                    return new Tectonic(x, y, title);
                case PuzzleType.Sudoku:
                    return new Sudoku(title);
                default:
                    return new Tectonic(x, y, title);
            }
        }
    }
}
