using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MatchingGame;

public class GameBoard : INotifyPropertyChanged
{
    // ObservableCollection tells the UI to add/remove buttons automatically
    public ObservableCollection<Tile> Tiles { get; set; } = new();

    public int columnSize;
    public bool IsGameOver = false;

    // Inside GameBoard.cs
    public int BoardWidth => columnSize * 84; // 80px button + 4px margin

    private int _matchesFound;

    
    public int MatchesFound
    {
        get => _matchesFound;
        set { _matchesFound = value; OnPropertyChanged(); }
    }

    
    private int _tenthsOfSecondsRemaining = 1200; // Start with 30 seconds
    public int TenthsOfSecondsRemaining
    {
        get => _tenthsOfSecondsRemaining;
        set
        {
            _tenthsOfSecondsRemaining = value;

            // Update the string the UI sees
            ElapsedTimeDisplay = $"Time Left: {(_tenthsOfSecondsRemaining / 10f):0.0}s";

            if (_tenthsOfSecondsRemaining <= 0)
            {
                _tenthsOfSecondsRemaining = 0;
                IsGameOver = true;
                // Trigger GameOver logic here
            }
            OnPropertyChanged();
        }
    }

    public string TimerColor { get; set { if (TenthsOfSecondsRemaining < 300) TimerColor = "#084CFF"; else TimerColor = "#FA1515"; } }

    public void AddBonusTime(int seconds)
    {
        TenthsOfSecondsRemaining += (seconds * 20);
    }

    private string _elapsedTimeDisplay = "Time elapsed: 120.0s";
    public string ElapsedTimeDisplay
    {
        get => _elapsedTimeDisplay;
        set 
        {

            _elapsedTimeDisplay = value; 
            OnPropertyChanged(); 
        }
        
    }

    private bool _isProcessing = false; // Prevents clicking 3 tiles at once
    public Tile LastSelectedTile { get; private set; }

    public ICommand SelectTileCommand { get; }

    public GameBoard(int rows, int cols)
    {
        columnSize = cols;
        SelectTileCommand = new Command<Tile>(async (tile) => await ExecuteSelectTile(tile));
        GenerateTiles(rows, cols);
    }

    private void GenerateTiles(int rows, int cols)
    {
        CharacterList chart = new CharacterList();
        string[] characters = chart.setCharacters(rows * cols);

        for (int i = 0; i < rows * cols; i++)
        {
            Tiles.Add(new Tile { Id = i, Value = characters[i] });
        }
    }

    private async Task ExecuteSelectTile(Tile clickedTile)
    {
        if (_isProcessing || clickedTile.IsMatched || clickedTile.IsFlipped) return;

        clickedTile.IsFlipped = true;

        if (LastSelectedTile == null)
        {
            LastSelectedTile = clickedTile;
        }
        else
        {
            if (LastSelectedTile.Value == clickedTile.Value)
            {
                clickedTile.IsMatched = true;
                LastSelectedTile.IsMatched = true;
                MatchesFound++;

                // REWARD: Add 5 seconds for a correct match!
                AddBonusTime(5);

                LastSelectedTile = null;
            }
            else
            {
                _isProcessing = true;
                await Task.Delay(500); // The user can see the wrong match
                clickedTile.IsFlipped = false;
                LastSelectedTile.IsFlipped = false;
                LastSelectedTile = null;
                _isProcessing = false;
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}