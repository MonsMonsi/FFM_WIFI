using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FFM_WIFI.Views;

namespace FFM_WIFI.ViewModels
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // public int Age { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        public int Points { get; set; }
        public bool Drafted { get; set; }

        public PlayerInfo(int id, string name, string position, string image, int points, bool drafted)
        {
            Id = id;
            Name = name;
            // Age = age;
            Position = position;
            Image = image;
            Points = points;
            Drafted = drafted;
        }
    }

    class GameHomeViewModel : BaseViewModel
    {
        // Properties für User
        private UserTeam _userTeam;
        public UserTeam UserTeam
        {
            get { return _userTeam; }
            set
            {
                _userTeam = value;
                OnPropertyChanged("UserTeam");
            }
        }

        // Properties für User Team und Performance
        public PlayerInfo[] UserTeamData { get; set; }

        // Property für FixtureWindow
        public int[] PlaydayFixtures { get; set; }

        // Properties für Datagrids
        public ObservableCollection<PlayerInfo> PlayerDataList { get; set; }
        public ObservableCollection<PlayerInfo> PlayerDraftList { get; set; }
        private PlayerInfo _selectedPlayer;
        public PlayerInfo SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");
                _line.RaiseCanExecuteChanged();
            }
        }
        
        // Property für Canvas
        private PlayerInfo[] _lineUp;
        public PlayerInfo[] LineUp
        {
            get { return _lineUp; }
            set
            {
                _lineUp = value;
            }
        }

        // Commands
        private RelayCommand<object> _sub;
        public ICommand SubCommand { get { return _sub; } }
        private RelayCommand _line;
        public ICommand LineUpCommand { get { return _line; } }
        private RelayCommand _play;
        public ICommand PlayCommand { get { return _play; } }
        private RelayCommand _save;
        public ICommand SaveCommand { get { return _save; } }

        // Attribute
        private Window _window;
        private User _user = new User();
        private int _playday;
        private int _iMin;
        private int _iMax;
        private int _lineCount;
        private string _position;
        private bool _positionChanged;
        private bool _complete;

        // Konstruktor
        public GameHomeViewModel(Window window, UserTeam userTeam, PlayerInfo[] userTeamData)
        {
            // Daten setzen Listen initialisieren
            _window = window;
            UserTeam = userTeam;
            // else -> Methode die TeamData liest
            _playday = UserTeam.UserTeamPlayday;
            _iMin = 0;
            _iMax = 0;
            _lineCount = 0;
            // Spielerdaten setzen oder updaten
            UserTeamData = new PlayerInfo[17];
            PlayerDraftList = new ObservableCollection<PlayerInfo>();
            GetUserTeamData();
            PlaydayFixtures = new int[9];
            LineUp = new PlayerInfo[11];
            PlayerDataList = new ObservableCollection<PlayerInfo>();
            SetPlayerDataList();
            SetPlayerDraftList();
            // Commands
            _sub = new RelayCommand<object>(SubPlayer);
            _line = new RelayCommand(LineUpPlayer, () => _lineCount < 11 && SelectedPlayer != null);
            _play = new RelayCommand(GoToFixture, () => _lineCount == 11 && _playday < 35);
            _save = new RelayCommand(GoToUserHome);
            _complete = CheckComplete();
            _play.RaiseCanExecuteChanged();
            SetIndexes();
            GetAllFixtures();
        }

        private void GoToFixture()
        {
            FixtureWindow fWindow = new FixtureWindow(UserTeam, PlaydayFixtures, UserTeamData);
            _window.Close();
            fWindow.ShowDialog();
        }

        private void GoToUserHome()
        {
            WriteData();
            SetUser();
            UserHomeWindow uhWindow = new UserHomeWindow(_user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void SubPlayer (object position)
        {
            string t = position.ToString();
            int i = int.Parse(t);

            if (LineUp[i] != null)
            {
                LineUp[i].Drafted = false;
                PlayerDataList.Add(LineUp[i]);
                LineUp[i] = null;
                OnPropertyChanged("LineUp");
                _lineCount--;
            }
            _play.RaiseCanExecuteChanged();
        }

        private void LineUpPlayer()
        {
            if (SelectedPlayer != null)
            {
                LineUp[_lineCount] = SelectedPlayer;
                PlayerDraftList.Remove(SelectedPlayer);
                LineUp[_lineCount].Drafted = true;
                OnPropertyChanged("LineUp");
                _lineCount++;
            }
            SetPlayerDraftList();
            _line.RaiseCanExecuteChanged();
            _play.RaiseCanExecuteChanged();
        }

        private void GetAllFixtures()
        {
            string filePath = @"D:\VS_Projects\FFM_WIFI\JsonFiles\AllFixturesBULI2020\AllFixturesBULI2020.json";

            StreamReader reader = File.OpenText(filePath);
          
            var fixtures = JsonConvert.DeserializeObject<JsonAllFixtures.Root>(reader.ReadToEnd());

            int index = 0;
            for (int i = _iMin; i < _iMax; i++)
            {
                PlaydayFixtures[index] = fixtures.response[i].fixture.id;
                index++;
            }
        }

        private void GetUserTeamData()
        {
            using (FootballContext context = new FootballContext())
            {
                // nullable = ändern -> "NewTeam" überarbeiten -> Defaultwert
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

                for (int i = 0; i < 17; i++)
                {
                    var player = context.Player.Where(p => p.PlayerPk == players[i]).FirstOrDefault();
                    PlayerInfo temp = new PlayerInfo(players[i], player.PlayerFirstName + player.PlayerLastName,
                    player.PlayerPosition, player.PlayerImage, points[i], false);

                    UserTeamData[i] = temp;
                }
                SetPlayerDraftList();
            }
        }

        private void SetPlayerDataList()
        {
            foreach (var p in UserTeamData)
            {
                PlayerDataList.Add(p);
            }
        }

        private void SetPlayerDraftList()
        {
            SetPosition();
            if (_positionChanged)
            {
                PlayerDraftList.Clear();
                foreach (var p in UserTeamData)
                {
                    if (p.Position == _position)
                        PlayerDraftList.Add(p);
                }
            _positionChanged = false;
            }
        }

        private void SetPosition()
        {
            switch (_lineCount)
            {
                case 0:
                    _position = "Goalkeeper";
                    _positionChanged = true;
                    break;
                case 1:
                    _position = "Defender";
                    _positionChanged = true;
                    break;
                case 5:
                    _position = "Midfielder";
                    _positionChanged = true;
                    break;
                case 9:
                    _position = "Attacker";
                    _positionChanged = true;
                    break;
            }
        }

        private void WriteData()
        {
            using (FootballContext context = new FootballContext())
            {
                var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == UserTeam.UserTeamPk).FirstOrDefault();

                performance.UserTeamPerformanceGk1 = UserTeamData[0].Points; performance.UserTeamPerformanceAt1 = UserTeamData[9].Points;
                performance.UserTeamPerformanceDf1 = UserTeamData[1].Points; performance.UserTeamPerformanceAt2 = UserTeamData[10].Points;
                performance.UserTeamPerformanceDf2 = UserTeamData[2].Points; performance.UserTeamPerformanceGk2 = UserTeamData[11].Points;
                performance.UserTeamPerformanceDf3 = UserTeamData[3].Points; performance.UserTeamPerformanceDf5 = UserTeamData[12].Points;
                performance.UserTeamPerformanceDf4 = UserTeamData[4].Points; performance.UserTeamPerformanceMf5 = UserTeamData[13].Points;
                performance.UserTeamPerformanceMf1 = UserTeamData[5].Points; performance.UserTeamPerformanceMf6 = UserTeamData[14].Points;
                performance.UserTeamPerformanceMf2 = UserTeamData[6].Points; performance.UserTeamPerformanceAt3 = UserTeamData[15].Points;
                performance.UserTeamPerformanceMf3 = UserTeamData[7].Points; performance.UserTeamPerformanceAt4 = UserTeamData[16].Points;
                performance.UserTeamPerformanceMf4 = UserTeamData[8].Points;

                context.SaveChanges();
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

        private void SetUser()
        {
            using (FootballContext context = new FootballContext())
            {
                var user = context.User.Where(u => u.UserPk == UserTeam.UserTeamUserFk).FirstOrDefault();
                _user = user;
            }
        }

        private bool CheckComplete()
        {
            for (int i = 0; i < 17; i++)
            {
                if (LineUp[i] == null)
                    return false;
            }
            return true;
        }
    }
}
