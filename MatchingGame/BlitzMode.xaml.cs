

namespace MatchingGame;

public partial class BlitzMode : ContentPage
{
    GameBoard board;
    int tenthsOfSeconds = 1200;

    public BlitzMode()
    {
        InitializeComponent();
        board = new GameBoard(4, 6);
        this.BindingContext = board; // Connects XAML to Logic

        Dispatcher.StartTimer(TimeSpan.FromSeconds(0.1), () =>
        {
            if (board.IsGameOver) return false; // Stop timer if time ran out or game won

            board.TenthsOfSecondsRemaining--;
            return true;
        });
    }

    private async void Exit_Clicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("//MainPage");
}