using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class ResultViewModel : BaseViewModel
    {
        #region InfoClasses

        public class ResultInfo
        {
            public TeamInfo Team { get; set; }
            public PlayerInfo[] Players { get; set; }
            public PlayerInfo Player1 { get; set; }
            public PlayerInfo Player2 { get; set; }
            public PlayerInfo Player3 { get; set; }

            public ResultInfo(TeamInfo team, PlayerInfo[] players)
            {
                Team = team;
                Players = players;
                Player1 = GetBestPlayer(ref players);
                Player2 = GetBestPlayer(ref players);
                Player3 = GetBestPlayer(ref players);
            }

            private PlayerInfo GetBestPlayer(ref PlayerInfo[] temp)
            {
                PlayerInfo player = new PlayerInfo();
                int index = 0;
                player.Points = 0;

                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] != null && temp[i].Points > player.Points)
                    {
                        player = temp[i];
                        index = i;
                    }
                }
                temp[index] = null;
                return player;
            }
        }
        #endregion

        #region Properties

        private ResultInfo _resultData;
        public ResultInfo ResultData
        {
            get { return _resultData; }
            set
            {
                _resultData = value;
                OnPropertyChanged();
            }
        }

        // Property für ListView
        public ObservableCollection<ResultInfo> InfoList { get; set; }
        #endregion

        #region Attributes

        private Window _window;
        private User _user;
        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        #endregion

        public ResultViewModel(Window window, TeamInfo teamData, PlayerInfo[] playerData)
        {
            _window = window;
            _user = new User();
            ResultData = new ResultInfo(teamData, playerData);
            InfoList = new ObservableCollection<ResultInfo>();
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
