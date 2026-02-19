
namespace MatchingGame;

public partial class PlayPage : ContentPage
{

    public PlayPage()
	{
        InitializeComponent();       
    }


    private async void Exit_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");        
    } 

    private async void ClassicMode_Clicked(object sender, EventArgs e)
    {

        if (sender is Button buttonClicked) {  string difficultyLower = buttonClicked.Text.ToLower();
            await Shell.Current.GoToAsync($"ClassicMode?difficulty={difficultyLower}"); }
    }

    private async void Blitz_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("BlitzMode");
    }

}