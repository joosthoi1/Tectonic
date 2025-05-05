using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tectonic.Models
{
    public class PositionedCelldata : CellData
    {
        [JsonIgnore]
        public GameBoard ParentBoard { get; set; }
        private int _group;
        public int Group {
            get => _group;
            set
            {
                _group = value;
                OnPropertyChanged(nameof(Group));
                OnPropertyChanged(nameof(GroupChanged));
                NotifyNeighborBordersChanged();
            }
        }

        private void NotifyNeighborBordersChanged()
        {
            if (ParentBoard is null) return;

            var (row, col) = ParentBoard.GetPosition(this);

            // Notify neighbors whose borders might depend on this cell
            var right = ParentBoard.GetCellAt(row, col + 1);
            var bottom = ParentBoard.GetCellAt(row + 1, col);
            var left = ParentBoard.GetCellAt(row - 1, col);
            var top = ParentBoard.GetCellAt(row, col - 1);

            right?.OnPropertyChanged(nameof(GroupChanged));
            bottom?.OnPropertyChanged(nameof(GroupChanged));
            left?.OnPropertyChanged(nameof(GroupChanged));
            top?.OnPropertyChanged(nameof(GroupChanged));
        }
        [JsonIgnore]
        public int GroupChanged => 0;
    }
}
