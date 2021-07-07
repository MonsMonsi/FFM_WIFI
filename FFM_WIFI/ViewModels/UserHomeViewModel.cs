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
                UserPlayerList.Clear();
                ShowUserPlayer();
                OnPropertyChanged("SelectedUserTeam");
            }
        }
        public ObservableCollection<Player> UserPlayerList { get; set; }

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
            }
        }

        // Attribute
        private UserTeam _userTeam;

        // Commands
        public ICommand NewTeamCommand { get; set; }

        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } }
        private RelayCommand _game;
        public ICommand GameCommand { get { return _game; } }

        // Konstruktor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            //Daten initialisieren
            _selectedLeague = null;
            _selectedSeason = null;
            // Commands
            //EditDBCommand = new RelayCommand(GoToEditDatabase);
            NewTeamCommand = new RelayCommand(GoToNewTeam);
            _draft = new RelayCommand(GoToDraft, () => SelectedUserTeam != null && SelectedUserTeam.UserTeamNumberPlayers != 17 && SelectedUserTeam.UserTeamName != "Keine Teams gefunden!");
            _game = new RelayCommand(GoToGame, () => SelectedUserTeam != null && SelectedUserTeam.UserTeamNumberPlayers == 17 && SelectedUserTeam.UserTeamName != "Keine Teams gefunden!");
            // User
            User = user;
            _userTeam = null;
            // Listen initialisieren und füllen
            UserTeamList = new ObservableCollection<UserTeam>();
            UserPlayerList = new ObservableCollection<Player>();
            SeasonList = new ObservableCollection<Season>();
            LeagueList = new ObservableCollection<League>();
            GetUserTeam();
            GetLeagueSeason();
        }

        private void GoToNewTeam()
        {
            NewTeamWindow ntwindow = new NewTeamWindow(_user);
            ntwindow.ShowDialog();
        }

        private void GoToDraft()
        {
            DraftWindow dWindow = new DraftWindow(_userTeam);
            dWindow.ShowDialog();

        }

        private void GoToGame()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(_user);
            ghWindow.ShowDialog();
        }

        private void ShowUserPlayer()
        {
            using (FootballContext context = new FootballContext())
            {
                _userTeam = SelectedUserTeam;
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();

                int?[] players = new int?[]
                {
                    SelectedUserTeam.UserTeamGk1, SelectedUserTeam.UserTeamDf1, SelectedUserTeam.UserTeamDf2, SelectedUserTeam.UserTeamDf3, SelectedUserTeam.UserTeamDf4, SelectedUserTeam.UserTeamMf1,
                    SelectedUserTeam.UserTeamMf2, SelectedUserTeam.UserTeamMf3, SelectedUserTeam.UserTeamMf4, SelectedUserTeam.UserTeamAt1, SelectedUserTeam.UserTeamAt2, SelectedUserTeam.UserTeamGk2,
                    SelectedUserTeam.UserTeamDf5, SelectedUserTeam.UserTeamMf5, SelectedUserTeam.UserTeamMf6, SelectedUserTeam.UserTeamAt3, SelectedUserTeam.UserTeamAt4
                };

                // UserPlayerList.Clear();
                foreach (var p in players)
                {
                    if (p != null)
                    {
                        var player = context.Player.Where(pl => pl.PlayerPk == p).FirstOrDefault();
                        UserPlayerList.Add(player);
                    }
                }
            }
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

        // Zugang zur Datenbank
        //private void GoToEditDatabase()
        //{
        //    EditDatabaseWindow edbWindow = new EditDatabaseWindow();
        //    edbWindow.ShowDialog();
        //}

    }
}
