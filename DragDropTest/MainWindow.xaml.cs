using DragDropTest.ViewModels;
using System.Windows;

namespace DragDropTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mvm;
        public MainWindow()
        {
            InitializeComponent();
            mvm = new MainViewModel();
            this.DataContext = mvm;
        }

        private void Button_DragOver(object sender, DragEventArgs e)
        {

        }
    }
}
