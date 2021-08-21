using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Windows.Media.Imaging;
using static FFM_WIFI.Models.Libraries.Images;
using FFM_WIFI.Models.Libraries;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Threading.Tasks;

namespace FFM_WIFI.Models.Utility
{
    public class GetFrom
    {
        public class Api
        {
            #region Properties
            public string LeagueId { get; set; }
            public string SeasonId { get; set; }
            public string TeamId { get; set; }
            public string PlayerId { get; set; }
            public string FixtureId { get; set; }
            public int Count { get; set; }
            #endregion

            #region Attributes

            #endregion

            #region Constants
            const string _apiBase = "https://v3.football.api-sports.io";
            const string _apiFixture = "/fixtures?";
            const string _apiPlayers = "/players?";
            const string _apiStandings = "/standings?";
            const string _apiTeamVenue = "/teams?";
            const string _id = "id=";
            const string _page = "page=";
            const string _league = "league=";
            const string _season = "season=";
            const string _team = "team=";
            #endregion

            #region Constructor
            public Api(uint leagueId = 0, uint seasonId = 0, uint teamId = 0, uint playerId = 0, uint fixtureId = 0)
            {
                LeagueId = leagueId.ToString();
                SeasonId = seasonId.ToString();
                TeamId = teamId.ToString();
                PlayerId = playerId.ToString();
                FixtureId = fixtureId.ToString();
                Count = 0;
            }
            #endregion

            #region Methods
            public JsonFixture.Root Fixture()
            {
                if (FixtureId != "0")
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                    var fixture = JsonSerializer.Deserialize<JsonFixture.Root>(client.DownloadString($"{_apiBase}{_apiFixture}{_id}{FixtureId}"),
                                                                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Count++;
                    return fixture;
                }
                return null;
            }

            public JsonAllFixtures.Root AllFixtures()
            {
                if (LeagueId != "0" && SeasonId != "0")
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                    var allFixtures = JsonSerializer.Deserialize<JsonAllFixtures.Root>(client.DownloadString($"{_apiBase}{_apiFixture}{_league}{LeagueId}&{_season}{SeasonId}"),
                                                                                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Count++;
                    return allFixtures;
                }
                return null;
            }

            public List<JsonPlayers.Root> Players()
            {
                if (TeamId != "0" && SeasonId != "0")
                {
                    var list = new List<JsonPlayers.Root>();
                    int pages = 1;

                    WebClient client = new WebClient();
                    client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                    string response = client.DownloadString(_apiBase + _apiFixture + _id + FixtureId);

                    var players = JsonSerializer.Deserialize<JsonPlayers.Root>(client.DownloadString($"{_apiBase}{_apiPlayers}{_team}{TeamId}&{_season}{SeasonId}&{_page}{pages.ToString()}"),
                                                                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Count++;
                    list.Add(players);

                    pages = players.Paging.Total;

                    if (pages > 1)
                    {
                        for (int i = 2; i <= pages; i++)
                        {
                            players = JsonSerializer.Deserialize<JsonPlayers.Root>(client.DownloadString($"{_apiBase}{_apiPlayers}{_team}{TeamId}&{_season}{SeasonId}&{_page}{i.ToString()}"),
                                                                                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            Count++;
                            list.Add(players);
                        }
                    }
                    return list;
                }
                return null;
            }

            public JsonStandings.Root Standings()
            {
                WebClient client = new WebClient();
                client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                var standings = JsonSerializer.Deserialize<JsonStandings.Root>(client.DownloadString($"{_apiBase}{_apiStandings}{_league}{LeagueId}&{_season}{SeasonId}"),
                                                                           new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Count++;
                return standings;
            }

            public JsonTeamVenue.Root TeamVenue()
            {
                if (LeagueId != "0" && SeasonId != "0")
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                    var teamVenue = JsonSerializer.Deserialize<JsonTeamVenue.Root>(client.DownloadString($"{_apiBase}{_apiTeamVenue}{_league}{LeagueId}&{_season}{SeasonId}"),
                                                                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Count++;
                    return teamVenue;
                }
                return null;
            }
            #endregion
        }
        public class Database
        {
            #region Properties
            public User User { get; set; }
            public int TeamPoints { get; set; }
            public UserTeam UserTeam { get; set; }
            #endregion

            #region Attributes
            private Create.Info _create;
            #endregion

            #region Constants

            #endregion

            #region Constructor
            public Database(User user = null, UserTeam userTeam = null)
            {
                if (user == null)
                {
                    User = userTeam.UserTeamUserFkNavigation;
                }
                else
                {
                    User = user;
                }
                _create = new Create.Info();
                UserTeam = userTeam;
            }
            #endregion

            #region Methods
            public Info.Player[] PlayerInfo()
            {
                Info.Player[] _players = new Info.Player[17];

                using (FootballContext context = new FootballContext())
                {
                    int[] players = new int[17]
                    {
                        UserTeam.UserTeamGk1, UserTeam.UserTeamDf1, UserTeam.UserTeamDf2, UserTeam.UserTeamDf3, UserTeam.UserTeamDf4, UserTeam.UserTeamMf1,
                        UserTeam.UserTeamMf2, UserTeam.UserTeamMf3, UserTeam.UserTeamMf4, UserTeam.UserTeamAt1, UserTeam.UserTeamAt2, UserTeam.UserTeamGk2,
                        UserTeam.UserTeamDf5, UserTeam.UserTeamMf5, UserTeam.UserTeamMf6, UserTeam.UserTeamAt3, UserTeam.UserTeamAt4
                    };

                    var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == UserTeam.UserTeamPk).FirstOrDefault();
                    int[] points = new int[17]
                    {
                        performance.UserTeamPerformanceGk1, performance.UserTeamPerformanceDf1, performance.UserTeamPerformanceDf2, performance.UserTeamPerformanceDf3, performance.UserTeamPerformanceDf4, performance.UserTeamPerformanceMf1,
                        performance.UserTeamPerformanceMf2, performance.UserTeamPerformanceMf3, performance.UserTeamPerformanceMf4, performance.UserTeamPerformanceAt1, performance.UserTeamPerformanceAt2, performance.UserTeamPerformanceGk2,
                        performance.UserTeamPerformanceDf5, performance.UserTeamPerformanceMf5, performance.UserTeamPerformanceMf6, performance.UserTeamPerformanceAt3, performance.UserTeamPerformanceAt4
                    };

                    TeamPoints = 0;
                    foreach (var p in points)
                    {
                        TeamPoints += p;
                    }

                    for (int i = 0; i < 17; i++)
                    {
                        var player = context.Player.Where(p => p.PlayerPk == players[i]).FirstOrDefault();
                        Info.Player temp = _create.PlayerInfo(player, points[i]);

                        _players[i] = temp;
                    }
                }

                return _players;
            }

            public List<Info.Team> TeamInfo()
            {
                List<Info.Team> _teams = new List<Info.Team>();
                using (FootballContext context = new FootballContext())
                {
                    var userTeams = context.UserTeam.Where(t => t.UserTeamUserFk == User.UserPk).Include(t => t.UserTeamUserFkNavigation);

                    if (userTeams != null)
                    {
                        foreach (var u in userTeams)
                        {
                            var league = context.League.Where(l => l.LeaguePk == u.UserTeamLeague).FirstOrDefault();
                            var season = context.Season.Where(s => s.SeasonPk == u.UserTeamSeason).FirstOrDefault();
                            var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == u.UserTeamPk).FirstOrDefault();

                            _teams.Add(_create.TeamInfo(u, league, season, performance));
                        }
                    }
                }
                return _teams;
            }

            public List<League> LeagueList()
            {
                var list = new List<League>();

                using (FootballContext context = new FootballContext())
                {
                    var leagues = context.League;

                    foreach (var l in leagues)
                    {
                        list.Add(l);
                    }
                }

                return list;
            }

            public List<Season> SeasonList()
            {
                var list = new List<Season>();

                using (FootballContext context = new FootballContext())
                {
                    var seasons = context.Season;

                    foreach (var s in seasons)
                    {
                        list.Add(s);
                    }
                }

                return list;
            }

            public List<string> LogoList()
            {
                var list = new List<string>();

                using (FootballContext context = new FootballContext())
                {
                    var teams = context.Team;

                    foreach (var t in teams)
                    {
                        list.Add(t.TeamLogo);
                    }
                }

                return list;
            }

            public List<UserTeam> UserRank()
            {
                var _userTeams = new List<UserTeam>();

                using (FootballContext context = new FootballContext())
                {
                    var userTeams = context.UserTeam.Where(u => u.UserTeamLeague == UserTeam.UserTeamLeague && u.UserTeamSeason == UserTeam.UserTeamSeason && u.UserTeamPlayday < 35).Include(u => u.UserTeamUserFkNavigation);

                    if (userTeams != null)
                    {
                        foreach (var u in userTeams)
                        {
                            _userTeams.Add(u);
                        }

                        return _userTeams;
                    }
                }
                return null;
            }
            #endregion
        }
        public class ConfigFile
        {
            #region Properties
            public Images Images { get; set; }
            #endregion

            #region Attributes

            #endregion

            #region Constructor
            public ConfigFile()
            {
                SetProperties();
            }
            #endregion

            #region Methods
            private void SetProperties()
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = Path.Combine(docPath, @"JsonFiles\Images\Images.Json");

                var jsonObject = JsonSerializer.Deserialize<Images>(File.ReadAllText(path));

                Images = jsonObject;
            }
            #endregion
        }
        public BitmapImage Image(string path)
        {
            if (path != null)
            {
                BitmapImage bI = new BitmapImage();

                bI.BeginInit();
                bI.CacheOption = BitmapCacheOption.OnDemand;
                bI.DecodePixelHeight = 100;
                bI.DecodePixelWidth = 100;
                bI.UriSource = new Uri(path);
                bI.EndInit();

                return bI;
            }
            return null;
        }
    }
}
