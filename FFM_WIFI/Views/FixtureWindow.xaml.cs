using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for FixtureWindow.xaml
    /// </summary>
    public partial class FixtureWindow : Window
    {
        private FixtureViewModel fvm;
        public FixtureWindow(TeamInfo teamInfo, PlayerInfo[] playerData)
        {
            InitializeComponent();
            fvm = new FixtureViewModel(this, teamInfo, playerData);
            this.DataContext = fvm;
        }
    }
}
