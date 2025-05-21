using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using PuzzleSolver.Commands;
using PuzzleSolver.Models.Puzzles;
using PuzzleSolver.Providers;

namespace PuzzleSolver.ViewModels
{
    class PuzzleSelectorViewModel : INotifyPropertyChanged
    {
        private readonly PuzzleProvider puzzleProvider;
        private ObservableCollection<string> _puzzles;
        private PuzzleType _selectedPuzzleType;
        private int _newY = 11;
        private int _newX = 9;

        public ObservableCollection<string> Puzzles { get => _puzzles; set { _puzzles = value; OnPropertyChanged(nameof(Puzzles)); } }
        public ICommand SolvePuzzleCommand { get; }
        public ICommand EditPuzzleCommand { get; }
        public ICommand RemovePuzzleCommand { get; }
        public ICommand NewPuzzleCommand { get; }

        public int NewX { get => _newX; set { _newX = value; OnPropertyChanged(nameof(NewX)); } }
        public int NewY { get => _newY; set { _newY = value; OnPropertyChanged(nameof(NewY)); } }
        public string NewTitle { get; set; }
        public Array PuzzleTypes { get; set; } = Enum.GetValues(typeof(PuzzleType));
        public PuzzleType SelectedPuzzleType { get => _selectedPuzzleType; set { 
                _selectedPuzzleType = value;
                if (_selectedPuzzleType == PuzzleType.Sudoku)
                {
                    NewX = NewY = 9;
                }
                else
                {
                    NewX = 9;
                    NewY = 11;
                }
            } }

        public PuzzleSelectorViewModel()
        {
            NewX = 9;
            NewY = 11;
            SelectedPuzzleType = PuzzleType.Tectonic;
            puzzleProvider = new PuzzleProvider("puzzles.json");
            RefreshPuzzles();
            NewTitle = puzzleProvider.GetNameSuffix("New Puzzle");

            EditPuzzleCommand = new RelayCommand<string>(NavigateCreator);
            SolvePuzzleCommand = new RelayCommand<string>(NavigateSolver);
            RemovePuzzleCommand = new RelayCommand<string>((x) => { puzzleProvider.DeletePuzzle(x); RefreshPuzzles(); });
            NewPuzzleCommand = new RelayCommand(() => NavigateCreator(NewX, NewY, NewTitle, SelectedPuzzleType));
        }

        private void RefreshPuzzles()
        {
            Puzzles = new ObservableCollection<string>(puzzleProvider.GetPuzzleNames());
        }

        private void NavigateCreator(string item)
        {
            var page = new CreatorPage(item);
            
            MainWindow.AppNavigationService.Navigate(page);
        }
        private void NavigateSolver(string item)
        {
            var page = new SolverPage(item);
            MainWindow.AppNavigationService.Navigate(page);
        }
        private void NavigateCreator(int x, int y, string title, PuzzleType type)
        {
            var page = new CreatorPage(x, y, title, type);

            MainWindow.AppNavigationService.Navigate(page);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
