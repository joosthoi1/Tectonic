using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PuzzleSolver.Models.Puzzles;

public abstract class PuzzleBase : IGameBoard, INotifyPropertyChanged
{
    protected static IEnumerable<(int x, int y)> GetNeighbors((int x, int y) coord)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;
                yield return (coord.x + dx, coord.y + dy);
            }
        }
    }


    protected ObservableCollection<PositionedCelldata> _cells;
    protected string title;
    protected int y;
    protected int x;
    
    
    public List<Color> Colors { get; set; }
    public string Title { get => title; set { title = value; OnPropertyChanged(nameof(Title)); } }
    public int X { get => x; protected set { x = value; OnPropertyChanged(nameof(X)); } }
    public int Y { get => y; protected set { y = value; OnPropertyChanged(nameof(Y)); } }
    public ObservableCollection<PositionedCelldata> Cells { get => _cells; set { _cells = value; OnPropertyChanged(nameof(Cells)); } }
    protected Stack<(int x, int y)> TriedCoordinates { get; set; }
    protected Stack<ObservableCollection<PositionedCelldata>> BoardPositions { get; set; }
    protected Stack<ObservableCollection<PositionedCelldata>> PreviousBoardPositions { get; set; }
    protected Stack<ObservableCollection<PositionedCelldata>> NextBoardPositions { get; set; }

    public PuzzleType PuzzleType { get; set; }

    public virtual async Task CheckNext(bool untilStuck)
    {
        PreviousBoardPositions.Push(DeepCopyCells());
        NextBoardPositions.Clear();
        OnPropertyChanged(nameof(PreviousBoardPositions));
    }

    public (bool Top, bool Right, bool Bottom, bool Left) GetBorders(PositionedCelldata cell)
    {
        return (
            HasTopBorder(cell),
            HasRightBorder(cell),
            HasBottomBorder(cell),
            HasLeftBorder(cell)
        );
    }
    protected bool HasTopBorder(PositionedCelldata cell)
    {
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row - 1, col);
        return top == null;
    }
    protected bool HasLeftBorder(PositionedCelldata cell)
    {
        var (row, col) = GetPosition(cell);
        var left = GetCellAt(row, col - 1);
        return left == null;
    }
    protected bool HasRightBorder(PositionedCelldata cell)
    {
        var (row, col) = GetPosition(cell);
        var right = GetCellAt(row, col + 1);
        return right == null || right.Group != cell.Group;
    }

    protected bool HasBottomBorder(PositionedCelldata cell)
    {
        var (row, col) = GetPosition(cell);
        var bottom = GetCellAt(row + 1, col);
        return bottom == null || bottom.Group != cell.Group;
    }


    public PositionedCelldata GetCellAt(int row, int col)
    {
        if (row < 0 || row >= Y || col < 0 || col >= X)
        {
            return null;
        }
        int index = row * X + col;
        if (index >= 0 && index < Cells.Count)
            return Cells[index];
        return null;
    }

    public (int Row, int Col) GetPosition(PositionedCelldata cell)
    {
        int index = Cells.IndexOf(cell);
        if (index == -1) return (-1, -1);
        return (index / X, index % X);
    }
    protected bool SetCompletedNormal()
    {
        bool returns = false;
        var cells = Cells.Where(cell => cell.HasSmallNumbers && cell.SmallNumbers.Count == 1);
        foreach (var cell in cells)
        {
            returns = true;
            cell.BigNumber = cell.SmallNumbers.First();
        }

        return returns;
    }
    protected bool SetCompletedSpecial()
    {
        bool returns = false;
        var cellGroups = Cells.GroupBy(
            cell => cell.Group,
            cell => cell,
            (group, cells) => new
            {
                Group = group,
                Cells = cells
            }
        );
        foreach (var cg in cellGroups)
        {
            // Step 1 & 2: Flatten all lists and count frequencies of each number
            var numberFrequencies = cg.Cells
                .Where(obj => obj.HasSmallNumbers)
                .SelectMany(obj => obj.SmallNumbers.Select(n => new { Obj = obj, Number = n })) // Project both object and number
                .GroupBy(item => item.Number)           // Group by the number itself
                .ToDictionary(group => group.Key, group => group.Count()); // Create a dictionary: number -> count

            // Step 3: Identify numbers that appear exactly once across all lists
            var uniqueNumbers = numberFrequencies
                .Where(kvp => kvp.Value == 1) // Filter for counts == 1
                .Select(kvp => kvp.Key)       // Select the number (the key)
                .ToHashSet();                 // Use a HashSet for efficient lookups (O(1) average)

            if (!uniqueNumbers.Any())
            {
                continue;
            }

            // Step 4: Find the objects that contain these unique numbers, and pair them
            var result = cg.Cells
                .Where(obj => obj.HasSmallNumbers)
                .SelectMany(obj => obj.SmallNumbers
                    .Where(num => uniqueNumbers.Contains(num))
                    .Select(num => new Tuple<PositionedCelldata, int>(obj, num))) // Pair object with unique number
                .ToList();

            foreach (var x in result)
            {
                returns = true;
                x.Item1.BigNumber = x.Item2;
            }
        }


        return returns;
    }
    protected bool CheckGroup()
    {
        bool returns = false;

        var cellGroups = Cells.GroupBy(
            cell => cell.Group,
            cell => cell,
            (group, cells) => new
            {
                Group = group,
                Cells = cells
            }
        );
        foreach (var cellgroup in cellGroups)
        {
            var cellnumbers = cellgroup.Cells.Where(c => c.BigNumber is not null);
            foreach (var cell in cellgroup.Cells)
            {
                foreach (var n in cellnumbers)
                {
                    if (cell.SmallNumbers?.Remove(n.BigNumber!.Value) ?? false)
                    {
                        returns = true;
                    }
                }
            }
        }

        return returns;
    }
    protected bool SetupNumbers()
    {
        if (Cells.Any(c=>c.HasSmallNumbers) && !Cells.All(c=>c.BigNumber is not null))
        {
            return false;
        }
        foreach (PositionedCelldata cell in Cells)
        {
            var count = GetGroupCount(cell.Group);
            if (cell.BigNumber is null)
            {
                cell.SmallNumbers = new(Enumerable.Range(1, count));
            }
        }
        return true;
    }
    protected int GetGroupCount(int group)
    {
        return Cells.Count(c => c.Group == group);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    protected abstract SolvedState GetSolvedState();
    protected bool TryNewNumber(SolvedState solvedState)
    {
        if (solvedState == SolvedState.Unsolvable || solvedState == SolvedState.Invalid)
        {
            (int x, int y) = TriedCoordinates.Pop();
            var oldValue = GetCellAt(x, y).BigNumber!.Value;

            Cells = BoardPositions.Pop();

            var cell = GetCellAt(x, y);
            cell.SmallNumbers.Remove(oldValue);
            cell.BigNumber = null;

            return false;
        }

        foreach (var cell in Cells)
        {
            if (cell.BigNumber is null && cell.SmallNumbers.Count > 0)
            {
                TriedCoordinates.Push(GetPosition(cell));
                BoardPositions.Push(DeepCopyCells());
                cell.BigNumber = cell.SmallNumbers.Last();
                return true;
            }
        }
        return false; // Already solved??
    }
    protected ObservableCollection<PositionedCelldata> DeepCopyCells()
    {
        var copy = new ObservableCollection<PositionedCelldata>();
        foreach (var cell in Cells)
        {
            copy.Add(new PositionedCelldata
            {
                ParentBoard = cell.ParentBoard,
                Group = cell.Group,
                BigNumber = cell.BigNumber,
                SmallNumbers = new ObservableCollection<int>(cell.SmallNumbers ?? Enumerable.Empty<int>()),
            });
        }
        return copy;
    }

    public void NextState()
    {
        PreviousBoardPositions.Push(DeepCopyCells());
        OnPropertyChanged(nameof(PreviousBoardPositions));
        Cells = NextBoardPositions.Pop();
    }

    public void PreviousState()
    {
        NextBoardPositions.Push(DeepCopyCells());
        OnPropertyChanged(nameof(NextBoardPositions));
        Cells = PreviousBoardPositions.Pop();
    }
    public bool HasPreviousState { get => PreviousBoardPositions.Count > 0; }
    public bool HasNextState { get => NextBoardPositions.Count > 0; }
}
