
using System.Text.Json;

namespace MatchingGame
{ 
    class ScoreManager
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "MatchingScores.json");
        public bool fileExist = false;
        public bool isNewRecord = false;
        public Dictionary<string, Scores[]> packagedScores { get; private set; } = new();

        public ScoreManager()
        {
            if (File.Exists(filePath)) { fileExist = true; ReadGameFile(); }
            else
            {
                packagedScores.Add("easy", CreateDefaultScores());
                packagedScores.Add("medium", CreateDefaultScores());
                packagedScores.Add("hard", CreateDefaultScores());
                packagedScores.Add("blitz", CreateDefaultScores());
                SaveGameFile(packagedScores);
                fileExist = true; 
            }
            Console.WriteLine($"SCORE FILE PATH: {filePath}");
           
        }       

        private void ReadGameFile()
        {

            packagedScores = JsonSerializer.Deserialize<Dictionary<string, Scores[]>>(File.ReadAllText(filePath));

        }

        private void SaveGameFile(Dictionary<string, Scores[]> content)
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(content));
        }

        private Scores[] CreateDefaultScores()
        {
            Scores[] scores = new Scores[10];
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = new Scores { Time = 9999, Name = "empty", MissedClicks = 9999 , Matches = 0 };
            }
            return scores;
        }

        public async Task<bool> CheckHighScore(Scores currentScore, Page currentPage) //checked at end of button clicked
        {

            Scores[] scores = packagedScores[currentScore.ScoreType];
            bool isNewRecord = false;
            bool isBlitz = false;

            if (currentScore.ScoreType == "blitz") { isNewRecord = (currentScore.Matches > scores[9].Matches); isBlitz =true; }
            else { isNewRecord = (currentScore.Time < scores[9].Time); }

            if (isNewRecord) 
            { 
                int lastTimeChecked = 9;

                for (int i = scores.Length - 1; i >= 0; i--)
                {
                    if (isBlitz)
                    {
                        if(currentScore.Matches > scores[i].Matches) { lastTimeChecked = i; }
                            else { break; }
                    }

                    else if (!isBlitz && currentScore.Time < scores[i].Time) { lastTimeChecked = i; }
                        else { break; }
                    
                }

                Scores newScore = new Scores();

                newScore.Name = await currentPage.DisplayPromptAsync($"Congrats! you reached the #{lastTimeChecked + 1} spot!", "Enter your name here: ", "Save", "Cancel");
                newScore.Place = lastTimeChecked;
                newScore.Time = currentScore.Time;
                newScore.Matches = currentScore.Matches;
                newScore.ScoreType = currentScore.ScoreType;

                packagedScores[currentScore.ScoreType] = InsertArray(scores, newScore);
                SaveGameFile(packagedScores);
                return true;
            }
            return false;
        }

        private Scores[] InsertArray(Scores[] scores, Scores newScore)
        {
            for (int i = scores.Length - 1; i > newScore.Place; i--)
            {
                scores[i] = scores[i - 1];
            }
            scores[newScore.Place] = newScore;

            return scores;
        }

    }
}
