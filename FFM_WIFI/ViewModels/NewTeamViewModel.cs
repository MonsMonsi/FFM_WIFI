using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.ViewModels
{
    public class TeamLogo
    {
        public BitmapImage Logo { get; set; }
        public string LogoPath { get; set; }
        public string Name { get; set; }
        public TeamLogo(BitmapImage logo, string path, string name)
        {
            Logo = logo;
            LogoPath = path;
            Name = name;
        }
    }

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
            }
        }

        // Properties für Combobox
        public ObservableCollection<League> LeagueList { get; set; }

        private League _selectedLeague;
        public League SelectedLeague
        {
            get { return _selectedLeague; }
            set
            {
                _selectedLeague = value;
                OnPropertyChanged();
                _save.RaiseCanExecuteChanged();
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
                _save.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<TeamLogo> LogoList { get; set; }
        private TeamLogo _selectedLogo;
        public TeamLogo SelectedLogo
        {
            get { return _selectedLogo; }
            set
            {
                _selectedLogo = value;
                OnPropertyChanged();
                _save.RaiseCanExecuteChanged();
            }
        }
        #endregion

        // Attribute
        private Window _window;
        private User _user;

        // Commands
        private RelayCommand _save;
        public ICommand SaveCommand { get { return _save; } }

        // Konstruktor
        public NewTeamViewModel(Window window, User user)
        {
            // Attribute
            _window = window;
            _user = user;
            // Commands
            _save = new RelayCommand(SaveTeam, () => SelectedLeague != null && SelectedSeason != null && SelectedLogo != null);
            // Listen
            LeagueList = new ObservableCollection<League>();
            SeasonList = new ObservableCollection<Season>();
            LogoList = new ObservableCollection<TeamLogo>();
            GetComboBoxData();
        }

        private void GoToUserHome()
        {
            UserHomeWindow uhWindow = new UserHomeWindow(_user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void SaveTeam()
        {
            using (FootballContext context = new FootballContext())
            {

                //var user = context.UserTeam.Where(u => u.UserTeamUserFkNavigation.UserName == _user.UserName)
                //                           .FirstOrDefault();

                //if (user != null && user.UserTeamName != NewTeamName)
                //{
                //    user.UserTeamName = NewTeamName;
                //    user.UserTeamLeague = _selectedLeague.LeaguePk;
                //    user.UserTeamSeason = _selectedSeason.SeasonPk;
                //    context.SaveChanges();
                //    GoToUserHome();
                //}


                // neues Team
                _userTeam team = new _userTeam();
                team.UserTeamName = NewTeamName;
                team.UserTeamLogo = _selectedLogo.LogoPath;
                team.UserTeamLeague = _selectedLeague.LeaguePk;
                team.UserTeamSeason = _selectedSeason.SeasonPk;
                team.UserTeamUserFk = _user.UserPk;
                team.UserTeamPlayday = 1;
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
        }

        private void GetComboBoxData()
        {
            using (FootballContext context = new FootballContext())
            {
                var leagues = context.League;
                var seasons = context.Season;
                var teams = context.Team;

                foreach (var l in leagues)
                {
                    League temp = new League();
                    temp = l;
                    LeagueList.Add(temp);
                }

                foreach (var s in seasons)
                {
                    Season temp = new Season();
                    temp = s;
                    SeasonList.Add(temp);
                }

                foreach (var t in teams)
                {
                    TeamLogo temp = new TeamLogo(new GetFrom().Image(t.TeamLogo), t.TeamLogo, t.TeamName);
                    LogoList.Add(temp);
                }
            }
        }
    }
}
