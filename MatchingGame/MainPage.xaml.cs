
namespace MatchingGame
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
           

        }

       

        private async void PlayButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("PlayPage");
        }


        private async void ScoreBoard_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ScoreBoard");
        }

        private async void Exit_Clicked(object sender, EventArgs e)
        {
            Microsoft.Maui.Controls.Application.Current.Quit();
        }

       
    }



    
}
