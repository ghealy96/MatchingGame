
namespace MatchingGame;

public partial class ScoreBoard : ContentPage
{
    Dictionary<string, Scores[]> highScores = new Dictionary<string, Scores[]>();
    ScoreManager scoreManager;

    public ScoreBoard()
	{
		InitializeComponent();
		scoreManager = new ScoreManager();
        highScores = scoreManager.packagedScores;
        
        CreateGrid();    
	}

    private void CreateGrid()
    {
        var scoreRows = new List<ScoreRow>();

        for (int i = 0; i < 10; i++)
        {
            scoreRows.Add(new ScoreRow
            {                
                Easy = FormatScore("easy", i),
                Medium = FormatScore("medium", i),
                Hard = FormatScore("hard", i),
                Blitz=FormatScore("blitz",i)
            });
        }

        ScoreList.ItemsSource = scoreRows;
    }

    private string FormatScore(string diff, int index)
    {
        var score = highScores[diff][index];
        if (diff=="blitz"&&score.Matches>0)
        {            
            
            float matches = score.Matches;
            return $"{score.Name}: {matches} Matches";
        }
        else if (highScores.ContainsKey(diff))
        {
            // If the score is the default "9999", just show a dash
            if (score.Time >= 9999) return "-";

            // Format: "Player: 12.5s"
            float seconds = score.Time / 10f;
            return $"{score.Name}: {seconds:0.0}s";
        }
        return "-";
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
    public string Blitz { get; set; }
}