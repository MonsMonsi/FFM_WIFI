using System;
using System.Net;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.Utility
{
    public static class Get
    {
        public static BitmapImage Image(string path)
        {
            Uri url = new Uri(path);
            BitmapImage image = new BitmapImage(url);
            return image;
        }
    }
}
