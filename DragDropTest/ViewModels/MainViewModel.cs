using FFM_WIFI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DragDropTest.ViewModels
{
    public class Player
    {
        public string Image { get; set; }
    }

    class MainViewModel : BaseViewModel
    {
        private Player _player;
        public Player Player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged("Player");
            }
        }

        public MainViewModel()
        {
            _player = new Player();
            _player.Image = "http://bilder.bild.de/fotos/finanztest-macht-immobilien-check-wann-und-wo-sich-kaufen-lohnt-28916741-31379484/Bild/3.bild.jpg";
        }


        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(image,
                                     image.Source.ToString(),
                                     DragDropEffects.Copy);
            }
        }

        //private string _imageSource;
        //private void button_DragEnter(object sender, DragEventArgs e)
        //{
        //    Button button = sender as Button;
        //    if (button != null)
        //    {
        //        // Save the current Fill brush so that you can revert back to this value in DragLeave.
        //        _previousFill = button.Fill;

        //        // If the DataObject contains string data, extract it.
        //        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        //        {
        //            string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

        //            // If the string can be converted into a Brush, convert it.
        //            BrushConverter converter = new BrushConverter();
        //            if (converter.IsValid(dataString))
        //            {
        //                Brush newFill = (Brush)converter.ConvertFromString(dataString);
        //                button.Fill = newFill;
        //            }
        //        }
        //    }
        //}
    }
}
