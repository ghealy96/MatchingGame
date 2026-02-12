using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MatchingGame;

public partial class PlayPage : ContentPage
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
    private string difficulty;
    public PlayPage()
	{
        InitializeComponent();
        /*GameGrid.IsVisible = false;
        highScores.Add("easy", CreateDefaultScores());
        highScores.Add("medium", CreateDefaultScores());
        highScores.Add("hard", CreateDefaultScores());
        if (!File.Exists(filePath)) { SaveGameFile(highScores); fileExist = true; }
        if (File.Exists(filePath)) { fileExist = true; ReadGameFile(); }*/
    }


    private async void Exit_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
        
    }

    private Scores[] CreateDefaultScores()
    {
        Scores[] scores = new Scores[10];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = new Scores { Time = 9999, Name = "empty", MissedClicks = 9999 };
        }
        return scores;
    }

    /*private void Setup()
    {
        GameGrid.Children.Clear();
        GameGrid.RowDefinitions.Clear();
        GameGrid.ColumnDefinitions.Clear();

        tenthsOfSecondsElapsed = 0;

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



    }*/

    private async void PlayAgainButton_Clicked(object sender, EventArgs e)
    {

        if (sender is Button buttonClicked) {  string difficultyLower = buttonClicked.Text.ToLower();
            await Shell.Current.GoToAsync($"ClassicMode?difficulty={difficultyLower}"); }
         

        /*SelectGameMode(sender);

        Setup();
        GameGrid.IsVisible = true;
        easy.IsVisible = false;
        medium.IsVisible = false;
        hard.IsVisible = false;

        gameOver = false;

        SpeedRun.Text = "Time elapsed: " + (highScoreSpeed / 10f).ToString("0.0s"); //duplicate

        List<string> list = new List<string>()
            {
                "🦑","🦘","🐸","🦙","🦍","🐘","🦉","🐨","🐯","🦝","🦧",
                "🐎","🐮","🦔","🦇","🐼","🐧","🐋","🦭","🦈","🦞","🐞"
            };

        List<string> sortList = new List<string>();
        int gridSize = GameGrid.RowDefinitions.Count * GameGrid.ColumnDefinitions.Count;
        for (int c = 0; c < (gridSize / 2); c++)
        {
            sortList.Add(list[0]);
            sortList.Add(list[0]);
            list.RemoveAt(0);
        }

        foreach (var button in GameGrid.Children.OfType
    <Button>())
        {
            int index = Random.Shared.Next(sortList.Count);
            string nextEmoji = sortList[index];
            button.CommandParameter = nextEmoji;
            sortList.RemoveAt(index);
        }
        Dispatcher.StartTimer(TimeSpan.FromSeconds(.1), TimerTick);*/

    }
/*
    private bool TimerTick()
    {
        if (!this.IsLoaded) return false;
        if (gameOver) return false;


        tenthsOfSecondsElapsed++;
        TimeElapsed.Text = "Time elapsed: " + (tenthsOfSecondsElapsed / 10f).ToString("0.0s");

        if (easy.IsVisible)
        {
            tenthsOfSecondsElapsed = 0;
            return false;
        }
        return true;

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        if (findingMatch && lastClicked != null)
        {
            return;

        }

        if (sender is Button buttonClicked)
        {

            if ((buttonClicked.Text != null && buttonClicked.Text != "?") || string.IsNullOrWhiteSpace(buttonClicked.CommandParameter?.ToString()))
                return;


            buttonClicked.Text = buttonClicked.CommandParameter.ToString();


            if (lastClicked == null)
            {
                lastClicked = buttonClicked;
                return;
            }


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
        if (matchesFound == gridSize / 2)
        {
            gameOver = true;
            CheckHighScore();
            await DisplayAlert("Victory!", "You found them all!", "OK");
            ResetGame();
        }
    }
    private void ResetGame()
    {

        SpeedRun.Text = "Time elapsed: " + (highScoreSpeed / 10f).ToString("0.0s");
        matchesFound = 0;
        GameGrid.IsVisible = false;
        easy.IsVisible = true;
        medium.IsVisible = true;
        hard.IsVisible = true;
    }

    private void ReadGameFile()
    {

        highScores = JsonSerializer.Deserialize<Dictionary<string, Scores[]>>(File.ReadAllText(filePath));

    }

    private void SaveGameFile(Dictionary<string, Scores[]> content)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(content));
    }
    private void SelectGameMode(object sender)
    {
        string commandString = "44";
        if (sender is Button difButtonClicked)
        {
            commandString = difButtonClicked.CommandParameter.ToString();
            difficulty = difButtonClicked.Text.ToLower();
            highScoreSpeed = highScores[difficulty][0].Time;
            lowScoreSpeed = highScores[difficulty][9].Time;
        }
        rowSize = commandString[0] - '0';
        columnSize = commandString[1] - '0';
        Debug.WriteLine(columnSize);
        Debug.WriteLine(rowSize);
        gameOver = false;
    }

    private void CheckHighScore()
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
            highScores[difficulty] = InsertArray(lastTimeChecked, scores);
            SaveGameFile(highScores);
        }
    }
    private Scores[] InsertArray(int lastTimeChecked, Scores[] scores)
    {
        for (int i = scores.Length - 1; i > lastTimeChecked; i--)
        {
            scores[i] = scores[i - 1];
        }
        scores[lastTimeChecked] = new Scores { Name = "Player", Time = tenthsOfSecondsElapsed, MissedClicks = 0 };

        return scores;
    }*/
}