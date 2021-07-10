using FFM_WIFI.Models.DataContext;
using FFM_WIFI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for FixtureWindow.xaml
    /// </summary>
    public partial class FixtureWindow : Window
    {
        private FixtureViewModel fvm;
        public FixtureWindow(UserTeam userTeam, int[] fixtures, PlayerInfo[] userTeamData)
        {
            InitializeComponent();
            fvm = new FixtureViewModel(this, userTeam, fixtures, userTeamData);
            this.DataContext = fvm;
        }
    }
}
