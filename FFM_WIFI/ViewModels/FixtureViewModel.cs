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
        private bool _back;
        #endregion

        #region Commands
        private RelayCommand _previous;
        public ICommand PreviousCommand { get { return _previous; } }

        private RelayCommand _next;
        public ICommand NextCommand { get { return _next; } }

        private RelayCommand _home;
        public ICommand HomeCommand { get { return _home; } }
        #endregion

        // Konstruktor
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
            _back = false;
            _previous = new RelayCommand(ShowPrevious, () => _playday == _userTeam.UserTeamPlayday);
            _next = new RelayCommand(ShowNext, () => _playday == _userTeam.UserTeamPlayday);
            _home = new RelayCommand(GoToGameHome, () => _back);
            _home.RaiseCanExecuteChanged();
        }

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
            TeamInfo.Playday++;
            TeamInfo.UserTeam.UserTeamPlayday++;
            var create = new Create.Info();

            TeamInfo.BestPlayers = create.GetBestPlayers(_playerInfo);
        }

        private void ShowPrevious()
        {
            if (_fixtureCount == 0)
            {
                _playerInfo = calc.PlayerInfo;
                ListToArray();
            }
            CurrentFixture = _fixtureInfo[_fixtureCount];
            _fixtureCount++;
            if (_fixtureCount == 9)
            {
                _playday++;
                _previous.RaiseCanExecuteChanged();
                _next.RaiseCanExecuteChanged();
                _back = true;
                _home.RaiseCanExecuteChanged();
            }
        }

        private void ShowNext()
        {
            if (_fixtureCount == 0)
            {
                _playerInfo = calc.PlayerInfo;
                ListToArray();
            }
            CurrentFixture = _fixtureInfo[_fixtureCount];
            _fixtureCount++;
            if (_fixtureCount == 9)
            {
                _playday++;
                _previous.RaiseCanExecuteChanged();
                _next.RaiseCanExecuteChanged();
                _back = true;
                _home.RaiseCanExecuteChanged();
            }
        }

        private void ListToArray()
        {
            _fixtureInfo = new Info.Fixture[9];
            foreach (var f in calc.FixtureList)
            {
                _fixtureInfo[_fixtureCount] = f;
                _fixtureCount++;
            }
            _fixtureCount = 0;
        }

        private void SetDraftedPlayersList()
        {
            PlayerList.Clear();
            foreach (var p in _playerInfo)
            {
                if (p.Drafted)
                {
                    PlayerList.Add(p);
                }
            }
        }
    }
}
