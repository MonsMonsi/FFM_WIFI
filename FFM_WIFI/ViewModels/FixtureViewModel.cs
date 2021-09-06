using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.Views;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class FixtureViewModel : BaseViewModel
    {
        #region Propperties
        // Property für DetailBorder
        private Info.Fixture _currentFixture;
        public Info.Fixture CurrentFixture
        {
            get { return _currentFixture; }
            set
            {
                _currentFixture = value;
                OnPropertyChanged();
            }
        }

        // Property PlayerList
        public ObservableCollection<Info.Player> PlayerList { get; set; }
        private Info.Player _selectedPlayer;
        public Info.Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged();
            }
        }

        // Property für TextBlock und Image
        private Info.Team _teamInfo;
        public Info.Team TeamInfo
        {
            get { return _teamInfo; }
            set
            {
                _teamInfo = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Attributes
        private Window _window;
        private UserTeam _userTeam;
        private Info.Player[] _playerInfo;
        private Info.Fixture[] _fixtureInfo;
        private Calculate calc;
        private int _fixtureCount;
        private int _playday;
        private int _max;
        private bool _back;
        #endregion

        #region Commands
        private RelayCommand _previous;
        public ICommand PreviousCommand { get { return _previous; } } // Zeigt das vorige Spiel an

        private RelayCommand _next;
        public ICommand NextCommand { get { return _next; } } // zeigt das nächste Spiel an

        private RelayCommand _home;
        public ICommand HomeCommand { get { return _home; } } // Zurück zum gameHomeWindow
        #endregion

        #region Constructor
        public FixtureViewModel(Window window, Info.Team teamInfo, Info.Player[] playerInfo)
        {
            TeamInfo = teamInfo;
            PlayerList = new ObservableCollection<Info.Player>();
            _window = window;
            _fixtureCount = 0;
            _userTeam = _teamInfo.UserTeam;
            _playerInfo = playerInfo;
            SetDraftedPlayersList();
            _playday = _userTeam.UserTeamPlayday;
            calc = new Calculate(_playday, TeamInfo.UserTeam.UserTeamLeague, TeamInfo.Season, _playerInfo);
            calc.TeamPoints();
            _max = 9;
            ListToArray();
            _back = false;
            _previous = new RelayCommand(ShowPrevious, () => _fixtureCount > 0);
            _next = new RelayCommand(ShowNext, () => _fixtureCount < _max - 1);
            _home = new RelayCommand(GoToGameHome, () => _back);
            _home.RaiseCanExecuteChanged();
        }
        #endregion

        #region Methods
        private void GoToGameHome()
        {
            UpdateTeamInfo();
            if (_playday > 34)
            {
                GoToResult();
            }
            GameHomeWindow uhWindow = new GameHomeWindow(TeamInfo, _playerInfo);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void GoToResult()
        {
            ResultWindow rWindow = new ResultWindow(TeamInfo, _playerInfo);
            _window.Close();
            rWindow.ShowDialog();
        }

        private void UpdateTeamInfo()
        {
            // Wird aufgerufen, bevor User zum GameHomeWindow zurückgeleitet wird
            // Updated den Spieltag
            TeamInfo.Playday++;
            TeamInfo.UserTeam.UserTeamPlayday++;
            var create = new Create.Info();

            TeamInfo.BestPlayers = create.GetBestPlayers(_playerInfo);
        }

        private void ShowPrevious()
        {
            _fixtureCount--;
            CurrentFixture = _fixtureInfo[_fixtureCount];
            _previous.RaiseCanExecuteChanged();
            _next.RaiseCanExecuteChanged();
        }

        private void ShowNext()
        {
            _fixtureCount++;
            if (_fixtureCount == _max - 1)
            {
                _playday++;
                _back = true;
                _home.RaiseCanExecuteChanged();
            }
            CurrentFixture = _fixtureInfo[_fixtureCount];
            _previous.RaiseCanExecuteChanged();
            _next.RaiseCanExecuteChanged();
        }

        private void ListToArray()
        {
            // Wandelt die SpieltagsListe in ein Array um, somit kann leichter auf einzelne Spieltage zugegriffen werden
            if (TeamInfo.UserTeam.UserTeamLeague == 61 || TeamInfo.UserTeam.UserTeamLeague == 39)
            {
                _max = 10;
            }
            _fixtureInfo = new Info.Fixture[_max];
            foreach (var f in calc.FixtureList)
            {
                _fixtureInfo[_fixtureCount] = f;
                _fixtureCount++;
            }
            _fixtureCount = -1;
        }

        private void SetDraftedPlayersList()
        {
            // Füllt die Spieler-ListView
            PlayerList.Clear();
            foreach (var p in _playerInfo)
            {
                if (p.Drafted)
                {
                    PlayerList.Add(p);
                }
            }
        }
        #endregion
    }
}
