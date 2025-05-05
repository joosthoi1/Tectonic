using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tectonic.Commands;
using Tectonic.Models;
using Tectonic.Pages;
using Tectonic.Providers;

namespace Tectonic.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private GameBoard board;

    public GameBoard Board { get => board; set { board = value; OnPropertyChanged(nameof(Board)); } }
    public ICommand CheckNextCommand { get; }
    public ICommand CheckAllCommand { get; }
    public ICommand WriteGroupsCommand { get; }
    public ICommand WriteValuesCommand { get; }
    private PuzzleProvider puzzleProvider { get; init; }

    public MainViewModel()
    {

        puzzleProvider = new PuzzleProvider("puzzles.json");
        Board = new GameBoard(9, 11, puzzleProvider.GetNameSuffix("New Puzzle"));

        CheckNextCommand = new RelayCommand(() => _ = Board.CheckNext(false));
        CheckAllCommand = new RelayCommand(() => _ = Board.CheckNext(true));
        WriteGroupsCommand = new RelayCommand(() =>
        {
            puzzleProvider.SavePuzzle(Board);

            MainWindow.AppNavigationService.Navigate(new PuzzleSelector());
        });
    }
    public void LoadPuzzle(string name)
    {
        Board = puzzleProvider.GetPuzzleByName(name);
    }
    public void LoadPuzzle()
    {
        GameBoard puzzle = new GameBoard(9, 11, new int[9 * 11], new int[9 * 11], puzzleProvider.GetNameSuffix("New Puzzle"));
        Board = puzzle;
    }
    public void LoadPuzzle(GameBoard puzzle)
    {
        Board = puzzle;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
