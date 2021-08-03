using FFM_WIFI.Models.DataContext;
using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for DraftWindow.xaml
    /// </summary>
    public partial class DraftWindow : Window
    {
        private DraftViewModel dvm;
        public DraftWindow(_userTeam userTeam = null)
        {
            InitializeComponent();
            dvm = new DraftViewModel(this, userTeam);
            this.DataContext = dvm;
        }
    }
}
