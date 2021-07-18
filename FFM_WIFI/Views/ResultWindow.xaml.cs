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
    public partial class ResultWindow : Window
    {
        private ResultViewModel rvm;
        public ResultWindow()
        {
            InitializeComponent();
            rvm = new ResultViewModel();
            this.DataContext = rvm;
        }
    }
}
