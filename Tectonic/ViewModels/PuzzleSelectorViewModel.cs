using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Tectonic.Commands;
using Tectonic.Providers;

namespace Tectonic.ViewModels
{
    class PuzzleSelectorViewModel
    {
        private readonly PuzzleProvider puzzleProvider;

        public IEnumerable<string> Puzzles { get; set; }
        public ICommand SolvePuzzleCommand { get; }
        public ICommand EditPuzzleCommand { get; }
        public ICommand NewPuzzleCommand { get; }

        public int NewX { get; set; } = 9;
        public int NewY { get; set; } = 11;
        public string NewTitle { get; set; }

        public PuzzleSelectorViewModel()
        {

            puzzleProvider = new PuzzleProvider("puzzles.json");
            Puzzles = puzzleProvider.GetPuzzleNames();
            NewTitle = puzzleProvider.GetNameSuffix("New Puzzle");
            
            EditPuzzleCommand = new RelayCommand<string>(NavigateCreator);
            SolvePuzzleCommand = new RelayCommand<string>(NavigateSolver);
            NewPuzzleCommand = new RelayCommand(()=>NavigateCreator(NewX, NewY, NewTitle));
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
        private void NavigateCreator(int x, int y, string title)
        {
            var page = new CreatorPage(x, y, title);

            MainWindow.AppNavigationService.Navigate(page);
        }
    }
}
