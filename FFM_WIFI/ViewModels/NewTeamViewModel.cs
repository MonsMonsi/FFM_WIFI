using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class NewTeamViewModel : BaseViewModel
    {
        // Properties
        private string _newTeamName;
        public string NewTeamName
        {
            get { return _newTeamName; }
            set
            {
                _newTeamName = value;
                OnPropertyChanged("NewTeamName");
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
                OnPropertyChanged("SelectedLeague");
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
                OnPropertyChanged("SelectedSeason");
                _save.RaiseCanExecuteChanged();
            }
        }

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
            _save = new RelayCommand(SaveTeam, () => SelectedLeague != null && SelectedSeason != null);
            // Listen
            LeagueList = new ObservableCollection<League>();
            SeasonList = new ObservableCollection<Season>();
            GetLeagueSeason();
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
                UserTeam team = new UserTeam();
                team.UserTeamName = NewTeamName;
                team.UserTeamLeague = _selectedLeague.LeaguePk;
                team.UserTeamSeason = _selectedSeason.SeasonPk;
                team.UserTeamUserFk = _user.UserPk;
                team.UserTeamPlayday = 1;
                team.UserTeamGk1 = 1001001; team.UserTeamDf1 = 1001001; team.UserTeamDf2 = 1001001; team.UserTeamDf3 = 1001001; team.UserTeamDf4 = 1001001; team.UserTeamMf1 = 1001001; team.UserTeamMf2 = 1001001;
                team.UserTeamMf3 = 1001001; team.UserTeamMf4 = 1001001; team.UserTeamAt1 = 1001001; team.UserTeamAt2 = 1001001; team.UserTeamGk2 = 1001001; team.UserTeamDf5 = 1001001; team.UserTeamMf5 = 1001001;
                team.UserTeamMf6 = 1001001; team.UserTeamAt3 = 1001001; team.UserTeamAt4 = 1001001; team.UserTeamNumberPlayers = 0;
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

        private void GetLeagueSeason()
        {
            using (FootballContext context = new FootballContext())
            {
                var leagues = context.League;
                var seasons = context.Season;

                foreach (var item in leagues)
                {
                    League temp = new League();
                    temp = item;
                    LeagueList.Add(temp);
                }

                foreach (var item in seasons)
                {
                    Season temp = new Season();
                    temp = item;
                    SeasonList.Add(temp);
                }
            }
        }
    }
}
