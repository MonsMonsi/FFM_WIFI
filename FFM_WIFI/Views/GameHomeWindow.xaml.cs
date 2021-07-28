using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameHomeWindow : Window
    {
        private GameHomeViewModel ghvm;
        public GameHomeWindow(TeamInfo teamData = null, PlayerInfo[] userTeamData = null)
        {
            InitializeComponent();
            ghvm = new GameHomeViewModel(this, teamData, userTeamData);
            this.DataContext = ghvm;
        }
    }
}
