using FFM_WIFI.Models.DataContext;
using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for UserHomeWindow.xaml
    /// </summary>
    public partial class UserHomeWindow : Window
    {
        private UserHomeViewModel uhvm;
        public UserHomeWindow(User user = null)
        {
            InitializeComponent();
            uhvm = new UserHomeViewModel(this, user);
            this.DataContext = uhvm;
        }
    }
}
