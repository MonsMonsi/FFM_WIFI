using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.DataViewModel
{
    public class Detail
    {
        public class Team
        {
            public string CoachName { get; set; }
            public BitmapImage CoachImage { get; set; }
            public string Formation { get; set; }
            public int TotalShots { get; set; }
            public int ShotsOnGoal { get; set; }
            public string BallPossession { get; set; }
            public string PassAccuracy { get; set; }
            public int Fouls { get; set; }
            public int YellowCards { get; set; }
            public int RedCards { get; set; }

            public Team(string coachName, BitmapImage coachImage, string formation, object totalShots, object shotsOnGoal, string ballPossession, string passAccuracy, object fouls, object yellowCards, object redCards)
            {
                CoachName = coachName;
                CoachImage = coachImage;
                Formation = formation;

                if (totalShots != null)
                {
                    TotalShots = int.Parse(totalShots.ToString());
                }
                else
                {
                    TotalShots = 0;
                }

                if (shotsOnGoal != null)
                {
                    ShotsOnGoal = int.Parse(shotsOnGoal.ToString());
                }
                else
                {
                    ShotsOnGoal = 0;
                }

                BallPossession = ballPossession;
                PassAccuracy = passAccuracy;

                if (fouls != null)
                {
                    Fouls = int.Parse(fouls.ToString());
                }
                else
                {
                    Fouls = 0;
                }

                if (yellowCards !=null)
                {
                    YellowCards = int.Parse(yellowCards.ToString());
                }
                else
                {
                    YellowCards = 0;
                }

                if (redCards != null)
                {
                    RedCards = int.Parse(redCards.ToString());
                }
                else
                {
                    RedCards = 0;
                }
            }
        }
    }
}
