using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.ViewModels
{
    class NewTeamViewModel : BaseViewModel
    {
        #region Properties
        private string _newTeamName;
        public string NewTeamName
        {
            get { return _newTeamName; }
            set
            {
                _newTeamName = value;
                OnPropertyChanged();
                _name.RaiseCanExecuteChanged();
            }
        }

        // Comboboxes
        public ObservableCollection<League> LeagueList { get; set; }

        private League _selectedLeague;
        public League SelectedLeague
        {
            get { return _selectedLeague; }
            set
            {
                _selectedLeague = value;
                OnPropertyChanged();
                _leagueSeason.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Season> SeasonList { get; set; }
        private Season _selectedSeason;
        public Season SelectedSeason
        {
            get { return _selectedSeason; }
            set
            {
                _selectedSeason = value;
                OnPropertyChanged();
                _leagueSeason.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<string> LogoList { get; set; }
        private string _selectedLogo;
        public string SelectedLogo
        {
            get { return _selectedLogo; }
            set
            {
                _selectedLogo = value;
                OnPropertyChanged();
                _save.RaiseCanExecuteChanged();
                _undo.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Attributes
        private Window _window;
        private User _user;
        private GetFrom.Database _get;
        private bool _nameSubmitted = false;
        private bool _leagueSeasonSubmitted = false;
        #endregion

        #region Commands
        private RelayCommand _name;
        public ICommand NameCommand => _name;

        private RelayCommand _leagueSeason;
        public ICommand LeagueSeasonCommand => _leagueSeason;

        private RelayCommand _undo;
        public ICommand UndoCommand => _undo;

        private RelayCommand _save;
        public ICommand SaveCommand => _save;
        #endregion

        // Konstruktor
        public NewTeamViewModel(Window window, User user)
        {
            _window = window;
            _user = user;
            _get = new GetFrom.Database(_user);
            _name = new RelayCommand(SubmitName, () => NewTeamName != null && NewTeamName.Length != 0 && !_nameSubmitted);
            _leagueSeason = new RelayCommand(SubmitLeagueSeason, () => SelectedLeague != null && SelectedSeason != null && !_leagueSeasonSubmitted);
            _undo = new RelayCommand(UndoAll, () => SelectedLogo != null);
            _save = new RelayCommand(SaveTeam, () => SelectedLogo != null);
        }

        #region Methods
        private void GoToUserHome()
        {
            UserHomeWindow uhWindow = new UserHomeWindow(_user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void SubmitName()
        {
            LeagueList = new ObservableCollection<League>(_get.LeagueList());
            OnPropertyChanged("LeagueList");
            SeasonList = new ObservableCollection<Season>(_get.SeasonList());
            OnPropertyChanged("SeasonList");
            _nameSubmitted = true;
            _name.RaiseCanExecuteChanged();
        }

        private void SubmitLeagueSeason()
        {
            LogoList = new ObservableCollection<string>(_get.LogoList());
            OnPropertyChanged("LogoList");
            _leagueSeasonSubmitted = true;
            _leagueSeason.RaiseCanExecuteChanged();
        }

        private void UndoAll()
        {
            LogoList.Clear();
            SeasonList.Clear();
            LeagueList.Clear();
            NewTeamName = "";
            _leagueSeasonSubmitted = false;
            _nameSubmitted = false;
            _save.RaiseCanExecuteChanged();
            _leagueSeason.RaiseCanExecuteChanged();
            _name.RaiseCanExecuteChanged();
        }

        private void SaveTeam()
        {
            using (FootballContext context = new FootballContext())
            {
                var t = context.UserTeam.Where(t => t.UserTeamName.ToUpper() == NewTeamName.ToUpper()).FirstOrDefault();

                if (t == null)
                {
                    // neues Team
                    UserTeam team = new UserTeam();
                    team.UserTeamName = NewTeamName;
                    team.UserTeamLogo = _selectedLogo;
                    team.UserTeamLeague = _selectedLeague.LeaguePk;
                    team.UserTeamSeason = _selectedSeason.SeasonPk;
                    team.UserTeamUserFk = _user.UserPk;
                    team.UserTeamPlayday = 1;
                    team.UserTeamPoints = 0;
                    team.UserTeamGk1 = 0; team.UserTeamDf1 = 0; team.UserTeamDf2 = 0; team.UserTeamDf3 = 0; team.UserTeamDf4 = 0; team.UserTeamMf1 = 0; team.UserTeamMf2 = 0;
                    team.UserTeamMf3 = 0; team.UserTeamMf4 = 0; team.UserTeamAt1 = 0; team.UserTeamAt2 = 0; team.UserTeamGk2 = 0; team.UserTeamDf5 = 0; team.UserTeamMf5 = 0;
                    team.UserTeamMf6 = 0; team.UserTeamAt3 = 0; team.UserTeamAt4 = 0; team.UserTeamNumberPlayers = 0;
                    context.UserTeam.Add(team);
                    context.SaveChanges();

                    UserTeamPerformance performance = new UserTeamPerformance();
                    performance.UserTeamPerformanceUserTeamFk = team.UserTeamPk;
                    performance.UserTeamPerformanceGk1 = 0; performance.UserTeamPerformanceDf1 = 0; performance.UserTeamPerformanceDf2 = 0; performance.UserTeamPerformanceDf3 = 0;
                    performance.UserTeamPerformanceDf4 = 0; performance.UserTeamPerformanceMf1 = 0; performance.UserTeamPerformanceMf2 = 0; performance.UserTeamPerformanceMf3 = 0;
                    performance.UserTeamPerformanceMf4 = 0; performance.UserTeamPerformanceAt1 = 0; performance.UserTeamPerformanceAt2 = 0; performance.UserTeamPerformanceGk2 = 0;
                    performance.UserTeamPerformanceDf5 = 0; performance.UserTeamPerformanceMf5 = 0; performance.UserTeamPerformanceMf6 = 0; performance.UserTeamPerformanceAt3 = 0;
                    performance.UserTeamPerformanceAt4 = 0;
                    context.UserTeamPerformance.Add(performance);
                    context.SaveChanges();
                    GoToUserHome();
                }

                MessageBox.Show("Teamname ist bereits vergeben!");
                UndoAll();
            }
        }
        #endregion
    }
}
