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
        private User _user;

        // Commands
        private RelayCommand _save;
        public ICommand SaveCommand { get { return _save; } }

        // Konstruktor
        public NewTeamViewModel(Window window, User user)
        {
            // Attribute
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

                UserTeam temp = new UserTeam();
                temp.UserTeamName = NewTeamName;
                temp.UserTeamLeague = _selectedLeague.LeaguePk;
                temp.UserTeamSeason = _selectedSeason.SeasonPk;
                temp.UserTeamUserFk = _user.UserPk;
                context.UserTeam.Add(temp);
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
