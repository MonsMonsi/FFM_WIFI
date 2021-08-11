using FFM_WIFI.Models.DataJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFM_WIFI.Models.DataViewModel;
using System.Collections.ObjectModel;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.Libraries;
using static FFM_WIFI.Models.Utility.GetFrom;

namespace FFM_WIFI.Models.Utility
{
    public class Create
    {
        public class Info
        {
            #region Properties
            
            #endregion

            #region Attributes
            private GetFrom _get;
            private List<DataViewModel.Info.Event> _eventList;
            private Images _images = new ConfigFile().Images;
            #endregion

            #region Constructor
            public Info()
            {
                _get = new GetFrom();
                _eventList = new List<DataViewModel.Info.Event>();
            }
            #endregion

            #region Methods
            public DataViewModel.Info.Team TeamInfo(UserTeam userTeam, League league, Season season, UserTeamPerformance performance, int points)
            {
                DataViewModel.Info.Team temp;
                DataViewModel.Info.Player[] players = new Database(null, userTeam).PlayerInfo();

                if (userTeam.UserTeamPlayday > 34)
                {
                    temp = new DataViewModel.Info.Team(userTeam.UserTeamPk, userTeam.UserTeamUserFkNavigation.UserPk, userTeam.UserTeamName, new GetFrom().Image(_images.Star),
                                                       userTeam.UserTeamPlayday, userTeam.UserTeamNumberPlayers, new GetFrom().Image(league.LeagueLogo), season.SeasonPk,
                                                       points, GetBestPlayers(players), userTeam, performance);
                    return temp;
                }
                temp = new DataViewModel.Info.Team(userTeam.UserTeamPk, userTeam.UserTeamUserFkNavigation.UserPk, userTeam.UserTeamName, new GetFrom().Image(userTeam.UserTeamLogo),
                                                   userTeam.UserTeamPlayday, userTeam.UserTeamNumberPlayers, new GetFrom().Image(league.LeagueLogo), season.SeasonPk,
                                                   points, GetBestPlayers(players), userTeam, performance);
                return temp;
            }

            public DataViewModel.Info.Player[] GetBestPlayers (DataViewModel.Info.Player[] temp)
            {
                var allPlayers = temp;
                var best = new DataViewModel.Info.Player[3];
                int points = 20000000;

                if (allPlayers[0] != null)
                {
                    for (int index = 0; index < 3; index++)
                    {
                        var player = new DataViewModel.Info.Player();
                        for (int i = 0; i < allPlayers.Length; i++)
                        {
                            if (allPlayers[i].Points > player.Points && allPlayers[i].Points < points)
                            {
                                player = temp[i];
                            }
                        }
                        best[index] = player;
                        points = player.Points;
                    }

                    return best;
                }
                return null;
            }

            public Detail.Team TeamDetail(JsonFixture.Response fixture, string mode)
            {
                Detail.Team temp;
                if (mode == "home")
                {
                    temp = new Detail.Team(fixture.Lineups[0].Coach.Name, _get.Image(fixture.Lineups[0].Coach.Photo), fixture.Lineups[0].Formation,
                                                       fixture.Statistics[0].Statistics[2].Value, fixture.Statistics[0].Statistics[0].Value,
                                                       fixture.Statistics[0].Statistics[9].Value.ToString(), fixture.Statistics[0].Statistics[15].Value.ToString(),
                                                       fixture.Statistics[0].Statistics[6].Value, fixture.Statistics[0].Statistics[10].Value,
                                                       fixture.Statistics[0].Statistics[11].Value);
                    return temp;
                }
                temp = new Detail.Team(fixture.Lineups[1].Coach.Name, _get.Image(fixture.Lineups[1].Coach.Photo), fixture.Lineups[1].Formation,
                                                   fixture.Statistics[1].Statistics[2].Value, fixture.Statistics[1].Statistics[1].Value,
                                                   fixture.Statistics[1].Statistics[9].Value.ToString(), fixture.Statistics[1].Statistics[15].Value.ToString(),
                                                   fixture.Statistics[1].Statistics[6].Value.ToString(), fixture.Statistics[1].Statistics[10].Value,
                                                   fixture.Statistics[1].Statistics[11].Value);
                return temp;
            }

            public DataViewModel.Info.Fixture FixtureInfo(JsonFixture.Response fixture)
            {
                _eventList.Clear();
                SetEventList(fixture);

                var temp = new DataViewModel.Info.Fixture(fixture.Teams.Home.Name, _get.Image(fixture.Teams.Home.Logo),
                                                          fixture.Teams.Away.Name, _get.Image(fixture.Teams.Away.Logo),
                                                          $"{fixture.Score.Halftime.Home} : {fixture.Score.Halftime.Away}",
                                                          $"{fixture.Score.Fulltime.Home} : {fixture.Score.Fulltime.Away}", TeamDetail(fixture, "home"), TeamDetail(fixture, ""), new ObservableCollection<DataViewModel.Info.Event>(_eventList));
                return temp;
            }

            public DataViewModel.Info.Player PlayerInfo(Player player, int points)
            {
                DataViewModel.Info.Player temp;
                if (player != null)
                {
                    temp = new DataViewModel.Info.Player(player.PlayerPk, player.PlayerFirstName + " " + player.PlayerLastName,
                                                             player.PlayerPosition, player.PlayerHeight, player.PlayerNationality, new GetFrom().Image(player.PlayerImage), points, false);
                    return temp;
                }
                return null;
            }

            public DataViewModel.Info.Playday PlaydayInfo(JsonFixture.Response fixture, Venue venue)
            {
                var temp = new DataViewModel.Info.Playday(fixture.Teams.Home.Name, new GetFrom().Image(fixture.Teams.Home.Logo),
                                                          fixture.Teams.Away.Name, new GetFrom().Image(fixture.Teams.Away.Logo),
                                                          fixture.Fixture.Date, fixture.Fixture.Referee, venue, fixture.Fixture.Status.Long);
                return temp;
            }

            public DataViewModel.Info.Standings StandingsInfo(JsonStandings.Standing standings)
            {
                var temp = new DataViewModel.Info.Standings(standings.Rank, standings.Team.Name, _get.Image(standings.Team.Logo),
                                                            standings.Points, standings.All.Goals.For, standings.All.Goals.Against,
                                                            standings.All.Goals.For - standings.All.Goals.Against,
                                                            standings.All.Win, standings.All.Draw, standings.All.Lose, standings.Update);
                return temp;
            }

            private void SetEventList(JsonFixture.Response fixture)
            {
                DataViewModel.Info.Event temp;
                foreach (var e in fixture.Events)
                {
                    switch (e.Type)
                    {
                        case "Goal":
                            temp = new DataViewModel.Info.Event(e);
                            break;
                        case "subst":
                            temp = new DataViewModel.Info.Event(e);
                            break;
                        case "Card":
                            temp = new DataViewModel.Info.Event(e);
                            break;
                        default:
                            temp = new DataViewModel.Info.Event(e);
                            break;
                    }
                    _eventList.Add(temp);
                }
            }
            #endregion
        }
    }
}
