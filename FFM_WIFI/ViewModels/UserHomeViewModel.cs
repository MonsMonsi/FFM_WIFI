using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.ViewModels
{
    public class Images
    {
        public string Star { get; set; }
        public Images(string star = null)
        {
            Star = star;
        }
    }
    #region InfoClasses
    public class TeamInfo
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public BitmapImage Logo { get; set; }
        public int Playday { get; set; }
        public int? Players { get; set; }
        public BitmapImage League { get; set; }
        public string Season { get; set; }
        public int Points { get; set; }
        public UserTeam Team { get; set; }
        public UserTeamPerformance Performance { get; set; }
        public Images Images { get; set; }

        public TeamInfo(int teamId, int userId, string name, BitmapImage logo, int day, int? players, BitmapImage league, string season, int points, UserTeam team, UserTeamPerformance performance, Images images)
        {
            TeamId = teamId;
            UserId = userId;
            Name = name;
            Logo = logo;
            Playday = day;
            Players = players;
            League = league;
            Season = season;
            Points = points;
            Team = team;
            Performance = performance;
            Images = images;
        }

        public TeamInfo()
        {

        }
    }
    #endregion
    class UserHomeViewModel : BaseViewModel
    {
        #region Properties

        // User
        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        // Images
        private Images _images;
        public Images Images
        {
            get { return _images; }
            set 
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        // Team Datagrids
        public ObservableCollection<TeamInfo> ActiveTeamList { get; set; }
        private TeamInfo _selectedActiveTeam;
        public TeamInfo SelectedActiveTeam
        {
            get { return _selectedActiveTeam; }
            set
            {
                _selectedActiveTeam = value;
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TeamInfo> ClassicTeamList { get; set; }
        private TeamInfo _selectedClassicTeam;
        public TeamInfo SelectedClassicTeam
        {
            get { return _selectedClassicTeam; }
            set
            {
                _selectedClassicTeam = value;
                _pdf.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ICommand NewTeamCommand { get; set; }
        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } }
        private RelayCommand _game;
        public ICommand GameCommand { get { return _game; } }
        private RelayCommand _pdf;
        public ICommand PdfCommand { get { return _pdf; } }
        #endregion


        #region Attributes

        private Window _window;  
        #endregion
        // Konstruktor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            _window = window;
            _user = user;
            SetImages();
            // Commands
            //EditDBCommand = new RelayCommand(GoToEditDatabase);
            NewTeamCommand = new RelayCommand(GoToNewTeam);
            _draft = new RelayCommand(GoToDraft, () => SelectedActiveTeam != null && SelectedActiveTeam.Players != 17);
            _game = new RelayCommand(GoToGameHome, () => SelectedActiveTeam != null && SelectedActiveTeam.Players == 17);
            _pdf = new RelayCommand(SaveAsPdf, () => SelectedClassicTeam != null);
            // Listen initialisieren und füllen
            ActiveTeamList = new ObservableCollection<TeamInfo>();
            ClassicTeamList = new ObservableCollection<TeamInfo>();
            GetTeamInfo();
        }

        #region Methods
        private void GoToNewTeam()
        {
            NewTeamWindow ntwindow = new NewTeamWindow(_user);
            _window.Close();
            ntwindow.ShowDialog();
        }

        private void GoToDraft()
        {
            DraftWindow dWindow = new DraftWindow(_selectedActiveTeam.Team);
            dWindow.ShowDialog();

        }

        private void ResetSelectedTeams()
        {
            SelectedActiveTeam = null;
            SelectedClassicTeam = null;
        }

        private void GoToGameHome()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(_selectedActiveTeam);
            ghWindow.ShowDialog();
        }

        private void SaveAsPdf()
        {

        }

        private void GetTeamInfo()
        {
            using (FootballContext context = new FootballContext())
            {
                ActiveTeamList.Clear();
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

                        if (u.UserTeamPlayday < 35)
                        {
                            ActiveTeamList.Add(new TeamInfo(u.UserTeamPk, u.UserTeamUserFkNavigation.UserPk, u.UserTeamName, Get.Image(u.UserTeamLogo), u.UserTeamPlayday, u.UserTeamNumberPlayers, Get.Image(league.LeagueLogo), season.SeasonName, points, u, performance, Images));
                        }
                        else
                        {
                            ClassicTeamList.Add(new TeamInfo(u.UserTeamPk, u.UserTeamUserFkNavigation.UserPk, u.UserTeamName, Get.Image(u.UserTeamLogo), u.UserTeamPlayday, u.UserTeamNumberPlayers, Get.Image(league.LeagueLogo), season.SeasonName, points, u, performance, Images));
                        }
                    }
                }
            }
        }

        private void SetImages()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string path = Path.Combine(docPath, @"JsonFiles\Images\Images.json");

            string json = File.ReadAllText(path);

            var jsonObject = JsonSerializer.Deserialize<Images>(json);

            Images = jsonObject;
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
