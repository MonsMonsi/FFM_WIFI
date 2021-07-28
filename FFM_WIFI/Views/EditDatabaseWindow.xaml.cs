using FFM_WIFI.ViewModels;
using System.Windows;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for EditDatabaseWindow.xaml
    /// </summary>
    public partial class EditDatabaseWindow : Window
    {
        private EditDatabaseViewModel edvm;
        public EditDatabaseWindow()
        {
            InitializeComponent();
            edvm = new EditDatabaseViewModel();
            this.DataContext = edvm;
        }
    }
}
