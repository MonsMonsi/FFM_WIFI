using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private StartViewModel swvm;
        public StartWindow()
        {
            InitializeComponent();
            swvm = new StartViewModel(this);
            this.DataContext = swvm;
        }
    }
}
