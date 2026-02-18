

namespace MatchingGame;

public partial class BlitzMode : ContentPage
{

    GameBoard board;

    Button lastClicked = null;
    bool failedMatch = false;
    int matchesFound;
    int tenthsOfSecondsElapsed = 120;

    bool gameOver = true;

    public string difficulty { get; set; }

    public BlitzMode()
    {
        InitializeComponent();
      
    }
    protected override void OnAppearing()
    {
        Setup();
    }

    private void Setup()
    {
        board = new GameBoard(4,6);

        BuildBoard();

        gameOver = false;
        tenthsOfSecondsElapsed = 1200;
        

        Dispatcher.StartTimer(TimeSpan.FromSeconds(.1), TimerTick);

    }

   

    private void BuildBoard()
    {
        GameGrid.Children.Clear();
        GameGrid.RowDefinitions.Clear();
        GameGrid.ColumnDefinitions.Clear();
        GameGrid.IsVisible = true;

        for (int i = 0; i < board.rowSize; i++) { GameGrid.RowDefinitions.Add(new RowDefinition { Height = 100 }); }

        for (int i = 0; i < board.columnSize; i++) { GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 }); }

        int fontSize = ((board.rowSize > 4) ? 30 : 60); // double??

        foreach (Tile tile in board.grid)
        {
            var button = new Button
            {
                Text = "?",
                FontSize = fontSize,
                BackgroundColor = Colors.Cyan,
                BorderColor = Colors.Black,
                BorderWidth = 1,
                Margin = 2,
                BindingContext = tile // Binding??
            };            
            button.Clicked += Button_Clicked;
            GameGrid.Add(button, tile.col, tile.row);
        }

    }

   

    private async void GameOver()
    {
        gameOver = true;

        string whatNext = await DisplayActionSheet("Victory! you found them all, Play Again?", "Main Menu", null, "Play Again"); 

        if (whatNext.ToLower() is "play again") { difficulty = whatNext.ToLower(); ResetGame(); }
        else { await Shell.Current.GoToAsync("//MainPage"); }

    }

    //TRIGGERED THROUGHOUT GAME
  

    private void ResetGame() //checked after gameOver
    {
        gameOver = false;
        
        matchesFound = 0;
        GameGrid.IsVisible = false;

        Setup();

    }

    private bool TimerTick() //constantly checked thoughout game
    {
        if (!this.IsLoaded) return false;
        if (gameOver) return false;


        tenthsOfSecondsElapsed--;
        TimeElapsed.Text = "Time elapsed: " + (tenthsOfSecondsElapsed / 10f).ToString("0.0s");
        if (tenthsOfSecondsElapsed < 0) { gameOver = true; CheckScores(); GameOver(); }

        if (gameOver)
        {
            tenthsOfSecondsElapsed = 0;
            return false;
        }
        return true;

    }


    //MAIN INTERACTION WITH TILES
    private async void Button_Clicked(object sender, EventArgs e)
    {

        var button = sender as Button;
        if (button == null || failedMatch) return;

        

        int row = GameGrid.GetRow(button);
        int col = GameGrid.GetColumn(button);
        Tile clickedTile = board.grid[row,col];

        if (clickedTile.IsMatched || clickedTile.IsFlipped) return;

        button.Text = clickedTile.Value;
        

        bool isMatch = board.ProcessSelection(clickedTile);

        

        if (isMatch)
        {
            button.BackgroundColor = Colors.Green;
            lastClicked.Background = Colors.Green;
            lastClicked = null;
            matchesFound++;

            MatchScored.Text = $"Matches Found: {matchesFound}";

            int resetPoints = (board.rowSize * board.columnSize) / 2 ;
        
            if ((matchesFound%resetPoints)==0) NewBoard();           
        }
        else if(board.lastSelectedTile==null)
        {
            failedMatch = true;
            await Task.Delay(500);
            failedMatch= false;

            
            lastClicked.Text = "";
            button.Text = "";
            lastClicked = null;


        }
        else
            lastClicked = button;
    }
    public void NewBoard()
    {
        board = new GameBoard(4, 6);
        BuildBoard();
    }



    private async void Exit_Clicked(object sender, EventArgs e)
    {
        gameOver = true;
        await Shell.Current.GoToAsync("//MainPage");

    }

    private void CheckScores()
    {

    }

    //SINGLE FUNCTION TOOLS

   
}