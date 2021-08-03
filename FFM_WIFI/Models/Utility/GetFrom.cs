using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.Utility
{
    public class GetFrom
    {
        public class DataJson
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
            const string _apiTeamVenue = "/teams?";
            const string _id = "id=";
            const string _page = "page=";
            const string _league = "league=";
            const string _season = "season=";
            const string _team = "team=";
            #endregion

            #region Constructor
            public DataJson(uint leagueId = 0, uint seasonId = 0, uint teamId = 0, uint playerId = 0, uint fixtureId = 0)
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
            public Info.Player[] Players { get; set; }
            #endregion

            #region Attributes
            private _userTeam _userTeam { get; set; }
            #endregion

            #region Constants

            #endregion

            #region Constructor
            public Database(_userTeam userTeam = null)
            {
                Players = new Info.Player[17];
                _userTeam = userTeam;
            }
            #endregion

            #region Methods
            public void PlayerInfo()
            {
                using (FootballContext context = new FootballContext())
                {
                    int[] players = new int[17]
                    {
                        DataContext._userTeam.UserTeamGk1, DataContext._userTeam.UserTeamDf1, DataContext._userTeam.UserTeamDf2, DataContext._userTeam.UserTeamDf3, DataContext._userTeam.UserTeamDf4, DataContext._userTeam.UserTeamMf1,
                        DataContext._userTeam.UserTeamMf2, DataContext._userTeam.UserTeamMf3, DataContext._userTeam.UserTeamMf4, DataContext._userTeam.UserTeamAt1, DataContext._userTeam.UserTeamAt2, DataContext._userTeam.UserTeamGk2,
                        DataContext._userTeam.UserTeamDf5, DataContext._userTeam.UserTeamMf5, DataContext._userTeam.UserTeamMf6, DataContext._userTeam.UserTeamAt3, DataContext._userTeam.UserTeamAt4
                    };

                    var performance = context.UserTeamPerformance.Where((object p) => p.UserTeamPerformanceUserTeamFk == DataContext._userTeam.UserTeamPk).FirstOrDefault();
                    int[] points = new int[17]
                    {
                    performance.UserTeamPerformanceGk1, performance.UserTeamPerformanceDf1, performance.UserTeamPerformanceDf2, performance.UserTeamPerformanceDf3, performance.UserTeamPerformanceDf4, performance.UserTeamPerformanceMf1,
                    performance.UserTeamPerformanceMf2, performance.UserTeamPerformanceMf3, performance.UserTeamPerformanceMf4, performance.UserTeamPerformanceAt1, performance.UserTeamPerformanceAt2, performance.UserTeamPerformanceGk2,
                    performance.UserTeamPerformanceDf5, performance.UserTeamPerformanceMf5, performance.UserTeamPerformanceMf6, performance.UserTeamPerformanceAt3, performance.UserTeamPerformanceAt4
                    };

                    int poi = 0;
                    foreach (var p in points)
                    {
                        poi += p;
                    }

                    _teamData.Points = poi;

                    for (int i = 0; i < 17; i++)
                    {
                        var player = context.Player.Where(p => p.PlayerPk == players[i]).FirstOrDefault();
                        Info.Player temp = new Info.Player(players[i], player.PlayerFirstName + " " + player.PlayerLastName,
                                                           player.PlayerPosition, player.PlayerHeight, player.PlayerNationality, Get.Image(player.PlayerImage), points[i], false);

                        PlayerData[i] = temp;
                    }
                }
            }
            public BitmapImage Image(string path)
        {
            Uri url = new Uri(path);
            BitmapImage image = new BitmapImage(url);
            return image;
        }
    }
}
