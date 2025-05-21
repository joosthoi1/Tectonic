using PuzzleSolver.Models.Puzzles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver.Models
{
    class SavePuzzleModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[] Groups { get; set; }
        public int[] Values { get; set; }
        public PuzzleType Type { get; set; }
    }
}
