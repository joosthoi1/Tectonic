using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace PuzzleSolver.Models.Puzzles;

public class Tectonic : PuzzleBase
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


    public Tectonic(int x, int y, int[] numbers, int[] groups, string title)
    {
        PuzzleType = PuzzleType.Tectonic;
        Cells = new ObservableCollection<PositionedCelldata>();
        TriedCoordinates = new Stack<(int x, int y)>();
        BoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
        PreviousBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
        NextBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
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
    public Tectonic(int x, int y, string title)
    {
        Cells = new ObservableCollection<PositionedCelldata>();
        TriedCoordinates = new Stack<(int x, int y)>();
        BoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
        PreviousBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
        NextBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();

        Title = title;
        Colors = ColorGenerator.GenerateColors(x * y);
        List<PositionedCelldata> GameCells = new List<PositionedCelldata>();
        for (int i = 0; i < x * y; i++)
        {
            int group = i;
            int number = 0;
            Cells.Add(new PositionedCelldata() { ParentBoard = this, Group = group, BigNumber = number == 0 ? null : number });
        }

        X = x;
        Y = y;
    }
    private bool TopCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row - 1, col);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool TopRightCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row - 1, col + 1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool RightCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row, col + 1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool BottomRightCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row + 1, col + 1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool BottomCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row + 1, col);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool BottomLeftCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row + 1, col - 1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool LeftCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row, col - 1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }
    private bool TopLeftCheck(PositionedCelldata cell)
    {
        if (cell.BigNumber is null) return false;
        var (row, col) = GetPosition(cell);
        var top = GetCellAt(row - 1, col - 1);
        if (top is null || !top.HasSmallNumbers) return false;
        int i = top.SmallNumbers.IndexOf(cell.BigNumber.Value);
        if (i == -1) return false;
        top.SmallNumbers.RemoveAt(i);
        return true;
    }

    public override async Task CheckNext(bool untilStuck)
    {
        await base.CheckNext(untilStuck);
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
            if (CheckAllNeighbors()) { Trace.WriteLine("Checked whether all small numbers of a group were neighboring a big number of another group"); continue; }

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

    private bool CheckAllNeighbors()
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

        foreach (var cellGroup in cellGroups)
        {
            Dictionary<int, List<PositionedCelldata>> cellsPerSmallNumber = new();
            foreach (var cell in cellGroup.Cells.Where(c => c.HasSmallNumbers))
            {
                foreach (int smallN in cell.SmallNumbers)
                {
                    if (!cellsPerSmallNumber.ContainsKey(smallN))
                    {
                        cellsPerSmallNumber[smallN] = new List<PositionedCelldata>();
                    }
                    cellsPerSmallNumber[smallN].Add(cell);
                }
            }
            foreach (var SmallNumberCells in cellsPerSmallNumber)
            {
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
    private bool CheckBigNumbers()
    {
        bool returns = false;

        foreach (var cell in Cells)
        {
            if (cell.BigNumber is not null)
            {
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
    protected override SolvedState GetSolvedState()
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
}
