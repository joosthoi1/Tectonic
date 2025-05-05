using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tectonic.Models
{
    class SavePuzzleModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[] Groups { get; set; }
        public int[] Values { get; set; }
    }
}
