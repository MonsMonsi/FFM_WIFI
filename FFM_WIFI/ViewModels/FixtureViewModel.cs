using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FFM_WIFI.Views;

namespace FFM_WIFI.ViewModels
{
    class FixtureViewModel : BaseViewModel
    {
        // Property für User Team und Performance
        public PlayerInfo[] UserTeamData { get; set; }

        // Property für ListView
        public ObservableCollection<string> PlaydayText { get; set; }

        // Attribute
        private Window _window;
        private int[] _fixtures;
        private UserTeam _userTeam;
        private int _teamPk;
        private bool _back;
        private int _playday;
        private string _header;
        private string _line;

        // Commands
        private RelayCommand _start;
        public ICommand StartCommand { get { return _start; } }
        private RelayCommand _home;
        public ICommand HomeCommand { get { return _home; } }

        // Konstruktor
        public FixtureViewModel(Window window, UserTeam userTeam, int[] fixtures, PlayerInfo[] userTeamData)
        {
            // Daten setzen und initialisieren
            _window = window;
            _userTeam = userTeam;
            _fixtures = fixtures;
            UserTeamData = userTeamData;
            _playday = userTeam.UserTeamPlayday;
            _teamPk = userTeam.UserTeamPk;
            _back = false;
            PlaydayText = new ObservableCollection<string>();
            SetStrings();
            // Commands
            _start = new RelayCommand(CalculatePlayday, () => _playday == userTeam.UserTeamPlayday);
            _home = new RelayCommand(GoToGameHome, () => _back);
        }

        private void GoToGameHome()
        {
            SetPlayday();
            SaveToDatabase();
            GameHomeWindow uhWindow = new GameHomeWindow(_userTeam, UserTeamData);
            _window.Close();
            uhWindow.ShowDialog();   
        }

        private void CalculatePlayday()
        {
            foreach (var id in _fixtures)
            {
                string filePath = @$"D:\VS_Projects\FFM_WIFI\JsonFiles\FixtureDetailsBULI2020\BULI2020_Fixture{id}.json";

                StreamReader reader = File.OpenText(filePath);

                var fixture = JsonConvert.DeserializeObject<JsonFixture.Root>(reader.ReadToEnd());

                SetLineUpPoints(fixture);

                ShowPlayDay(fixture);
            }

            using (FootballContext context = new FootballContext())
            {
                var team = context.UserTeam.Where(t => t.UserTeamPk == _teamPk).FirstOrDefault();

                team.UserTeamPlayday++;
                context.SaveChanges();
            }
            _playday++;
            _start.RaiseCanExecuteChanged();
            
            _back = true;
            _home.RaiseCanExecuteChanged();
        }

        private void ShowPlayDay(JsonFixture.Root fixture)
        {
            PlaydayText.Add(_header + "\n" + fixture.response[0].teams.home.name + " vs. " + fixture.response[0].teams.away.name);
            PlaydayText.Add(_line);

            foreach (var e in fixture.response[0].events)
            {
                string eventType1 = "";
                string eventType2 = "";

                switch (e.type)
                {
                    case "Goal":
                        eventType1 = $"Torschütze: {e.player.name}";
                        eventType2 = $" - Assist: {e.assist.name}";
                        break;
                    case "subst":
                        eventType1 = $"Aus: {e.player.name}";
                        eventType2 = $" - Ein: {e.assist.name}";
                        break;
                    case "Card":
                        eventType1 = $"Karte für: {e.player.name}";
                        break;
                }

                string newEvent = $"Minute: {e.time.elapsed} - Team: {e.team.name}\n" +
                                  $"{e.type}!\n" +
                                  $"{eventType1}{eventType2}\n" +
                                  $"{_line}";
                PlaydayText.Add(newEvent);
            }
        }

        private void SetLineUpPoints(JsonFixture.Root fixture)
        {
            foreach (var s in fixture.response[0].lineups[0].startXI)
            {
                foreach (var p in UserTeamData)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 4;
                        OnPropertyChanged("UserTeamData");
                    }
                }
            }

            foreach (var s in fixture.response[0].lineups[1].startXI)
            {
                foreach (var p in UserTeamData)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 4;
                        OnPropertyChanged("UserTeamData");
                    }
                }
            }

            foreach (var s in fixture.response[0].lineups[0].substitutes)
            {
                foreach (var p in UserTeamData)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 1;
                        OnPropertyChanged("UserTeamData");
                    }
                }
            }

            foreach (var s in fixture.response[0].lineups[1].substitutes)
            {
                foreach (var p in UserTeamData)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 1;
                        OnPropertyChanged("UserTeamData");
                    }
                }
            }
        }

        private void SaveToDatabase()
        {
            using (FootballContext context = new FootballContext())
            {
                var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == _userTeam.UserTeamPk).FirstOrDefault();

                performance.UserTeamPerformanceGk1 = UserTeamData[0].Points; performance.UserTeamPerformanceDf1 = UserTeamData[1].Points; performance.UserTeamPerformanceDf2 = UserTeamData[2].Points; performance.UserTeamPerformanceDf3 = UserTeamData[3].Points; performance.UserTeamPerformanceDf4 = UserTeamData[4].Points; 
                performance.UserTeamPerformanceMf1 = UserTeamData[5].Points; performance.UserTeamPerformanceMf2 = UserTeamData[6].Points; performance.UserTeamPerformanceMf3 = UserTeamData[7].Points; performance.UserTeamPerformanceMf4 = UserTeamData[8].Points; performance.UserTeamPerformanceAt1 = UserTeamData[9].Points;
                performance.UserTeamPerformanceAt2 = UserTeamData[10].Points; performance.UserTeamPerformanceGk2 = UserTeamData[11].Points; performance.UserTeamPerformanceDf5 = UserTeamData[12].Points; performance.UserTeamPerformanceMf5 = UserTeamData[13].Points; performance.UserTeamPerformanceMf6 = UserTeamData[14].Points;
                performance.UserTeamPerformanceAt3 = UserTeamData[15].Points; performance.UserTeamPerformanceAt4 = UserTeamData[16].Points;

                context.SaveChanges();
            }
        }

        private void SetPlayday()
        {
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamName == _userTeam.UserTeamName).FirstOrDefault();

                userTeam.UserTeamPlayday = _playday;
                _userTeam = userTeam;
                context.SaveChanges();
            }
        }

        private void SetStrings()
        {
            _header = "--- Paarung:";
            _line = "--------------------------------------------------------";
        }
    }
}
