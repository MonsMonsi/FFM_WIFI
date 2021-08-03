using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataViewModel;
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
        public ObservableCollection<Info.Team> ActiveTeamList { get; set; }
        private Info.Team _selectedActiveTeam;
        public Info.Team SelectedActiveTeam
        {
            get { return _selectedActiveTeam; }
            set
            {
                _selectedActiveTeam = value;
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
                _pdf.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Info.Team> ClassicTeamList { get; set; }
        private Info.Team _selectedClassicTeam;
        public Info.Team SelectedClassicTeam
        {
            get { return _selectedClassicTeam; }
            set
            {
                _selectedClassicTeam = value;
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
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
            ActiveTeamList = new ObservableCollection<Info.Team>();
            ClassicTeamList = new ObservableCollection<Info.Team>();
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
            DraftWindow dWindow = new DraftWindow(_selectedActiveTeam.UserTeam);
            dWindow.ShowDialog();

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
                            ActiveTeamList.Add(new Info.Team(u.UserTeamPk, u.UserTeamUserFkNavigation.UserPk, u.UserTeamName, new GetFrom().Image(u.UserTeamLogo),
                                u.UserTeamPlayday, u.UserTeamNumberPlayers, new GetFrom().Image(league.LeagueLogo), season.SeasonName, points, u, performance, Images));
                        }
                        else
                        {
                            ClassicTeamList.Add(new Info.Team(u.UserTeamPk, u.UserTeamUserFkNavigation.UserPk, u.UserTeamName, new GetFrom().Image(Images.Star),
                                u.UserTeamPlayday, u.UserTeamNumberPlayers, new GetFrom().Image(league.LeagueLogo), season.SeasonName, points, u, performance, Images));
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
