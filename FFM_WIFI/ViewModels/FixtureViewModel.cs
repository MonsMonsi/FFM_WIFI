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
        //Property für FixtureInfos
        private ObservableCollection<Info.Fixture> _fixtureList;
        public ObservableCollection<Info.Fixture> FixtureList
        {
            get { return _fixtureList; }
            set
            {
                _fixtureList = value;
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
        private RelayCommand _all;
        public ICommand AllCommand { get { return _all; } }

        private RelayCommand _next;
        public ICommand NextCommand { get { return _next; } }

        private RelayCommand _home;
        public ICommand HomeCommand { get { return _home; } }
        #endregion

        // Konstruktor
        public FixtureViewModel(Window window, Info.Team teamInfo, Info.Player[] playerInfo)
        {
            _window = window;
            TeamInfo = teamInfo;
            _fixtureCount = 0;
            _userTeam = _teamInfo.UserTeam;
            _playerInfo = playerInfo;
            FixtureList = new ObservableCollection<Info.Fixture>();
            PlayerList = new ObservableCollection<Info.Player>();
            SetDraftedPlayersList();
            _playday = _userTeam.UserTeamPlayday;
            calc = new Calculate(_playday, _playerInfo);
            calc.TeamPoints();
            _back = false;
            _all = new RelayCommand(ShowAll, () => _playday == _userTeam.UserTeamPlayday);
            _next = new RelayCommand(ShowNext, () => _playday == _userTeam.UserTeamPlayday);
            _home = new RelayCommand(GoToGameHome, () => _back);
            _home.RaiseCanExecuteChanged();
        }

        private void GoToGameHome()
        {
            _teamInfo.Playday++;
            _teamInfo.UserTeam.UserTeamPlayday++;
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

        private void ShowAll()
        {
            _playerInfo = calc.PlayerInfo;
            FixtureList = calc.FixtureList;
            SetDraftedPlayersList();
            _playday++;
            if (_playday > 34)
            {
                GoToResult();
            }
            _all.RaiseCanExecuteChanged();
            _next.RaiseCanExecuteChanged();
            _back = true;
            _home.RaiseCanExecuteChanged();
        }

        private void ShowNext()
        {
            if (_fixtureCount == 0)
            {
                _playerInfo = calc.PlayerInfo;
                ListToArray();
            }
            FixtureList.Clear();
            FixtureList.Add(_fixtureInfo[_fixtureCount]);
            _fixtureCount++;
            if (_fixtureCount == 9)
            {
                _playday++;
                _all.RaiseCanExecuteChanged();
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
                PlayerList.Add(p);
            }
        }
    }
}
