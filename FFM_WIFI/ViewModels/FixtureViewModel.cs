using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
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
        private ObservableCollection<FixtureInfo> _infoList;
        public ObservableCollection<FixtureInfo> InfoList
        {
            get { return _infoList; }
            set
            {
                _infoList = value;
                OnPropertyChanged();
            }
        }

        // Property für User Team und Performance
        public PlayerInfo[] PlayerData { get; set; }
        public ObservableCollection<PlayerInfo> DraftedPlayers { get; set; }
        private PlayerInfo _selectedPlayer;
        public PlayerInfo SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged();
            }
        }

        // Property für TextBlock und Image
        private TeamInfo _teamData;
        public TeamInfo TeamData
        {
            get { return _teamData; }
            set
            {
                _teamData = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Attributes
        private Window _window;
        private UserTeam _userTeam;
        private int _playday;
        private bool _back;
        #endregion

        #region Commands
        private RelayCommand _start;
        public ICommand StartCommand { get { return _start; } }
        //private RelayCommand _detail;
        //public ICommand DetailCommand { get { return _detail; } }
        private RelayCommand _home;
        public ICommand HomeCommand { get { return _home; } }
        #endregion

        // Konstruktor
        public FixtureViewModel(Window window, TeamInfo teamData, PlayerInfo[] playerData)
        {
            _window = window;
            TeamData = teamData;
            _userTeam = _teamData.Team;
            PlayerData = playerData;
            InfoList = new ObservableCollection<FixtureInfo>();
            DraftedPlayers = new ObservableCollection<PlayerInfo>();
            SetDraftedPlayersList();
            _playday = _userTeam.UserTeamPlayday;
            _back = false;
            _start = new RelayCommand(CalculatePlayday, () => _playday == _userTeam.UserTeamPlayday);
            _home = new RelayCommand(GoToGameHome, () => _back);
            _home.RaiseCanExecuteChanged();
        }

        private void GoToGameHome()
        {
            _teamData.Playday++;
            _teamData.Team.UserTeamPlayday++;
            GameHomeWindow uhWindow = new GameHomeWindow(TeamData, PlayerData);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void GoToResult()
        {
            ResultWindow rWindow = new ResultWindow(TeamData, PlayerData);
            _window.Close();
            rWindow.ShowDialog();
        }

        private void CalculatePlayday()
        {
            // Punkte berechnen und PlaydayText setzen
            Calculate calc = new Calculate(_playday, PlayerData);
            calc.TeamPoints();

            PlayerData = calc.TeamData;
            InfoList = calc.InfoList;
            SetDraftedPlayersList();
            _playday++;
            if (_playday > 34)
            {
                GoToResult();
            }
            _start.RaiseCanExecuteChanged();
            _back = true;
            _home.RaiseCanExecuteChanged();
        }

        private void SetDraftedPlayersList()
        {
            DraftedPlayers.Clear();
            foreach (var p in PlayerData)
            {
                if (p.Drafted)
                    DraftedPlayers.Add(p);
            }
        }
    }
}
