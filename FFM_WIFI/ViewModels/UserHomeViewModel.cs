using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.Models.Utility;
using FFM_WIFI.Views;
using System.Collections.ObjectModel;
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
        private GetFrom.Database _getFromDatabase;
        #endregion
        // Konstruktor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            ActiveTeamList = new ObservableCollection<Info.Team>();
            ClassicTeamList = new ObservableCollection<Info.Team>();
            User = user;
            _window = window;
            _getFromDatabase = new GetFrom.Database(_user);
            //EditDBCommand = new RelayCommand(GoToEditDatabase);
            NewTeamCommand = new RelayCommand(GoToNewTeam);
            _draft = new RelayCommand(GoToDraft, () => SelectedActiveTeam != null && SelectedActiveTeam.Players != 17);
            _game = new RelayCommand(GoToGameHome, () => SelectedActiveTeam != null && SelectedActiveTeam.Players == 17);
            _pdf = new RelayCommand(SaveAsPdf, () => SelectedClassicTeam != null);
            SetTeamLists();
        }

        #region Methods
        private void GoToNewTeam()
        {
            NewTeamWindow ntwindow = new NewTeamWindow(User);
            _window.Close();
            ntwindow.ShowDialog();
        }

        private void GoToDraft()
        {
            DraftWindow dWindow = new DraftWindow(SelectedActiveTeam.UserTeam);
            dWindow.ShowDialog();

        }

        private void GoToGameHome()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(SelectedActiveTeam);
            ghWindow.ShowDialog();
        }

        private void SaveAsPdf()
        {

        }

        private void SetTeamLists()
        {
            // ineffektiv?
            foreach (var team in _getFromDatabase.TeamInfo())
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
