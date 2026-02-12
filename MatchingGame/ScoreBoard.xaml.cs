//using Java.Nio.FileNio;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace MatchingGame;

public partial class ScoreBoard : ContentPage
{
    Dictionary<string, Scores[]> highScores = new Dictionary<string, Scores[]>();
    string filePath = "test.txt";
    private bool fileExist = false;

    public ScoreBoard()
	{
		InitializeComponent();
		
        if (File.Exists(filePath)) { ReadGameFile(); fileExist = true; }
        if (fileExist)
        {
           // ClearGrid();
            CreateGrid();
        }
        

	}

    private void ClearGrid()
    {
       // ScoreGrid.Children.Clear();
       // ScoreGrid.RowDefinitions.Clear();
        //ScoreGrid.ColumnDefinitions.Clear();

    }

    private void CreateGrid()
    {
        var scoreRows = new List<ScoreRow>();

        for (int i = 0; i < 10; i++)
        {
            scoreRows.Add(new ScoreRow
            {
                // We combine Name and Time into a single display string
                Easy = FormatScore("easy", i),
                Medium = FormatScore("medium", i),
                Hard = FormatScore("hard", i)
            });
        }

        ScoreList.ItemsSource = scoreRows;
    }

    private string FormatScore(string diff, int index)
    {
        if (highScores.ContainsKey(diff))
        {
            var score = highScores[diff][index];

            // If the score is the default "9999", just show a dash
            if (score.Time >= 9999) return "-";

            // Format: "Player: 12.5s"
            float seconds = score.Time / 10f;
            return $"{score.Name}: {seconds:0.0}s";
        }
        return "-";
    }

    private void ReadGameFile()
    {

        highScores = JsonSerializer.Deserialize<Dictionary<string, Scores[]>>(File.ReadAllText(filePath));

    }


    private async void Exit_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}




public class ScoreRow
{
    public string Easy { get; set; }
    public string Medium { get; set; }
    public string Hard { get; set; }
}