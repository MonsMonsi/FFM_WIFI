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
    #region InfoClasses
    public class TeamInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Playday { get; set; }
        public int? Players { get; set; }
        public string League { get; set; }
        public string Season { get; set; }
        public int Points { get; set; }
        public UserTeam Team { get; set; }
        public UserTeamPerformance Performance { get; set; }

        public TeamInfo(int id, string name, int day, int? players, string league, string season, int points, UserTeam team, UserTeamPerformance performance)
        {
            Id = id;
            Name = name;
            Playday = day;
            Players = players;
            League = league;
            Season = season;
            Points = points;
            Team = team;
            Performance = performance;
        }

        public TeamInfo()
        {

        }
    }
    #endregion
    class UserHomeViewModel : BaseViewModel
    {
        #region Properties
        // Property für Team Datagrids
        public ObservableCollection<TeamInfo> TeamList { get; set; }
        private TeamInfo _selectedTeam;
        public TeamInfo SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedTeam");
            }
        }
        #endregion

        #region Commands

        public ICommand NewTeamCommand { get; set; }
        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } }
        private RelayCommand _game;
        public ICommand GameCommand { get { return _game; } }
        #endregion


        #region Attributes

        private User _user;
        #endregion
        // Konstruktor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            // User
            _user = user;
            // Commands
            //EditDBCommand = new RelayCommand(GoToEditDatabase);
            NewTeamCommand = new RelayCommand(GoToNewTeam);
            _draft = new RelayCommand(GoToDraft, () => SelectedTeam != null && SelectedTeam.Players != 17);
            _game = new RelayCommand(GoToGameHome, () => SelectedTeam != null && SelectedTeam.Players == 17);
            // Listen initialisieren und füllen
            TeamList = new ObservableCollection<TeamInfo>();
            GetTeamInfo();
        }

        #region Methods
        private void GoToNewTeam()
        {
            NewTeamWindow ntwindow = new NewTeamWindow(_user);
            ntwindow.ShowDialog();
        }

        private void GoToDraft()
        {
            DraftWindow dWindow = new DraftWindow(_selectedTeam.Team);
            dWindow.ShowDialog();

        }

        private void GoToGameHome()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(_selectedTeam);
            ghWindow.ShowDialog();
        }

        private void GetTeamInfo()
        {
            using (FootballContext context = new FootballContext())
            {
                TeamList.Clear();
                var userTeam = context.UserTeam.Where(t => t.UserTeamUserFk == _user.UserPk).Include(t => t.UserTeamUserFkNavigation);

                if (userTeam != null)
                {
                    foreach (var u in userTeam)
                    {
                        var league = context.League.Where(l => l.LeaguePk == u.UserTeamLeague).FirstOrDefault();
                        var season = context.Season.Where(s => s.SeasonPk == u.UserTeamSeason).FirstOrDefault();
                        var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == u.UserTeamPk).FirstOrDefault();

                        int points = performance.UserTeamPerformanceGk1 + performance.UserTeamPerformanceDf1 + performance.UserTeamPerformanceDf2 + performance.UserTeamPerformanceDf3
                     + performance.UserTeamPerformanceDf4 + performance.UserTeamPerformanceMf1 + performance.UserTeamPerformanceMf2 + performance.UserTeamPerformanceMf3
                      + performance.UserTeamPerformanceMf4 + performance.UserTeamPerformanceAt1 + performance.UserTeamPerformanceAt2 + performance.UserTeamPerformanceGk2
                       + performance.UserTeamPerformanceDf5 + performance.UserTeamPerformanceMf5 + performance.UserTeamPerformanceMf6 + performance.UserTeamPerformanceAt3
                        + performance.UserTeamPerformanceAt4;

                        TeamList.Add(new TeamInfo(u.UserTeamPk, u.UserTeamName, u.UserTeamPlayday, u.UserTeamNumberPlayers, league.LeagueLogo, season.SeasonName, points, u, performance));
                    }
                }
            }
        }
        #endregion

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
