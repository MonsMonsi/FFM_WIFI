using FFM_WIFI.Models.DataContext;
using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for NewTeamWindow.xaml
    /// </summary>
    public partial class NewTeamWindow : Window
    {
        private NewTeamViewModel ntvm;
        public NewTeamWindow(User user = null)
        {
            InitializeComponent();
            ntvm = new NewTeamViewModel(this, user);
            this.DataContext = ntvm;
        }
    }
}
