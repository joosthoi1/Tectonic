using PuzzleSolver.Models.Puzzles;

namespace PuzzleSolver.Providers
{
    interface IPuzzleProvider
    {
        bool DeletePuzzle(string title);
        IGameBoard? GetPuzzleByName(string title);
        IEnumerable<string> GetPuzzleNames();
        void SavePuzzle(IGameBoard puzzle);
    }
}