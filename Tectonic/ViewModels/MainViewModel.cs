using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PuzzleSolver.Commands;
using PuzzleSolver.Models;
using PuzzleSolver.Models.Puzzles;
using PuzzleSolver.Pages;
using PuzzleSolver.Providers;

namespace PuzzleSolver.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private IGameBoard board;

    public IGameBoard Board { get => board; set { board = value; OnPropertyChanged(nameof(Board)); } }
    public ICommand CheckNextCommand { get; }
    public ICommand CheckAllCommand { get; }
    public ICommand WriteGroupsCommand { get; }
    public ICommand WriteValuesCommand { get; }
    public ICommand NextStateCommand { get; }
    public ICommand PreviousStateCommand { get; }
    private PuzzleProvider puzzleProvider { get; init; }

    public MainViewModel()
    {

        puzzleProvider = new PuzzleProvider("puzzles.json");
        Board = new Tectonic(9, 11, puzzleProvider.GetNameSuffix("New Puzzle"));

        CheckNextCommand = new RelayCommand(() => _ = Board.CheckNext(false));
        CheckAllCommand = new RelayCommand(() => _ = Board.CheckNext(true));
        WriteGroupsCommand = new RelayCommand(() =>
        {
            puzzleProvider.SavePuzzle(Board);

            MainWindow.AppNavigationService.Navigate(new PuzzleSelector());
        });
        NextStateCommand = new RelayCommand(() =>
        {
            Board.NextState();
        }, () => Board.HasNextState);
        PreviousStateCommand = new RelayCommand(() =>
        {
            Board.PreviousState();
        }, () => Board.HasPreviousState);
    }
    public void LoadPuzzle(string name)
    {
        Board = puzzleProvider.GetPuzzleByName(name);
    }
    public void LoadPuzzle()
    {
        IGameBoard puzzle = new Tectonic(9, 11, puzzleProvider.GetNameSuffix("New Puzzle"));
        Board = puzzle;
    }
    public void LoadPuzzle(IGameBoard puzzle)
    {
        Board = puzzle;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
