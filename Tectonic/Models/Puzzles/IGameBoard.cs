using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PuzzleSolver.Models.Puzzles
{
    public interface IGameBoard
    {
        ObservableCollection<PositionedCelldata> Cells { get; set; }
        List<Color> Colors { get; set; }
        string Title { get; set; }
        int X { get; }
        int Y { get; }
        PuzzleType PuzzleType { get; set; }
        Task CheckNext(bool untilStuck);
        (bool Top, bool Right, bool Bottom, bool Left) GetBorders(PositionedCelldata cell);
        PositionedCelldata GetCellAt(int row, int col);
        (int Row, int Col) GetPosition(PositionedCelldata cell);
        bool HasPreviousState { get; }
        bool HasNextState { get; }
        void NextState();
        void PreviousState();
    }
}