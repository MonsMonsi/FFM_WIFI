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
            public int Minute { get; set; }
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
                PlayerId = e.Player.Id;
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
                switch (_type)
                {
                    case "Goal":
                        CommentHeader = "TOR !!! TOR!!!";
                        CommentText = $"{PlayerName} Knallt die Kugel ins Kreuzeck!!!";
                        break;
                    case "subst":
                        CommentHeader = "Auswechslung!";
                        CommentText = $"{PlayerName} geht runter. Für ihn kommt {AssistName}";
                        break;
                    case "Card":
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
            public ObservableCollection<Event> List { get; set; }

            public Fixture(string hn, BitmapImage hImg, string an, BitmapImage aImg, ObservableCollection<Event> list)
            {
                HomeName = hn;
                HomeImage = hImg;
                AwayName = an;
                AwayImage = aImg;
                List = list;
            }
        }

        public class Playday
        {
            public string Home { get; set; }
            public BitmapImage HomeImage { get; set; }
            public string Away { get; set; }
            public BitmapImage AwayImage { get; set; }
            public string Date { get; set; }
            public string Referee { get; set; }
            public Venue Venue { get; set; }

            public Playday(string h, BitmapImage hI, string a, BitmapImage aI, DateTime date, string referee, Venue venue)
            {
                Home = h;
                HomeImage = hI;
                Away = a;
                AwayImage = aI;
                Date = date.ToShortDateString();
                Referee = referee;
                Venue = venue;
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
            public string Season { get; set; }
            public int Points { get; set; }
            public _userTeam UserTeam { get; set; }
            public UserTeamPerformance Performance { get; set; }
            public Images Images { get; set; }

            public Team(int teamId, int userId, string name, BitmapImage logo, int day, int? players, BitmapImage league, string season, int points, _userTeam userTeam, UserTeamPerformance performance, Images images)
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
                UserTeam = userTeam;
                Performance = performance;
                Images = images;
            }

            public Team()
            {

            }
        }

        public class Result
        {
            public Team Team { get; set; }
            public Player[] Players { get; set; }
            public Player Player1 { get; set; }
            public Player Player2 { get; set; }
            public Player Player3 { get; set; }

            public Result(Team team, Player[] players)
            {
                Team = team;
                Players = players;
                Player1 = GetBestPlayer(ref players);
                Player2 = GetBestPlayer(ref players);
                Player3 = GetBestPlayer(ref players);
            }

            private Player GetBestPlayer(ref Player[] temp)
            {
                Player player = new Player();
                int index = 0;
                player.Points = 0;

                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] != null && temp[i].Points > player.Points)
                    {
                        player = temp[i];
                        index = i;
                    }
                }
                temp[index] = null;
                return player;
            }
        }
    }
}
