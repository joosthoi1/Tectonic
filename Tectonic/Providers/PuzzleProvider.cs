﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PuzzleSolver.Models;
using PuzzleSolver.Models.Puzzles;

namespace PuzzleSolver.Providers
{
    class PuzzleProvider : IPuzzleProvider
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        private Dictionary<string, SavePuzzleModel> _puzzles;
        public PuzzleProvider(string _filePath)
        {
            this._filePath = _filePath;

            LoadAll();
        }

        private void LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                _puzzles = new Dictionary<string, SavePuzzleModel>();
                return;
            }

            string json = File.ReadAllText(_filePath);
            _puzzles = JsonSerializer.Deserialize<Dictionary<string, SavePuzzleModel>>(json) ?? new();
        }
        private void SaveAll()
        {
            string json = JsonSerializer.Serialize(_puzzles, _options);
            File.WriteAllText(_filePath, json);
        }
        public string GetNameSuffix(string title)
        {
            if (!_puzzles.ContainsKey(title)) return title;
            int i = 1;
            while (_puzzles.ContainsKey(title + i))
            {
                i++;
            }
            return title + i;
        }

        public IGameBoard GetPuzzleByName(string title)
        {
            _puzzles.TryGetValue(title, out var puzzleModel);
            if (puzzleModel is null) return PuzzleCreator.CreatePuzzle(9, 11, GetNameSuffix(title), PuzzleType.Tectonic);

            IGameBoard puzzle = PuzzleCreator.CreatePuzzle(puzzleModel.X, puzzleModel.Y, puzzleModel.Values, puzzleModel.Groups, title, puzzleModel.Type);
            return puzzle;
        }

        public IEnumerable<string> GetPuzzleNames() => _puzzles.Keys;

        public void SavePuzzle(IGameBoard puzzle)
        {
            SavePuzzleModel puzzleModel = new SavePuzzleModel()
            {
                X = puzzle.X,
                Y = puzzle.Y,
                Groups = puzzle.Cells.Select((c) => c.Group).ToArray(),
                Values = puzzle.Cells.Select((c) => c.BigNumber ?? 0).ToArray(),
                Type = puzzle.PuzzleType
            };
            _puzzles[puzzle.Title ?? "test"] = puzzleModel;
            SaveAll();
        }

        public bool DeletePuzzle(string title)
        {
            bool removed = _puzzles.Remove(title);
            if (removed) SaveAll();
            return removed;
        }
    }
}
