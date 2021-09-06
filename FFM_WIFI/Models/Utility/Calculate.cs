using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.Models.Utility
{
    public class Calculate
    {
        // Calculate-Klasse:
        // -> berechnet zum einen die Spieltagsinformationen
        // -> zum anderen die Punkte, die jedes team pro Spieltag erspielt

        #region Properties
        public Info.Player[] PlayerInfo { get; set; }
        public ObservableCollection<Info.Playday> PlaydayList { get; set; }
        public ObservableCollection<Info.Fixture> FixtureList { get; set; }
        #endregion

        #region Attributes
        private int _playday;
        private int _league;
        private int _season;
        private int _iMin;
        private int _iMax;
        private GetFrom _get;
        private Create.Info _create;
        private List<int> _fixtures;
        #endregion

        #region Constructor
        public Calculate(int playday, int league, int season, Info.Player[] playerInfo = null)
        {
            PlayerInfo = playerInfo;
            FixtureList = new ObservableCollection<Info.Fixture>();
            PlaydayList = new ObservableCollection<Info.Playday>();
            _get = new GetFrom();
            _create = new Create.Info();
            _playday = playday;
            _league = league;
            _season = season;
            _iMin = 0;
            _iMax = 0;
            _fixtures = new List<int>();
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
            // Ligen haben verschiedene Größen, daher unterschiedliche Anzahl Spiele
            int games = _league switch
            {
                78 => 9,
                61 or 39 => 10,
                _ => 0
            };

            // Die Spieltag-Infos werden von der APi als Array zurückgesendet
            // Die hier errechneten Indexes, entsprechen der jeweiligen Stelle im Array
            if (_playday == 1)
            {
                _iMin = 0;
                _iMax = games;
            }
            else
            {
                _iMin = (_playday - 1) * games;
                _iMax = _iMin + games;
            }
        }

        private void GetFixtures() 
        {
            // Am Beginn einer neuen Saison stehen in der API die Daten für alle Spieltag bereit
            // Daher wird der gesamte Spielplan am Anfang der Saison abgespeichert und die jeweiligen Daten können von der Datei geholt werden
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(docPath, @$"JsonFiles\AllFixtures\AllFixtures_L{_league}S{_season}.json");

            if (File.Exists(path))
            {
                StreamReader reader = File.OpenText(path);
                var fixtures = JsonSerializer.Deserialize<JsonAllFixtures.Root>(reader.ReadToEnd(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                for (int i = _iMin; i < _iMax; i++)
                {
                    _fixtures.Add(fixtures.Response[i].Fixture.Id);
                }
            }
        }

        private void SetProperties()
        {
            // Die Spieltag-Details stehen nur in der API, wenn der Spieltag bereits abgeschlossen ist
            // Daher können nur abgeschlossene Spieltag in ein File geschrieben werden
            // Handelt es sich um einen neuen Spieltag, muss die Api abgefragt werden, ob bereits Daten vorhanden sind
            foreach (var id in _fixtures)
            {
                JsonFixture.Response fixture;

                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = Path.Combine(docPath, @$"JsonFiles\Fixture\Fixture_L{_league}S{_season}I{id}.json");

                bool exists = File.Exists(path);
                // wenn ein File existiert, soll er es lesen
                if (File.Exists(path))
                {
                    fixture = JsonSerializer.Deserialize<JsonFixture.Response>(File.ReadAllText(path));
                }
                // wenn kein File existiert, ruft er die API
                else
                {
                    WebClient client = new ();
                    client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                    fixture = JsonSerializer.Deserialize<JsonFixture.Root>(client.DownloadString($"https://v3.football.api-sports.io/fixtures?id={id}"),
                                                                                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Response[0];

                    // wenn das Spiel schon vorbei ist, schreibt er ein File
                    if (fixture.Fixture.Status.Long == "Match Finished")
                    {
                        path = Path.Combine(docPath, @$"JsonFiles\Fixture\Fixture_L{_league}S{_season}I{id}.json");

                        File.WriteAllText(path, JsonSerializer.Serialize(fixture));
                    }
                }

                if (PlayerInfo == null)
                {
                    SetPlaydayList(fixture);
                }
                else
                {
                    SetLineUpPoints(fixture);
                    SetEventPoints(fixture);

                    Info.Fixture temp = _create.FixtureInfo(fixture);

                    FixtureList.Add(temp);
                }
            }
        }

        private void SetLineUpPoints(JsonFixture.Response fixture)
        {
            // Aus den geladenen Spieltag-Details werden nun die für die Wertung wichtigen Daten herausgeholt
            // LineUp-Points erhält ein Spieler wenn er entweder in der Startelf (6 Punkte) war oder auf der Bank (2 Punkte)
            Info.Player[] temp = PlayerInfo;
            foreach (var s in fixture.Lineups[0].StartXI)
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

            foreach (var s in fixture.Lineups[1].StartXI)
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

            foreach (var s in fixture.Lineups[0].Substitutes)
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

            foreach (var s in fixture.Lineups[1].Substitutes)
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

        private void SetEventPoints(JsonFixture.Response fixture)
        {
            // Aus den geladenen Spieltag-Details werden die Events ausgewertet:
            // -> Goal : 16 Punkte;
            // -> Subst (Einwechslung): 2 Punkte;
            // -> Card: Gelbe Karte (-1 Punkt), Rote Karte (-4 Punkte)
            Info.Player[] temp = PlayerInfo;
            foreach (var e in fixture.Events)
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

        private void SetPlaydayList(JsonFixture.Response fixture)
        {
            // Füllt die PlaydayList-Property für das GameHomeWindow
            using (FootballContext context = new FootballContext())
            {
                var venue = context.Venue.Where(v => v.VenuePk == fixture.Fixture.Venue.Id).FirstOrDefault();

                PlaydayList.Add(_create.PlaydayInfo(fixture, venue));
            }
        }
        #endregion
    }
}
