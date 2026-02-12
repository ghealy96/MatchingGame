namespace MatchingGame
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ScoreBoard", typeof(ScoreBoard));
            Routing.RegisterRoute("PlayPage", typeof(PlayPage));
            Routing.RegisterRoute("ClassicMode", typeof(ClassicMode));
        }
    }
}
