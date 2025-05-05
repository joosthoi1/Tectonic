namespace Tectonic.Providers
{
    interface IPuzzleProvider
    {
        bool DeletePuzzle(string title);
        GameBoard? GetPuzzleByName(string title);
        IEnumerable<string> GetPuzzleNames();
        void SavePuzzle(GameBoard puzzle);
    }
}