using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.Utility
{
    #region InfoClasses
    public class FixtureInfo
    {
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public ObservableCollection<string> List { get; set; }

        public FixtureInfo(string hn, string an, ObservableCollection<string> list)
        {
            HomeName = hn;
            AwayName = an;
            List = list;
        }
    }

    public class PlaydayInfo
    {
        public string Home { get; set; }
        public BitmapImage HomeImage { get; set; }
        public string Away { get; set; }
        public BitmapImage AwayImage { get; set; }
        public string Date { get; set; }
        public string Referee { get; set; }
        public Venue Venue { get; set; }

        public PlaydayInfo(string h, BitmapImage hI, string a, BitmapImage aI, DateTime date, string referee, Venue venue)
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
    #endregion
    public class Calculate
    {
        #region Properties
        public PlayerInfo[] TeamData { get; set; }
        public ObservableCollection<PlaydayInfo> PlaydayList { get; set; }
        public ObservableCollection<FixtureInfo> InfoList { get; set; }
        #endregion

        #region Attributes
        private int _playday;
        private int _iMin;
        private int _iMax;
        private List<int> _fixtures;
        private List<string> _listViewText { get; set; }
        #endregion

        #region Constructor
        public Calculate(int playday, PlayerInfo[] teamData = null)
        {
            TeamData = teamData;
            InfoList = new ObservableCollection<FixtureInfo>();
            PlaydayList = new ObservableCollection<PlaydayInfo>();
            _playday = playday;
            _iMin = 0;
            _iMax = 0;
            _fixtures = new List<int>();
            _listViewText = new List<string>();
        }
        #endregion

        #region Methods
        public void TeamPoints()
        {
            ResetPlayerData();
            SetIndexes();
            GetFixtures();
            SetProperties();
        }

        public void PlaydayInfo()
        {
            SetIndexes();
            GetFixtures();
            SetProperties();
        }

        private void ResetPlayerData()
        {
            // Werte auf 0 setzen
            foreach (var p in TeamData)
            {
                p.LineUp = 0;
                p.Subst = 0;
                p.YellowC = 0;
                p.RedC = 0;
                p.Goals = 0;
                p.Assists = 0;
            }
        }

        private void SetIndexes()
        {
            if (_playday == 1)
            {
                _iMin = 0;
                _iMax = _iMin + 9;
            }
            else
            {
                _iMin = (_playday - 1) * 8 + (_playday - 1);
                _iMax = _iMin + 9;
            }
        }

        private void GetFixtures()
        {
            // _fixtures.Clear();

            string filePath = @"D:\VS_Projects\FFM_WIFI\JsonFiles\AllFixturesBULI2020\AllFixturesBULI2020.json";
            StreamReader reader = File.OpenText(filePath);
            var fixtures = JsonSerializer.Deserialize<JsonAllFixtures.Root>(reader.ReadToEnd(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            for (int i = _iMin; i < _iMax; i++)
            {
                _fixtures.Add(fixtures.Response[i].Fixture.Id);
            }
        }

        private void SetProperties()
        {
            foreach (var id in _fixtures)
            {
                string filePath = @$"D:\VS_Projects\FFM_WIFI\JsonFiles\FixtureDetailsBULI2020\BULI2020_Fixture{id}.json";
                StreamReader reader = File.OpenText(filePath);
                var fixture = JsonSerializer.Deserialize<JsonFixture.Root>(reader.ReadToEnd(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (TeamData == null)
                {
                    SetPlaydayList(fixture);
                }
                else
                {
                    SetLineUpPoints(fixture);
                    SetEventPoints(fixture);

                    _listViewText.Clear();
                    SetListViewText(fixture);
                    FixtureInfo temp = new FixtureInfo(fixture.Response[0].Teams.Home.Name, fixture.Response[0].Teams.Away.Name, new ObservableCollection<string>(_listViewText));
                    InfoList.Add(temp);
                }
            }
        }

        private void SetLineUpPoints(JsonFixture.Root fixture)
        {
            PlayerInfo[] temp = TeamData;
            foreach (var s in fixture.Response[0].Lineups[0].StartXI)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.Player.Id && p.Drafted)
                    {
                        p.Points += 6;
                        p.LineUp += 6;
                    }
                }
            }

            foreach (var s in fixture.Response[0].Lineups[1].StartXI)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.Player.Id && p.Drafted)
                    {
                        p.Points += 6;
                        p.LineUp += 6;
                    }
                }

            }

            foreach (var s in fixture.Response[0].Lineups[0].Substitutes)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.Player.Id && p.Drafted)
                    {
                        p.Points += 2;
                        p.LineUp += 2;
                    }
                }

            }

            foreach (var s in fixture.Response[0].Lineups[1].Substitutes)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.Player.Id && p.Drafted)
                    {
                        p.Points += 2;
                        p.LineUp += 2;
                    }
                }

            }
            TeamData = temp;
        }

        private void SetEventPoints(JsonFixture.Root fixture)
        {
            PlayerInfo[] temp = TeamData;
            foreach (var e in fixture.Response[0].Events)
            {
                switch (e.Type)
                {
                    case "Goal":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.Player.Id && p.Drafted)
                            {
                                p.Points += 16;
                                p.Goals++;
                            }
                            if (p.Id == e.Assist.Id && p.Drafted)
                            {
                                p.Points += 8;
                                p.Assists++;
                            }
                        }
                        break;
                    case "subst":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.Player.Id && e.Time.Elapsed < 45 && p.Drafted)
                            {
                                p.Points -= 2;
                                p.Subst++;
                            }

                        }
                        break;
                    case "Card":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.Player.Id && e.Detail == "Yellow Card" && p.Drafted)
                            {
                                p.Points -= 1;
                                p.YellowC++;
                            }
                            if (p.Id == e.Player.Id && e.Detail == "Red Card" && p.Drafted)
                            {
                                p.Points -= 4;
                                p.RedC++;
                            }
                        }
                        break;
                }
                TeamData = temp;
            }
        }

        private void SetListViewText(JsonFixture.Root fixture)
        {
            foreach (var e in fixture.Response[0].Events)
            {
                string eventType1 = "";
                string eventType2 = "";

                switch (e.Type)
                {
                    case "Goal":
                        eventType1 = $"Torschütze: {e.Player.Name}";
                        eventType2 = $" - Assist: {e.Assist.Name}";
                        break;
                    case "subst":
                        eventType1 = $"Aus: {e.Player.Name}";
                        eventType2 = $" - Ein: {e.Assist.Name}";
                        break;
                    case "Card":
                        eventType1 = $"Karte für: {e.Player.Name}";
                        break;
                }

                string newEvent = $"Minute: {e.Time.Elapsed} - Team: {e.Team.Name}\n" +
                                  $"{e.Type}!\n" +
                                  $"{eventType1}{eventType2}\n" +
                                  $"----------";
                _listViewText.Add(newEvent);
            }
        }

        private void SetPlaydayList(JsonFixture.Root fixture)
        {
            using (FootballContext context = new FootballContext())
            {
                var venue = context.Venue.Where(v => v.VenuePk == fixture.Response[0].Fixture.Venue.Id).FirstOrDefault();

                PlaydayList.Add(new PlaydayInfo(fixture.Response[0].Teams.Home.Name, Get.Image(fixture.Response[0].Teams.Home.Logo),
                                                fixture.Response[0].Teams.Away.Name, Get.Image(fixture.Response[0].Teams.Away.Logo),
                                                fixture.Response[0].Fixture.Date, fixture.Response[0].Fixture.Referee, venue));
            }
        }
        #endregion
    }
}
