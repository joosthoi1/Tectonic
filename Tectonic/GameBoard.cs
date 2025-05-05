using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Tectonic.Models;

namespace Tectonic;

public class GameBoard : INotifyPropertyChanged
{
    private int[] groups =
    {
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,

    };
    private int[] numbers =
    {
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,
    };
    [JsonIgnore]
    public List<Color> Colors { get; set; }

    private Stack<(int x, int y)> TriedCoordinates { get; set; }
    private Stack<ObservableCollection<PositionedCelldata>> BoardPositions { get; set; }
    public string Title { get => title; set { title = value; OnPropertyChanged(nameof(Title)); } }
    public int X { get => x; set { x = value; OnPropertyChanged(nameof(X)); } }
    public int Y { get => y; set { y = value; OnPropertyChanged(nameof(Y)); } }
    public ObservableCollection<PositionedCelldata> Cells { get => _cells; set { _cells = value; OnPropertyChanged(nameof(Cells)); } }

    public GameBoard(int x, int y, int[] numbers, int[] groups, string title)
    {
        Cells = new ObservableCollection<PositionedCelldata>();
        TriedCoordinates = new Stack<(int x, int y)>();
        BoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
        Title = title;
        Colors = ColorGenerator.GenerateColors(x * y);
        List<PositionedCelldata> GameCells = new List<PositionedCelldata>();
        for (int i = 0; i < groups.Length; i++)
        {
            int group = groups[i];
            int number = numbers[i];
            Cells.Add(new PositionedCelldata() { ParentBoard = this, Group = group, BigNumber = number == 0 ? null : number });
        }

        X = x;
        Y = y;
    }
    public GameBoard(int x, int y, string title)
    {
        Cells = new ObservableCollection<PositionedCelldata>();
        TriedCoordinates = new Stack<(int x, int y)>();
        BoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
        Title = title;
        Colors = ColorGenerator.GenerateColors(x*y);
        List<PositionedCelldata> GameCells = new List<PositionedCelldata>();
        for (int i = 0; i < groups.Length; i++)
        {
            int group = groups[i];
            int number = numbers[i];
            Cells.Add(new PositionedCelldata() { ParentBoard = this, Group = group, BigNumber = number == 0 ? null : number});
        }

        X = x;
        Y = y;
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

    public bool HasRightBorder(PositionedCelldata cell)
    {
        var (row, col) = GetPosition(cell);
        if (col >= X - 1) return false;

        var right = GetCellAt(row, col + 1);
        return right != null && right.Group != cell.Group;
    }

    public bool HasBottomBorder(PositionedCelldata cell)
    {
        var (row, col) = GetPosition(cell);
        var bottom = GetCellAt(row + 1, col);
        return bottom != null && bottom.Group != cell.Group;
    }

    public bool TopCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var(row, col) = GetPosition(cell);
        var top = GetCellAt(row-1, col);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool TopRightCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var(row, col) = GetPosition(cell);
        var top = GetCellAt(row-1, col+1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool RightCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row, col+1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool BottomRightCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row+1, col+1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool BottomCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row+1, col);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool BottomLeftCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row+1, col-1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool LeftCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row, col-1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    public bool TopLeftCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row-1, col-1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }

    private bool Setup = false;
    private int y;
    private int x;
    private string title;
    private ObservableCollection<PositionedCelldata> _cells;

    public async Task CheckNext(bool untilStuck)
    {
        do
        {
            Application.Current.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
            await Task.Delay(10);
            if (SetupNumbers()) { Trace.WriteLine("Setup"); continue; }
            var solvedState = GetSolvedState();
            if (solvedState == SolvedState.Invalid || solvedState == SolvedState.Unsolvable)
            {
                TryNewNumber(solvedState);
                Trace.WriteLine($"Removed last tried number");
                continue;
            }
            if (CheckGroup()) { Trace.WriteLine("Removed small numbers in a group"); continue; }
            if (CheckBigNumbers()) { Trace.WriteLine("Removed small numbers from a neighboring groups big numbers"); continue; }
            if (SetCompletedSpecial()) { Trace.WriteLine("Added a big number where it was the only occurrence in that group"); continue; }
            if (SetCompletedNormal()) { Trace.WriteLine("Added a big number where it was the only possible for that cell"); continue; }
            if (CheckAllNeighbours()) { Trace.WriteLine("Checked whether all small numbers of a group were neighboring a big number of another group"); continue; }

            if (solvedState == SolvedState.Solved)
            {
                MessageBox.Show("Found nothing else");
                untilStuck = false;
            }
            else
            {
                if (TryNewNumber(solvedState)) { (int x, int y) = TriedCoordinates.Peek(); var num = GetCellAt(x, y).BigNumber; Trace.WriteLine($"Trying number {num} in cell x: {x} y: {y}"); }
                else { Trace.WriteLine($"Removed last tried number"); }
            }
        } while (untilStuck);
    }

    private bool CheckAllNeighbours()
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

        foreach (var cellGroup in cellGroups) {
            Dictionary<int, List<PositionedCelldata>> cellsPerSmallNumber = new();
            foreach (var cell in cellGroup.Cells.Where(c=>c.HasSmallNumbers)) {
                foreach (int smallN in cell.SmallNumbers)
                {
                    if (!cellsPerSmallNumber.ContainsKey(smallN))
                    {
                        cellsPerSmallNumber[smallN] = new List<PositionedCelldata>();
                    }
                    cellsPerSmallNumber[smallN].Add(cell);
                }
            }
            foreach (var SmallNumberCells in cellsPerSmallNumber) {
                List<(int x, int y)> intersectedXYs = new();
                var intersection = GetNeighbors(GetPosition(SmallNumberCells.Value[0])).ToHashSet();

                foreach (var cell in SmallNumberCells.Value.Skip(1))
                {
                    var neighbors = GetNeighbors(GetPosition(cell));
                    intersection.IntersectWith(neighbors);
                }

                Console.WriteLine("Coordinates touched by all:");
                foreach (var coord in intersection)
                {
                    var c = GetCellAt(coord.x, coord.y);
                    if (c is not null && c.HasSmallNumbers)
                    {
                        if (c.SmallNumbers.Remove(SmallNumberCells.Key))
                        {
                            returns = true;
                        }
                        
                    }
                    Console.WriteLine(coord);
                }
            }
        }

        return returns;
    }
    static IEnumerable<(int x, int y)> GetNeighbors((int x, int y) coord)
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

    private bool SetCompletedNormal()
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
    private bool SetCompletedSpecial() {
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

    private bool CheckGroup()
    {
        bool returns = false;

        var cellGroups = Cells.GroupBy(
            cell => cell.Group,
            cell => cell,
            (group, cells) =>new
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

    private bool SetupNumbers()
    {
        if (Setup)
        {
            return false;
        }
        Setup = true;
        foreach (PositionedCelldata cell in Cells)
        {
            var count = this.GetGroupCount(cell.Group);
            if (cell.BigNumber is null)
            {
                cell.SmallNumbers = new (Enumerable.Range(1, count));
            }
        }
        return true;
    }

    private int GetGroupCount(int group)
    {
        return Cells.Count(c => c.Group == group);
    }

    private bool CheckBigNumbers()
    {
        bool returns = false;

        foreach (var cell in Cells)
        {
            if (cell.BigNumber is not null) {
                if (TopCheck(cell)) returns = true;
                if (TopRightCheck(cell)) returns = true;
                if (RightCheck(cell)) returns = true;
                if (BottomRightCheck(cell)) returns = true;
                if (BottomCheck(cell)) returns = true;
                if (BottomLeftCheck(cell)) returns = true;
                if (LeftCheck(cell)) returns = true;
                if (TopLeftCheck(cell)) returns = true;
            }
        }

        return returns;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private SolvedState GetSolvedState()
    {
        int[] perms = [1, 0, -1];
        foreach (var cell in Cells)
        {
            if (cell.BigNumber is null) continue;
            foreach (var x in perms)
            {
                foreach (var y in perms)
                {
                    var (row, col) = GetPosition(cell);
                    var neighbor = GetCellAt(row + y, col + x);
                    if (neighbor is not null && neighbor.Group != cell.Group && neighbor.BigNumber == cell.BigNumber)
                    {
                        return SolvedState.Invalid;
                    }

                }
            }
        }
        if (Cells.All((c) => c.BigNumber is not null))
        {
            return SolvedState.Solved;
        }
        if (Cells.Where(c => c.HasSmallNumbers).All((c) => c.SmallNumbers.Count == 0))
        {
            return SolvedState.Unsolvable;
        }
        return SolvedState.NotSolved;
    }

    private bool TryNewNumber(SolvedState solvedState)
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
    private ObservableCollection<PositionedCelldata> DeepCopyCells()
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
}

public enum SolvedState
{
    NotSolved,
    Solved,
    Unsolvable,
    Invalid
}