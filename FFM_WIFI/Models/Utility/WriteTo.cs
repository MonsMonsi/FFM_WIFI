using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFM_WIFI.Models.Utility
{
    public class WriteTo
    {
        public class Database
        {
            #region Properties

            #endregion

            #region Attributes
            private UserTeam _userTeam;
            private Info.Player[] _playerInfo;
            #endregion

            #region Constants

            #endregion

            #region Constructor
            public Database(UserTeam userTeam = null, Info.Player[] playerInfo = null)
            {
                _userTeam = userTeam;
                _playerInfo = playerInfo;
            }
            #endregion

            #region Methods
            public void UserTeamPerformance()
            {
                using (FootballContext context = new FootballContext())
                {
                    var userTeam = context.UserTeam.Where(u => u.UserTeamPk == _userTeam.UserTeamPk).FirstOrDefault();

                    int points = 0;

                    foreach (var p in _playerInfo)
                    {
                        points += p.Points;
                    }

                    userTeam.UserTeamPlayday = _userTeam.UserTeamPlayday;
                    userTeam.UserTeamPoints = points;

                    var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == _userTeam.UserTeamPk).FirstOrDefault();

                    performance.UserTeamPerformanceGk1 = _playerInfo[0].Points; performance.UserTeamPerformanceAt1 = _playerInfo[9].Points;
                    performance.UserTeamPerformanceDf1 = _playerInfo[1].Points; performance.UserTeamPerformanceAt2 = _playerInfo[10].Points;
                    performance.UserTeamPerformanceDf2 = _playerInfo[2].Points; performance.UserTeamPerformanceGk2 = _playerInfo[11].Points;
                    performance.UserTeamPerformanceDf3 = _playerInfo[3].Points; performance.UserTeamPerformanceDf5 = _playerInfo[12].Points;
                    performance.UserTeamPerformanceDf4 = _playerInfo[4].Points; performance.UserTeamPerformanceMf5 = _playerInfo[13].Points;
                    performance.UserTeamPerformanceMf1 = _playerInfo[5].Points; performance.UserTeamPerformanceMf6 = _playerInfo[14].Points;
                    performance.UserTeamPerformanceMf2 = _playerInfo[6].Points; performance.UserTeamPerformanceAt3 = _playerInfo[15].Points;
                    performance.UserTeamPerformanceMf3 = _playerInfo[7].Points; performance.UserTeamPerformanceAt4 = _playerInfo[16].Points;
                    performance.UserTeamPerformanceMf4 = _playerInfo[8].Points;

                    

                    context.SaveChanges();
                }
            }
            #endregion
        }
    }
}
