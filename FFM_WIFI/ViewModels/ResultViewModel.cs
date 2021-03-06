using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class ResultViewModel : BaseViewModel
    {
        #region Properties
        private Info.Result _resultInfo;
        public Info.Result ResultInfo
        {
            get { return _resultInfo; }
            set
            {
                _resultInfo = value;
                OnPropertyChanged();
            }
        }

        // Property für ListView
        public ObservableCollection<Info.Result> ResultList { get; set; }

        public ObservableCollection<Info.Standings> StandingsList { get; set; }
        #endregion

        #region Attributes
        private Window _window;
        private User _user;
        #endregion

        #region Commands
        public ICommand HomeCommand { get; set; } // Zurück zum UserHomeWindow
        #endregion

        #region Constructor
        public ResultViewModel(Window window, Info.Team teamInfo, Info.Player[] playerInfo)
        {
            _window = window;
            _user = new User();
            ResultInfo = new Info.Result(teamInfo, playerInfo);
            ResultList = new ObservableCollection<Info.Result>();
            HomeCommand = new RelayCommand(GoToUserHome);
            SetLists();
        }
        #endregion

        #region Methods
        private void GoToUserHome()
        {
            SetUser();
            WriteData();
            UserHomeWindow uhWindow = new UserHomeWindow(_user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void WriteData()
        {
            // erhöht den gespeicherten Spieltag um eins, dadurch wird die Saison als beendet markiert
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamPk == ResultInfo.Team.TeamId).FirstOrDefault();
                userTeam.UserTeamPlayday = ResultInfo.Team.Playday;
                context.SaveChanges();
            }
        }

        private void SetUser()
        {
            // Holt die Userdaten aus der Datenbank
            using (FootballContext context = new FootballContext())
            {
                var user = context.User.Where(u => u.UserPk == ResultInfo.Team.UserId).FirstOrDefault();
                _user = user;
            }
        }

        private void SetLists()
        {
            // Füllt die Result-ListView
            ResultList.Add(ResultInfo);
            StandingsList = new ObservableCollection<Info.Standings>(ResultInfo.StandingsInfo);
        }
        #endregion
    }
}
