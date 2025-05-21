using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PuzzleSolver.Models;

public class CellData : INotifyPropertyChanged
{
    private int? _bigNumber;
    [JsonIgnore]
    private ObservableCollection<int> _smallNumbers;
    public int? BigNumber
    {
        get => _bigNumber;
        set
        {
            if (_bigNumber != value)
            {
                _bigNumber = value;
                OnPropertyChanged(nameof(BigNumber));
                OnPropertyChanged(nameof(HasSmallNumbers));
            }
        }
    }
    [JsonIgnore]
    public ObservableCollection<int> SmallNumbers
    {
        get => _smallNumbers;
        set
        {
            _smallNumbers = value;
            OnPropertyChanged(nameof(SmallNumbers));
            OnPropertyChanged(nameof(HasSmallNumbers));
        }
    }
    [JsonIgnore]
    public bool HasSmallNumbers => SmallNumbers != null && SmallNumbers.Count > 0 && BigNumber is null;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
