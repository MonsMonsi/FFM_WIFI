using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataViewModel;
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
    public class Calculate
    {
        #region Properties
        public Info.Player[] PlayerInfo { get; set; }
        public ObservableCollection<Info.Playday> PlaydayList { get; set; }
        public ObservableCollection<Info.Fixture> FixtureList { get; set; }
        #endregion

        #region Attributes
        private int _playday;
        private int _iMin;
        private int _iMax;
        private List<int> _fixtures;
        private List<Info.Event> _eventList { get; set; }
        #endregion

        #region Constructor
        public Calculate(int playday, Info.Player[] teamData = null)
        {
            PlayerInfo = teamData;
            FixtureList = new ObservableCollection<Info.Fixture>();
            PlaydayList = new ObservableCollection<Info.Playday>();
            _playday = playday;
            _iMin = 0;
            _iMax = 0;
            _fixtures = new List<int>();
            _eventList = new List<Info.Event>();
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
            foreach (var p in PlayerInfo)
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

                if (PlayerInfo == null)
                {
                    SetPlaydayList(fixture);
                }
                else
                {
                    SetLineUpPoints(fixture);
                    SetEventPoints(fixture);

                    _eventList.Clear();
                    SetEventList(fixture);
                    Info.Fixture temp = new Info.Fixture(fixture.Response[0].Teams.Home.Name, new GetFrom().Image(fixture.Response[0].Teams.Home.Logo),
                                                         fixture.Response[0].Teams.Away.Name, new GetFrom().Image(fixture.Response[0].Teams.Away.Logo),
                                                         $"Endstand  {fixture.Response[0].Score.Fulltime.Home} : {fixture.Response[0].Score.Fulltime.Away}",
                                                         new ObservableCollection<Info.Event>(_eventList));
                    FixtureList.Add(temp);
                }
            }
        }

        private void SetLineUpPoints(JsonFixture.Root fixture)
        {
            Info.Player[] temp = PlayerInfo;
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
            PlayerInfo = temp;
        }

        private void SetEventPoints(JsonFixture.Root fixture)
        {
            Info.Player[] temp = PlayerInfo;
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
                                p.Goals += 16;
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
                                p.Subst -= 2;
                            }

                        }
                        break;
                    case "Card":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.Player.Id && e.Detail == "Yellow Card" && p.Drafted)
                            {
                                p.Points -= 1;
                                p.YellowC -= 1;
                            }
                            if (p.Id == e.Player.Id && e.Detail == "Red Card" && p.Drafted)
                            {
                                p.Points -= 4;
                                p.RedC -= 4;
                            }
                        }
                        break;
                }
                PlayerInfo = temp;
            }
        }

        private void SetEventList(JsonFixture.Root fixture)
        {
            Info.Event temp;
            foreach (var e in fixture.Response[0].Events)
            {
                switch (e.Type)
                {
                    case "Goal":
                        temp = new Info.Event(e);
                        break;
                    case "subst":
                        temp = new Info.Event(e);
                        break;
                    case "Card":
                        temp = new Info.Event(e);
                        break;
                    default:
                        temp = new Info.Event(e);
                        break;
                }
                _eventList.Add(temp);
              }
        }

        private void SetPlaydayList(JsonFixture.Root fixture)
        {
            using (FootballContext context = new FootballContext())
            {
                var venue = context.Venue.Where(v => v.VenuePk == fixture.Response[0].Fixture.Venue.Id).FirstOrDefault();

                PlaydayList.Add(new Info.Playday(fixture.Response[0].Teams.Home.Name, new GetFrom().Image(fixture.Response[0].Teams.Home.Logo),
                                                 fixture.Response[0].Teams.Away.Name, new GetFrom().Image(fixture.Response[0].Teams.Away.Logo),
                                                 fixture.Response[0].Fixture.Date, fixture.Response[0].Fixture.Referee, venue));
            }
        }
        #endregion
    }
}
