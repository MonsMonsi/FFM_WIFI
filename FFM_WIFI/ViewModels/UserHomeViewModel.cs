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
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class UserHomeViewModel : BaseViewModel
    {
        // Properties für User
        private User _user;
        public User User 
        { 
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        // Property für Team Datagrids
        public ObservableCollection<UserTeam> UserTeamList { get; set; }
        private UserTeam _selectedUserTeam;
        public UserTeam SelectedUserTeam
        {
            get { return _selectedUserTeam; }
            set
            {
                _selectedUserTeam = value;
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedUserTeam");
                GetTeamInfo();
            }
        }

        // Property für Teamdaten
        public class TeamInfo
        {
            public string Name { get; set; }
            public int Playday { get; set; }
            public int? Players { get; set; }
            public string League { get; set; }
            public string Season { get; set; }

            public TeamInfo(string name, int day, int? players, string league, string season)
            {
                Name = name;
                Playday = day;
                Players = players;
                League = league;
                Season = season;
            }
        }

        private TeamInfo _teamData;
        public TeamInfo TeamData
        {
            get { return _teamData; }
            set
            {
                _teamData = value;
                OnPropertyChanged("TeamData");
            }
        } 

        // Commands
        public ICommand NewTeamCommand { get; set; }
        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } }
        private RelayCommand _game;
        public ICommand GameCommand { get { return _game; } }

        // Konstruktor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            // User
            User = user;
            // Commands
            //EditDBCommand = new RelayCommand(GoToEditDatabase);
            NewTeamCommand = new RelayCommand(GoToNewTeam);
            _draft = new RelayCommand(GoToDraft, () => SelectedUserTeam != null && SelectedUserTeam.UserTeamNumberPlayers != 17 && SelectedUserTeam.UserTeamName != "Keine Teams gefunden!");
            _game = new RelayCommand(GoToGameHome, () => SelectedUserTeam != null && SelectedUserTeam.UserTeamNumberPlayers == 17 && SelectedUserTeam.UserTeamName != "Keine Teams gefunden!");
            // Listen initialisieren und füllen
            UserTeamList = new ObservableCollection<UserTeam>();
            GetUserTeam();
        }

        private void GoToNewTeam()
        {
            NewTeamWindow ntwindow = new NewTeamWindow(_user);
            ntwindow.ShowDialog();
        }

        private void GoToDraft()
        {
            DraftWindow dWindow = new DraftWindow(_selectedUserTeam);
            dWindow.ShowDialog();

        }

        private void GoToGameHome()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(SelectedUserTeam);
            ghWindow.ShowDialog();
        }

        private void GetUserTeam()
        {
            using (FootballContext context = new FootballContext())
            {
                UserTeamList.Clear();
                var userTeam = context.UserTeam.Where(t => t.UserTeamUserFk == _user.UserPk).Include(t => t.UserTeamUserFkNavigation);

                if (userTeam != null)
                {
                    foreach (var u in userTeam)
                    {
                        UserTeamList.Add(u);
                    }
                }
                else
                {
                    UserTeam temp = new UserTeam();
                    temp.UserTeamName = "Keine Teams gefunden!";
                    UserTeamList.Add(temp);
                }
            }
        }

        private void GetTeamInfo()
        {
            using (FootballContext context = new FootballContext())
            {
                var league = context.League.Where(l => l.LeaguePk == SelectedUserTeam.UserTeamLeague).FirstOrDefault();
                var season = context.Season.Where(s => s.SeasonPk == SelectedUserTeam.UserTeamSeason).FirstOrDefault();

                TeamData = new TeamInfo(SelectedUserTeam.UserTeamName, SelectedUserTeam.UserTeamPlayday, SelectedUserTeam.UserTeamNumberPlayers, league.LeagueLogo, season.SeasonName);
            }
        }

        //private void DeleteTeam()
        //{
        //    using (FootballContext context = new FootballContext())
        //    {
        //        var team = context.UserTeam.Where(t => t.UserTeamPk == SelectedUserTeam.UserTeamPk).FirstOrDefault();

        //        context.Remove(team);
        //        context.SaveChanges();
        //        GetUserTeam();
        //    }
        //}
    }
}
