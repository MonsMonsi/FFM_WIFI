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
        private ObservableCollection<Info.Fixture> _infoList;
        public ObservableCollection<Info.Fixture> InfoList
        {
            get { return _infoList; }
            set
            {
                _infoList = value;
                OnPropertyChanged();
            }
        }

        // Property für User Team und Performance
        public Info.Player[] PlayerData { get; set; }
        public ObservableCollection<Info.Player> DraftedPlayers { get; set; }
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
        private Info.Team _teamData;
        public Info.Team TeamData
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
        private _userTeam _userTeam;
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
        public FixtureViewModel(Window window, Info.Team teamData, Info.Player[] playerData)
        {
            _window = window;
            TeamData = teamData;
            _userTeam = _teamData.UserTeam;
            PlayerData = playerData;
            InfoList = new ObservableCollection<Info.Fixture>();
            DraftedPlayers = new ObservableCollection<Info.Player>();
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
            _teamData.UserTeam.UserTeamPlayday++;
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
