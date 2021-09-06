using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
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

        // Team ListViews
        public ObservableCollection<Info.Team> ActiveTeamList { get; set; }
        private Info.Team _selectedActiveTeam;
        public Info.Team SelectedActiveTeam
        {
            get { return _selectedActiveTeam; }
            set
            {
                _selectedActiveTeam = value;
                SetUserRankList();
                _delete.RaiseCanExecuteChanged();
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
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
                _delete.RaiseCanExecuteChanged();
                _draft.RaiseCanExecuteChanged();
                _game.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        // UserRank Datagrid
        private ObservableCollection<UserTeam> _userRankList;
        public ObservableCollection<UserTeam> UserRankList
        {
            get { return _userRankList; }
            set
            {
                _userRankList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ICommand NewTeamCommand { get; set; } // Weiter zum NewTeamWindow

        private RelayCommand _delete;
        public ICommand DeleteCommand { get { return _delete; } } // Löscht bestehendes Team

        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } } // Weiter zum DraftWindow

        private RelayCommand _game;
        public ICommand GameCommand { get { return _game; } } // Weiter zum GameHomeWindow
        public ICommand StartCommand { get; set; } // Zurück zur Anmeldung
        #endregion

        #region Attributes
        private Window _window;
        private GetFrom.Database _get;
        #endregion

        #region Constructor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            ActiveTeamList = new ObservableCollection<Info.Team>();
            ClassicTeamList = new ObservableCollection<Info.Team>();
            User = user;
            _window = window;
            _get = new GetFrom.Database(_user);
            NewTeamCommand = new RelayCommand(GoToNewTeam);
            StartCommand = new RelayCommand(GoToStart);
            _delete = new RelayCommand(DeleteTeam, () => SelectedActiveTeam != null || SelectedClassicTeam != null);
            _draft = new RelayCommand(GoToDraft, () => SelectedActiveTeam != null && SelectedActiveTeam.Players != 17);
            _game = new RelayCommand(GoToGameHome, () => SelectedActiveTeam != null && SelectedActiveTeam.Players == 17);
            SetTeamLists();
        }
        #endregion

        #region Methods
        private void GoToStart()
        {
            StartWindow sWindow = new StartWindow();
            _window.Close();
            sWindow.ShowDialog();
        }

        private void GoToNewTeam()
        {
            NewTeamWindow ntWindow = new NewTeamWindow(User);
            _window.Close();
            ntWindow.ShowDialog();
        }

        private void GoToDraft()
        {
            DraftWindow dWindow = new DraftWindow(SelectedActiveTeam.UserTeam);
            _window.Close();
            dWindow.ShowDialog();
        }

        private void GoToGameHome()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(SelectedActiveTeam);
            _window.Close();
            ghWindow.ShowDialog();
        }

        private void SetTeamLists()
        {
            // Befüllt die Team-Listviews; GetFrom-Klasse: s. Utility/GetFrom
            ClassicTeamList.Clear();
            ActiveTeamList.Clear();
            foreach (var team in _get.TeamInfo())
            {
                if (team.Playday > 34)
                {
                    ClassicTeamList.Add(team);
                }
                else
                {
                    ActiveTeamList.Add(team);
                }
            }
        }

        private void SetUserRankList()
        {
            // Befüllt das UserRank-Datagrid: Vergleich von Teams unterschiedlicher Benutzer
            if (SelectedActiveTeam != null)
            {
                _get.UserTeam = SelectedActiveTeam.UserTeam;
                UserRankList = new(_get.UserRank());
            }
        }

        private void DeleteTeam()
        {
            // Löscht das ausgewählte Team
            using (FootballContext context = new FootballContext())
            {
                var team = context.UserTeam.Where(t => t.UserTeamPk == SelectedActiveTeam.UserTeam.UserTeamPk).FirstOrDefault();
                var performance = context.UserTeamPerformance.Where(t => t.UserTeamPerformanceUserTeamFk == SelectedActiveTeam.UserTeam.UserTeamPk).FirstOrDefault();

                context.Remove(performance);
                context.Remove(team);
                context.SaveChanges();
                SetTeamLists();
            }
        }
        #endregion
    }
}
