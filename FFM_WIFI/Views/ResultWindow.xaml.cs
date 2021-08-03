using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for DraftWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private ResultViewModel rvm;
        public ResultWindow(Info.Team teamData, Info.Player[] playerData)
        {
            InitializeComponent();
            rvm = new ResultViewModel(this, teamData, playerData);
            this.DataContext = rvm;
        }
    }
}
