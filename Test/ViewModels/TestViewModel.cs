using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Test.Commands;

namespace Test.ViewModels
{
    class TestViewModel : BaseViewModel
    {
        private string imageLink;

        private BitmapImage m_img;
        public BitmapImage Image
        {
            get { return m_img; }
            set
            {
                m_img = value;
                OnPropertyChanged("Image");
            }
        }

        private string m_name;
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        public TestViewModel()
        {
            imageLink = "https://media.api-sports.io/football/players/10000.png";
            Image = GetIcon(imageLink);
            Name = "Punkte: 300";
        }

        private BitmapImage GetIcon(string link)
        {
            WebClient client = new WebClient();
            Uri url = new Uri(link);
            BitmapImage image = new BitmapImage(url);
            return image;
        }
    }
}
