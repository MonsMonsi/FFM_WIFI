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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FFM_WIFI.ViewModels;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private StartViewModel mwvm;
        public StartWindow()
        {
            InitializeComponent();
            mwvm = new StartViewModel();
            this.DataContext = mwvm;
        }
    }
}
