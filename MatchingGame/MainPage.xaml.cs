//using static Android.Print.PrintAttributes;
//using Android.Content.Res;
//using Android.Media;
//using Org.Apache.Http.Impl.Conn.Tsccm;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Runtime.CompilerServices;

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

        private async void Blitz_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("BlitzMode");
        }
    }



    
}
