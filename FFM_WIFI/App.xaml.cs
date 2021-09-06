using FFM_WIFI.Models.DataJson;
using System.Windows;

namespace FFM_WIFI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Config Config { get; set; } = Config.GetConfig();
    }
}
