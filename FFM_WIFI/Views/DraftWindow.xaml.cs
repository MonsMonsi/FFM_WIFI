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
    /// Interaction logic for DraftWindow.xaml
    /// </summary>
    public partial class DraftWindow : Window
    {
        private DraftViewModel dvm;
        public DraftWindow(User user1 = null, User user2 = null, League league = null, Season season = null)
        {
            InitializeComponent();
            dvm = new DraftViewModel(this, user1, user2, league, season);
            this.DataContext = dvm;
        }
    }
}
