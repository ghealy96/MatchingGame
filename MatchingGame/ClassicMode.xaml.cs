

namespace MatchingGame;

[QueryProperty(nameof(difficulty), "difficulty")]
public partial class ClassicMode : ContentPage
{
    GameBoard board;

    int columnSize;
    int rowSize;

    Button lastClicked = null;
    bool failedMatch = false;
    int matchesFound;
    int tenthsOfSecondsElapsed = 0;
    int highScoreSpeed = 0;
    int lowScoreSpeed = 0;
        
    ScoreManager scoreManager;   
   
    bool gameOver = true;

    private string _difficulty;
    public string difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            
            if (difficulty != null)
            {
                Setup();
            }
        }
    }


    public ClassicMode()
	{
        InitializeComponent();
        scoreManager = new ScoreManager();
    }
   

    private void Setup()
    {
        SelectGameMode();

        board = new GameBoard(rowSize, columnSize);

        BuildBoard();        

        gameOver = false;
        tenthsOfSecondsElapsed = 0;
        SpeedRun.Text = $"{scoreManager.packagedScores[difficulty][0].Name} Highscore: " + (highScoreSpeed / 10f).ToString("0.0s");
        
        Dispatcher.StartTimer(TimeSpan.FromSeconds(.1), TimerTick);

    }

    private void SelectGameMode()
    {

        highScoreSpeed = scoreManager.packagedScores[difficulty][0].Time;
        lowScoreSpeed = scoreManager.packagedScores[difficulty][9].Time;

        switch (difficulty.ToLower())
        {
            case "easy":
                rowSize = 4;
                columnSize = 4;
                break;
            case "medium":
                rowSize = 4;
                columnSize = 5;
                break;
            case "hard":
                rowSize = 4;
                columnSize = 6;
                break;
        }
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

        Scores currentScores = new Scores(){Time=tenthsOfSecondsElapsed , ScoreType = difficulty , Matches = matchesFound};
        bool topScore = await scoreManager.CheckHighScore(currentScores,this);

        string whatNext;
        if (topScore) { whatNext = await DisplayActionSheet("Play Again?","Main Menu", null,"Easy", "Medium", "Hard"); }
        else { whatNext = await DisplayActionSheet("Victory! you found them all, Play Again?","Main Menu", null, "Easy", "Medium", "Hard"); }

        if(whatNext != null && whatNext.ToLower() is "easy" or "medium" or "hard" ) { ResetGame(); difficulty = whatNext.ToLower(); }
        else { await Shell.Current.GoToAsync("//MainPage"); }
            
    }

    private void ResetGame() //checked after gameOver
    {
        gameOver = false;
        SpeedRun.Text = $"{scoreManager.packagedScores[difficulty][0].Name} Top Score: " + (highScoreSpeed / 10f).ToString("0.0s");
        matchesFound = 0;
        GameGrid.IsVisible = false;    
    }

    private bool TimerTick() //constantly checked thoughout game
    {
        if (!this.IsLoaded) return false;
        if (gameOver) return false;


        tenthsOfSecondsElapsed++;
        TimeElapsed.Text = "Time elapsed: " + (tenthsOfSecondsElapsed / 10f).ToString("0.0s");

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
        Tile clickedTile = board.grid[row, col];

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

            int resetPoints = (board.rowSize * board.columnSize) / 2;

            if ((matchesFound % resetPoints) == 0) GameOver(); ;
        }
        else if (board.lastSelectedTile == null)
        {
            failedMatch = true;
            await Task.Delay(500);
            failedMatch = false;


            lastClicked.Text = "";
            button.Text = "";
            lastClicked = null;


        }
        else
            lastClicked = button;   
    
    }    

    private async void Exit_Clicked(object sender, EventArgs e)
    {
        gameOver = true;
        await Shell.Current.GoToAsync("//MainPage");

    }
       
}