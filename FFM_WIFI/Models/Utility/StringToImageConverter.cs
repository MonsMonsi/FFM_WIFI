using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.Utility
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is string)
            {
                value = new Uri((string)value);
            }
            if (value is Uri)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.DecodePixelWidth = 80;
                img.DecodePixelHeight = 100;
                img.UriSource = (Uri)value;
                img.EndInit();
                return img;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public StringToImageConverter()
        {

        }
    }
}
