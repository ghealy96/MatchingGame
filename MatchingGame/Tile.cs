using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatchingGame;

public class Tile : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Value { get; set; }

    private bool _isFlipped;
    public bool IsFlipped
    {
        get => _isFlipped;
        set { _isFlipped = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayValue)); }
    }

    private bool _isMatched;
    public bool IsMatched
    {
        get => _isMatched;
        set { _isMatched = value; OnPropertyChanged(); OnPropertyChanged(nameof(StatusColor)); OnPropertyChanged(nameof(DisplayValue)); }
    }

    public string DisplayValue => (IsFlipped || IsMatched) ? Value : "?";
    public Color StatusColor => IsMatched ? Colors.Green : Colors.Cyan;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}