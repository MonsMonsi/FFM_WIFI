using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Interfaces
{
    public interface ITeamDetail
    {
        string CoachName { get; set; }
        BitmapImage CoachImage { get; set; }
        string Formation { get; set; }
        int TotalShots { get; set; }
        int ShotsOnGoal { get; set; }
        string BallPossession { get; set; }
        string PassAccuracy { get; set; }
        int Fouls { get; set; }
        int YellowCards { get; set; }
        int RedCards { get; set; }
    }
}
