using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MatchingGame;

[QueryProperty(nameof(difficulty), "difficulty")]
public partial class ClassicMode : ContentPage
{

    int count = 0;
    int columnSize;
    int rowSize;


    Button lastClicked = null;
    bool findingMatch = false;
    int matchesFound;
    int tenthsOfSecondsElapsed = 0;
    int highScoreSpeed = 0;
    int lowScoreSpeed = 0;

    string filePath = "test.txt";

    Dictionary<string, Scores[]> highScores = new Dictionary<string, Scores[]>();

    bool gameOver = true;


    bool twoSelected = false;
    private bool fileExist;
    public string difficulty { get; set; }

    public ClassicMode()
	{
        InitializeComponent();
        highScores.Add("easy", CreateDefaultScores());
        highScores.Add("medium", CreateDefaultScores());
        highScores.Add("hard", CreateDefaultScores());
        if (!File.Exists(filePath)) { SaveGameFile(highScores); fileExist = true; }
        if (File.Exists(filePath)) { fileExist = true; ReadGameFile(); }
    }
    protected override void OnAppearing()
    {
        Setup();
    }

    private void Setup()
    {
        SelectGameMode();
        BuildBoard();
        CreateTileArray();

        gameOver = false;
        tenthsOfSecondsElapsed = 0;
        SpeedRun.Text = $"{highScores[difficulty][0].Name} Highscore: " + (highScoreSpeed / 10f).ToString("0.0s"); //duplicate

        
        Dispatcher.StartTimer(TimeSpan.FromSeconds(.1), TimerTick);

    }

    private void SelectGameMode()
    {

        highScoreSpeed = highScores[difficulty][0].Time;
        lowScoreSpeed = highScores[difficulty][9].Time;

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

        for (int i = 0; i < rowSize; i++) { GameGrid.RowDefinitions.Add(new RowDefinition { Height = 100 }); }

        for (int i = 0; i < columnSize; i++) { GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 }); }


        for (int row = 0; row < rowSize; row++)
        {
            for (int column = 0; column < columnSize; column++)
            {
                var button = new Button
                {
                    Text = "?",
                    FontSize = (rowSize > 4) ? 30 : 60,
                    BackgroundColor = Colors.Cyan,
                    BorderColor = Colors.Black,
                    BorderWidth = 1,
                    Margin = 2
                }
                ;
                button.Clicked += Button_Clicked;
                GameGrid.Add(button, column, row);
            }
        }
    }

    private void CreateTileArray()
    {
        List<string> list = new List<string>()
            {
                "🦑","🦘","🐸","🦙","🦍","🐘","🦉","🐨","🐯","🦝","🦧",
                "🐎","🐮","🦔","🦇","🐼","🐧","🐋","🦭","🦈","🦞","🐞"
            };

        List<string> sortList = new List<string>();
        int gridSize = GameGrid.RowDefinitions.Count * GameGrid.ColumnDefinitions.Count;
        for (int c = 0; c < (gridSize / 2); c++)
        {
            int rand= Random.Shared.Next(0, gridSize-1);
            sortList.Add(list[rand]);
            sortList.Add(list[rand]);
            list.RemoveAt(rand);
        }

        foreach (var button in GameGrid.Children.OfType<Button>())
        {
            int index = Random.Shared.Next(sortList.Count);
            string nextEmoji = sortList[index];
            button.CommandParameter = nextEmoji;
            sortList.RemoveAt(index);
        }
    }

    private async void GameOver()
    {
        gameOver = true;

        bool topScore = await CheckHighScore();

        string whatNext;
        if (topScore) { whatNext = await DisplayActionSheet("Play Again?","Main Menu", null,"Easy", "Medium", "Hard"); }
        else { whatNext = await DisplayActionSheet("Victory! you found them all, Play Again?","Main Menu", null, "Easy", "Medium", "Hard"); }

        if(whatNext.ToLower() is "easy" or "medium" or "hard") { difficulty = whatNext.ToLower(); ResetGame(); }
        else { await Shell.Current.GoToAsync("//MainPage"); }
            
    }

    //TRIGGERED THROUGHOUT GAME
    private async Task<bool> CheckHighScore() //checked at end of button clicked
    {
        if (tenthsOfSecondsElapsed < lowScoreSpeed || lowScoreSpeed == 0)
        {
            Scores[] scores = highScores[difficulty];
            int lastTimeChecked = 9;

            for (int i = scores.Length - 1; i >= 0; i--)
            {
                if (tenthsOfSecondsElapsed < scores[i].Time)//invert??
                { lastTimeChecked = i; }
                else
                { break; }
            }

            string playerName = await DisplayPromptAsync($"Congrats! you reached the #{lastTimeChecked+1} spot!", "Enter your name here: ", "Save", "Cancel");

            highScores[difficulty] = InsertArray(lastTimeChecked, scores, playerName);
            SaveGameFile(highScores);
            return true;
        }
        return false;
    }

    private void ResetGame() //checked after gameOver
    {
        gameOver = false;
        SpeedRun.Text = $"{highScores[difficulty][0].Name} Top Score: " + (highScoreSpeed / 10f).ToString("0.0s");
        matchesFound = 0;
        GameGrid.IsVisible = false;

        Setup();
    
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

        if (findingMatch && lastClicked != null){ return; }

        if (sender is Button buttonClicked)
        {
            if ((buttonClicked.Text != null && buttonClicked.Text != "?") || string.IsNullOrWhiteSpace(buttonClicked.CommandParameter?.ToString()))  return;

            buttonClicked.Text = buttonClicked.CommandParameter.ToString();

            if (lastClicked == null)
            { lastClicked = buttonClicked;
                return; }

            if (buttonClicked != lastClicked)
            {
                if (buttonClicked.Text == lastClicked.Text)
                {
                    matchesFound++;
                    MatchScored.Text = $"Matches Found: {matchesFound}";

                    buttonClicked.BackgroundColor = Colors.Green;
                    lastClicked.BackgroundColor = Colors.Green;


                    buttonClicked.CommandParameter = "";
                    lastClicked.CommandParameter = "";

                    lastClicked = null;
                }
                else
                {
                    findingMatch = true;

                    await Task.Delay(1000);

                    buttonClicked.Text = null;
                    if (lastClicked != null) lastClicked.Text = null;

                    lastClicked = null;
                    findingMatch = false;
                }
            }
        }


        int gridSize = GameGrid.RowDefinitions.Count * GameGrid.ColumnDefinitions.Count;
        
        if (matchesFound == gridSize  / 2)
            { GameOver(); }
    }

    

    private async void Exit_Clicked(object sender, EventArgs e)
    {
        gameOver = true;
        await Shell.Current.GoToAsync("//MainPage");

    }

    //SINGLE FUNCTION TOOLS

    private Scores[] CreateDefaultScores()
    {
        Scores[] scores = new Scores[10];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = new Scores { Time = 9999, Name = "empty", MissedClicks = 9999 };
        }
        return scores;
    }
   
    

    private void ReadGameFile()
    {

        highScores = JsonSerializer.Deserialize<Dictionary<string, Scores[]>>(File.ReadAllText(filePath));

    }

    private void SaveGameFile(Dictionary<string, Scores[]> content)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(content));
    }
    

    
    private Scores[] InsertArray(int lastTimeChecked, Scores[] scores,string player)
    {
        for (int i = scores.Length - 1; i > lastTimeChecked; i--)
        {
            scores[i] = scores[i - 1];
        }
        scores[lastTimeChecked] = new Scores { Name = player, Time = tenthsOfSecondsElapsed, MissedClicks = 0 };

        return scores;
    }
}