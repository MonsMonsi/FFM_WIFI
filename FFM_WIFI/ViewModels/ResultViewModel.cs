using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
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
        private Info.Result _resultData;
        public Info.Result ResultData
        {
            get { return _resultData; }
            set
            {
                _resultData = value;
                OnPropertyChanged();
            }
        }

        // Property für ListView
        public ObservableCollection<Info.Result> InfoList { get; set; }
        #endregion

        #region Attributes

        private Window _window;
        private User _user;
        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        #endregion

        public ResultViewModel(Window window, Info.Team teamData, Info.Player[] playerData)
        {
            _window = window;
            _user = new User();
            ResultData = new Info.Result(teamData, playerData);
            InfoList = new ObservableCollection<Info.Result>();
            HomeCommand = new RelayCommand(GoToUserHome);
            SetInfoList();
        }

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
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamPk == ResultData.Team.TeamId).FirstOrDefault();
                userTeam.UserTeamPlayday++;
                context.SaveChanges();
            }
        }

        private void SetUser()
        {
            using (FootballContext context = new FootballContext())
            {
                var user = context.User.Where(u => u.UserPk == ResultData.Team.UserId).FirstOrDefault();
                _user = user;
            }
        }

        private void SetInfoList()
        {
            InfoList.Add(ResultData);
        }
    }
}
