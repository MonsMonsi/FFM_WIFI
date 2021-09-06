using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.DataViewModel
{
    public class Info
    {
        public class Event
        {
            #region Properties
            public int? Minute { get; set; }
            public int PlayerId { get; set; }
            public string PlayerName { get; set; }
            public BitmapImage Logo { get; set; }
            public int? AssistId { get; set; }
            public string AssistName { get; set; }
            public string CommentHeader { get; set; }
            public string CommentText { get; set; }
            #endregion

            #region Attributes
            private string _type;
            private string _detail;
            #endregion

            public Event(JsonFixture.Event e)
            {
                Minute = e.Time.Elapsed;
                if (e.Player.Id != null)
                {
                    PlayerId = int.Parse(e.Player.Id.ToString());
                }
                else
                {
                    PlayerId = 0;
                }
                PlayerName = e.Player.Name;
                Logo = new GetFrom().Image(e.Team.Logo);
                AssistId = e.Assist.Id;
                AssistName = e.Assist.Name;
                _type = e.Type;
                _detail = e.Detail;
                SetProperties();
            }

            #region Methods
            private void SetProperties()
            {
                switch (_type.ToLower())
                {
                    case "goal":
                        CommentHeader = "TOR !!! TOR!!!";
                        CommentText = $"{PlayerName} Knallt die Kugel ins Kreuzeck!!!";
                        break;
                    case "subst":
                        CommentHeader = "Auswechslung!";
                        CommentText = $"{PlayerName} geht runter. Für ihn kommt {AssistName}";
                        break;
                    case "card":
                        if (_detail == "Yellow Card")
                        {
                            CommentHeader = "Gelbe Karte!";
                            CommentText = $"{PlayerName} wird verwarnt.";
                        }
                        if (_detail == "Red Card")
                        {
                            CommentHeader = "Rote Karte!";
                            CommentText = $"{PlayerName} muss vorzeitig duschen gehen!!";
                        }
                        break;
                    case "var":
                        CommentHeader = "VAR";
                        CommentText = "Die Szene wird überprüft!";
                        break;
                    default:
                        CommentHeader = "Anderes Ereignis!";
                        break;
                }
            }
            #endregion
        }

        public class Fixture
        {
            public string HomeName { get; set; }
            public BitmapImage HomeImage { get; set; }
            public string AwayName { get; set; }
            public BitmapImage AwayImage { get; set; }
            public string HalftimeResult { get; set; }
            public string EndResult { get; set; }
            public Detail.Team HomeDetail { get; set; }
            public Detail.Team AwayDetail { get; set; }
            public ObservableCollection<Event> EventList { get; set; }

            public Fixture(string homeName, BitmapImage homeImg, string awayName, BitmapImage awayImg, string halftimeresult, string endResult, Detail.Team homeDetail, Detail.Team awayDetail, ObservableCollection<Event> eventList)
            {
                HomeName = homeName;
                HomeImage = homeImg;
                AwayName = awayName;
                AwayImage = awayImg;
                HalftimeResult = halftimeresult;
                EndResult = endResult;
                HomeDetail = homeDetail;
                AwayDetail = awayDetail;
                EventList = eventList;
            }
        }

        public class Playday
        {
            public string Home { get; set; }
            public BitmapImage HomeImage { get; set; }
            public string Away { get; set; }
            public BitmapImage AwayImage { get; set; }
            public DateTime Date { get; set; }
            public string Referee { get; set; }
            public Venue Venue { get; set; }
            public string Status { get; set; }

            public Playday(string h, BitmapImage hI, string a, BitmapImage aI, DateTime date, string referee, Venue venue, string status)
            {
                Home = h;
                HomeImage = hI;
                Away = a;
                AwayImage = aI;
                Date = date;
                Referee = referee;
                Venue = venue;
                Status = status;
            }
        }

        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }
            // public int Age { get; set; }
            public string Position { get; set; }
            public string Height { get; set; }
            public string Nationality { get; set; }
            public BitmapImage Image { get; set; }
            public int Points { get; set; }
            public int LineUp { get; set; }
            public int Subst { get; set; }
            public int YellowC { get; set; }
            public int RedC { get; set; }
            public int Goals { get; set; }
            public int Assists { get; set; }
            public bool Drafted { get; set; }

            public Player(int id, string name, string position, string height, string nationality, BitmapImage image, int points, bool drafted)
            {
                Id = id;
                Name = name;
                // Age = age;
                Position = position;
                Height = height;
                Nationality = nationality;
                Image = image;
                Points = points;
                LineUp = 0;
                Subst = 0;
                YellowC = 0;
                RedC = 0;
                Goals = 0;
                Assists = 0;
                Drafted = drafted;
            }

            public Player()
            {

            }
        }

        public class Draft : IComparable
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string Position { get; set; }
            public double Value { get; set; }

            public Draft(int id, string name, string image, string position, double value)
            {
                Id = id;
                Name = name;
                Image = image;
                Position = position;
                Value = (double)value;
            }

            public int CompareTo(object obj)
            {
                Draft other = obj as Draft;
                if (other == null)
                {
                    throw new ArgumentException("No Draft!");
                }
                return this.Value.CompareTo(other.Value);
            }
        }

        public class Team
        {
            public int TeamId { get; set; }
            public int UserId { get; set; }
            public string Name { get; set; }
            public BitmapImage Logo { get; set; }
            public int Playday { get; set; }
            public int? Players { get; set; }
            public BitmapImage League { get; set; }
            public int Season { get; set; }
            public int Points { get; set; }
            public Player[] BestPlayers { get; set; }
            public UserTeam UserTeam { get; set; }
            public UserTeamPerformance Performance { get; set; }

            public Team(int teamId, int userId, string name, BitmapImage logo, int day, int? players, BitmapImage league, int season, int points,
                        Player[] bestPlayers, UserTeam userTeam, UserTeamPerformance performance)
            {
                TeamId = teamId;
                UserId = userId;
                Name = name;
                Logo = logo;
                Playday = day;
                Players = players;
                League = league;
                Season = season;
                Points = points;
                BestPlayers = bestPlayers;
                UserTeam = userTeam;
                Performance = performance;
            }

            public Team()
            {

            }
        }

        public class Result
        {
            #region Properties
            public Team Team { get; set; }
            public Player[] PlayerInfo { get; set; }
            public List<Standings> StandingsInfo { get; set; }
            #endregion

            #region Attributes
            private GetFrom.Api _get;
            private Create.Info _create;
            private JsonStandings.Root _standings;
            #endregion

            #region Constructor
            public Result(Team teamInfo, Player[] playerInfo)
            {
                Team = teamInfo;
                PlayerInfo = playerInfo;
                _get = new GetFrom.Api();
                _create = new Create.Info();
                StandingsInfo = new List<Standings>();
                GetStandings();
                SetProperties();
            }
            #endregion

            #region Methods
            private void GetStandings()
            {
                _get.LeagueId = Team.UserTeam.UserTeamLeague.ToString();
                _get.SeasonId = Team.UserTeam.UserTeamSeason.ToString();

                _standings = _get.Standings();
            }

            private void SetProperties()
            {
                foreach (var s in _standings.Response[0].League.Standings)
                {
                    foreach (var item in s)
                    {
                        StandingsInfo.Add(_create.StandingsInfo(item));
                    }
                }
            }

            //private Player GetBestPlayer(ref Player[] temp)
            //{
            //    Player player = new Player();
            //    int index = 0;
            //    player.Points = 0;

            //    for (int i = 0; i < temp.Length; i++)
            //    {
            //        if (temp[i] != null && temp[i].Points > player.Points)
            //        {
            //            player = temp[i];
            //            index = i;
            //        }
            //    }
            //    temp[index] = null;
            //    return player;
            //}
            #endregion
        }
        public class Standings
        {
            #region Properties
            public int Rank { get; set; }
            public string Name { get; set; }
            public BitmapImage Logo { get; set; }
            public int Points { get; set; }
            public int Goals { get; set; }
            public int GoalsAgainst { get; set; }
            public int GoalsDiff { get; set; }
            public int Wins { get; set; }
            public int Draws { get; set; }
            public int Loses { get; set; }
            public DateTime Update { get; set; }
            #endregion

            #region Attributes;
            #endregion

            #region Constructor
            public Standings(int rank, string name, BitmapImage logo, int points, int goals, int goalsAgainst, int goalsDiff, int wins, int draws, int loses, DateTime update)
            {
                Rank = rank;
                Name = name;
                Logo = logo;
                Points = points;
                Goals = goals;
                GoalsAgainst = goalsAgainst;
                GoalsDiff = goalsDiff;
                Wins = wins;
                Draws = draws;
                Loses = loses;
                Update = update;
            }
            #endregion

            #region Methods
            #endregion
        }
    }
}
