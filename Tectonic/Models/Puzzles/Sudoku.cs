using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace PuzzleSolver.Models.Puzzles
{
    internal class Sudoku : PuzzleBase
    {
        private int[] _groups = new int[81]
            {
                0, 0, 0, 1, 1, 1, 2, 2, 2,
                0, 0, 0, 1, 1, 1, 2, 2, 2,
                0, 0, 0, 1, 1, 1, 2, 2, 2,
                3, 3, 3, 4, 4, 4, 5, 5, 5,
                3, 3, 3, 4, 4, 4, 5, 5, 5,
                3, 3, 3, 4, 4, 4, 5, 5, 5,
                6, 6, 6, 7, 7, 7, 8, 8, 8,
                6, 6, 6, 7, 7, 7, 8, 8, 8,
                6, 6, 6, 7, 7, 7, 8, 8, 8,
            };

        public Sudoku(int[] numbers, string title)
        {
            PuzzleType = PuzzleType.Sudoku;
            X = 9;
            Y = 9;
            Cells = new ObservableCollection<PositionedCelldata>();
            TriedCoordinates = new Stack<(int x, int y)>();
            BoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
            PreviousBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
            NextBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
            Title = title;
            Colors = ColorGenerator.GenerateColors(x * y);
            List<PositionedCelldata> GameCells = new List<PositionedCelldata>();


            for (int i = 0; i < X * Y; i++)
            {
                int group = _groups[i];
                int number = numbers[i];
                Cells.Add(new PositionedCelldata() { ParentBoard = this, Group = group, BigNumber = number == 0 ? null : number });
            }

        }
        public Sudoku(string title)
        {
            PuzzleType = PuzzleType.Sudoku;
            X = 9;
            Y = 9;
            Cells = new ObservableCollection<PositionedCelldata>();
            TriedCoordinates = new Stack<(int x, int y)>();
            BoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
            PreviousBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
            NextBoardPositions = new Stack<ObservableCollection<PositionedCelldata>>();
            Title = title;
            Colors = ColorGenerator.GenerateColors(x * y);
            List<PositionedCelldata> GameCells = new List<PositionedCelldata>();
            

            for (int i = 0; i < X * Y; i++)
            {
                int group = _groups[i];
                Cells.Add(new PositionedCelldata() { ParentBoard = this, Group = group, BigNumber = null });
            }
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
                if (SetCompletedSpecial()) { Trace.WriteLine("Added a big number where it was the only occurrence in that group"); continue; }
                if (SetCompletedNormal()) { Trace.WriteLine("Added a big number where it was the only possible for that cell"); continue; }
                if (RemoveHorizontalVertical()) { Trace.WriteLine("Removed small numbers in a row or column where a big number was"); continue; }
                if (RemoveHorizontalVerticalSpecial()) { Trace.WriteLine("Removed small numbers in a row or column where all small number were in the same row or col"); continue; }
                if (solvedState == SolvedState.Solved)
                {
                    MessageBox.Show("Found nothing else");
                    untilStuck = false;
                }
                else
                {
                    if (TryNewNumber(solvedState)) { (int x, int y) = TriedCoordinates.Peek(); var num = GetCellAt(x, y).BigNumber; Trace.WriteLine($"Trying number {num} in cell x: {x} y: {y}"); MessageBox.Show("Had to guess"); }
                    else { Trace.WriteLine($"Removed last tried number"); }
                }
            } while (untilStuck);
        }

        private bool RemoveHorizontalVerticalSpecial()
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
                for (int i = 1; i <= 9; i++) {
                    var smallNums = cg.Cells.Where(c => c.HasSmallNumbers && c.SmallNumbers.Contains(i));
                    if (smallNums.Count() == 0)
                    {
                        continue;
                    }
                    var fRow = GetPosition(smallNums.First()).Row;
                    var fCol = GetPosition(smallNums.First()).Col;
                    if (smallNums.All(c => GetPosition(c).Row == fRow))
                    {
                        if (RemoveSmallNumbersInRow(fRow, i, cg.Group)) returns = true;
                    }
                    if (smallNums.All(c => GetPosition(c).Col == fCol))
                    {
                        if (RemoveSmallNumbersInColumn(fCol, i, cg.Group)) returns = true;
                    }

                }
            }

            return returns;
        }
        private bool RemoveSmallNumbersInRow(int row, int num, int? excludeGroup = null)
        {
            bool returns = false;
            for (int i = 0; i < 9; i++)
            {
                PositionedCelldata curCell = GetCellAt(row, i);
                if (excludeGroup is not null && curCell.Group == excludeGroup) continue;
                if (curCell.HasSmallNumbers && curCell.SmallNumbers.Contains(num))
                {
                    curCell.SmallNumbers.Remove(num);
                    returns = true;
                }
            }
            return returns;
        }
        private bool RemoveSmallNumbersInColumn(int col, int num, int? excludeGroup = null)
        {
            bool returns = false;
            for (int i = 0; i < 9; i++)
            {
                PositionedCelldata curCell = GetCellAt(i, col);
                if (excludeGroup is not null && curCell.Group == excludeGroup) continue;
                if (curCell.HasSmallNumbers && curCell.SmallNumbers.Contains(num))
                {
                    curCell.SmallNumbers.Remove(num);
                    returns = true;
                }
            }
            return returns;
        }
        private bool RemoveHorizontalVertical()
        {
            bool returns = false;

            foreach (PositionedCelldata cell in Cells.Where(c => c.BigNumber is not null))
            {
                int n = cell.BigNumber!.Value;
                (int bigX, int bigY) = GetPosition(cell);

                if (RemoveSmallNumbersInColumn(bigY, n)) { returns = true; }
                if (RemoveSmallNumbersInRow(bigX, n)) { returns = true; }
            }

            return returns;
        }

        protected override SolvedState GetSolvedState()
        {
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
}
