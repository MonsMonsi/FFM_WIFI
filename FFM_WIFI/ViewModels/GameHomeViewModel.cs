using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
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
    class GameHomeViewModel : BaseViewModel
    {
        #region Properties

        // Property für User
        private _userTeam _userTeam;
        public _userTeam UserTeam
        {
            get { return _userTeam; }
            set
            {
                _userTeam = value;
                OnPropertyChanged("UserTeam");
            }
        }

        // Property für TextBlock und Image
        private Info.Team _teamData;
        public Info.Team TeamData
        {
            get { return _teamData; }
            set
            {
                _teamData = value;
                OnPropertyChanged("TeamData");
            }
        }

        // Property für ListView
        public ObservableCollection<string> DetailText { get; set; }

        // Property für PlayerInfo
        public Info.Player[] PlayerInfo { get; set; }

        // Property für FixtureWindow
        public int[] PlaydayFixtures { get; set; }

        // Properties für Datagrids
        public ObservableCollection<Info.Player> DataList { get; set; }
        private Info.Player _selectedData;
        public Info.Player SelectedData
        {
            get { return _selectedData; }
            set
            {
                _selectedData = value;
                OnPropertyChanged();
                SetDetailText();
            }
        }

        public ObservableCollection<Info.Player> DraftList { get; set; }
        private Info.Player _selectedDraft;
        public Info.Player SelectedDraft
        {
            get { return _selectedDraft; }
            set
            {
                _selectedDraft = value;
                OnPropertyChanged();
                _line.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Info.Playday> _playdayList;
        public ObservableCollection<Info.Playday> PlaydayList
        {
            get { return _playdayList; }
            set
            {
                _playdayList = value;
                OnPropertyChanged();
            }
        }

        private Info.Playday _selectedPlayday;
        public Info.Playday SelectedPlayday
        {
            get { return _selectedPlayday; }
            set
            {
                _selectedPlayday = value;
                OnPropertyChanged();
            }
        }

        // Property für Canvas
        private Info.Player[] _lineUp;
        public Info.Player[] LineUp
        {
            get { return _lineUp; }
            set
            {
                _lineUp = value;
            }
        }
        #endregion

        #region Commands

        private RelayCommand<object> _sub;
        public ICommand SubCommand { get { return _sub; } }
        private RelayCommand _line;
        public ICommand LineUpCommand { get { return _line; } }
        private RelayCommand _play;
        public ICommand PlayCommand { get { return _play; } }
        private RelayCommand _save;
        public ICommand SaveCommand { get { return _save; } }
        public ICommand FastCommand { get; set; }

        #endregion

        #region Attributes

        private Window _window;
        private User _user;
        private int _playday;
        private int _lineCount;
        private string _position;

        #endregion
        // Konstruktor
        public GameHomeViewModel(Window window, Info.Team teamData, Info.Player[] playerData)
        {
            // Daten setzen Listen initialisieren
            _window = window;
            _user = new User();
            TeamData = teamData;
            UserTeam = _teamData.UserTeam;
            _playday = _userTeam.UserTeamPlayday;
            DetailText = new ObservableCollection<string>();
            LineUp = new Info.Player[11];
            _lineCount = GetLineUpIndex();
            // Spielerdaten setzen oder updaten
            PlayerInfo = new Info.Player[17];
            DraftList = new ObservableCollection<Info.Player>();
            if (playerData == null)
                SetPlayerInfo();
            else
                ResetDrafted(playerData);
            PlaydayFixtures = new int[9];
            DataList = new ObservableCollection<Info.Player>();
            PlaydayList = new ObservableCollection<Info.Playday>();
            SetPlayerDataList();
            SetPlayerDraftList();
            SetPlaydayList();
            // Commands
            _sub = new RelayCommand<object>(SubPlayer);
            _line = new RelayCommand(LineUpPlayer, () => !CheckComplete() && SelectedDraft != null);
            FastCommand = new RelayCommand(FastLineUp);
            _play = new RelayCommand(GoToFixture, () => CheckComplete() && _playday < 35);
            _save = new RelayCommand(GoToUserHome);
            _play.RaiseCanExecuteChanged();
        }

        #region Methods
        private void GoToFixture()
        {
            FixtureWindow fWindow = new FixtureWindow(TeamData, PlayerInfo);
            _window.Close();
            fWindow.ShowDialog();
        }

        private void GoToUserHome()
        {
            WriteData();
            SetUser();
            UserHomeWindow uhWindow = new UserHomeWindow(_user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void SubPlayer(object position)
        {
            string t = position.ToString();
            int i = int.Parse(t);

            if (LineUp[i] != null)
            {
                LineUp[i].Drafted = false;
                LineUp[i] = null;
                OnPropertyChanged("LineUp");
            }

            _lineCount = GetLineUpIndex();
            SetPlayerDraftList();
            _line.RaiseCanExecuteChanged();
            _play.RaiseCanExecuteChanged();
        }

        private void LineUpPlayer()
        {
            if (SelectedDraft != null)
            {
                LineUp[_lineCount] = SelectedDraft;
                DraftList.Remove(SelectedDraft);
                LineUp[_lineCount].Drafted = true;
                OnPropertyChanged("LineUp");
            }

            _lineCount = GetLineUpIndex();
            SetPlayerDraftList();
            _line.RaiseCanExecuteChanged();
            _play.RaiseCanExecuteChanged();
        }

        private void SetPlayerInfo()
        {
            using (FootballContext context = new FootballContext())
            {
                int[] players = new int[17]
                {
                    UserTeam.UserTeamGk1, UserTeam.UserTeamDf1, UserTeam.UserTeamDf2, UserTeam.UserTeamDf3, UserTeam.UserTeamDf4, UserTeam.UserTeamMf1,
                    UserTeam.UserTeamMf2, UserTeam.UserTeamMf3, UserTeam.UserTeamMf4, UserTeam.UserTeamAt1, UserTeam.UserTeamAt2, UserTeam.UserTeamGk2,
                    UserTeam.UserTeamDf5, UserTeam.UserTeamMf5, UserTeam.UserTeamMf6, UserTeam.UserTeamAt3, UserTeam.UserTeamAt4
                };

                var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == UserTeam.UserTeamPk).FirstOrDefault();
                int[] points = new int[17]
                {
                    performance.UserTeamPerformanceGk1, performance.UserTeamPerformanceDf1, performance.UserTeamPerformanceDf2, performance.UserTeamPerformanceDf3, performance.UserTeamPerformanceDf4, performance.UserTeamPerformanceMf1,
                    performance.UserTeamPerformanceMf2, performance.UserTeamPerformanceMf3, performance.UserTeamPerformanceMf4, performance.UserTeamPerformanceAt1, performance.UserTeamPerformanceAt2, performance.UserTeamPerformanceGk2,
                    performance.UserTeamPerformanceDf5, performance.UserTeamPerformanceMf5, performance.UserTeamPerformanceMf6, performance.UserTeamPerformanceAt3, performance.UserTeamPerformanceAt4
                };

                int poi = 0;
                foreach (var p in points)
                {
                    poi += p;
                }

                _teamData.Points = poi;

                for (int i = 0; i < 17; i++)
                {
                    var player = context.Player.Where(p => p.PlayerPk == players[i]).FirstOrDefault();
                    Info.Player temp = new Info.Player(players[i], player.PlayerFirstName + " " + player.PlayerLastName,
                                                       player.PlayerPosition, player.PlayerHeight, player.PlayerNationality, new GetFrom().Image(player.PlayerImage), points[i], false);

                    PlayerInfo[i] = temp;
                }
            }
        }

        private int GetLineUpIndex()
        {
            for (int i = 0; i < LineUp.Length; i++)
            {
                if (LineUp[i] == null)
                    return i;
            }
            return LineUp.Length;
        }

        private void ResetDrafted(Info.Player[] playerData)
        {
            int poi = 0;
            PlayerInfo = playerData;
            foreach (var p in PlayerInfo)
            {
                p.Drafted = false;
                poi += p.Points;
            }
            _teamData.Points = poi;
        }

        // noch schöner machen
        private void SetDetailText()
        {
            DetailText.Clear();
            DetailText.Add("");
            DetailText.Add("Punkte: " + SelectedData.Points);
            DetailText.Add("");
            DetailText.Add("Letzer Spieltag");
            DetailText.Add("");
            DetailText.Add("  LineUp  +" + SelectedData.LineUp);
            DetailText.Add("  Goal    +" + SelectedData.Goals);
            DetailText.Add("  Assist  +" + SelectedData.Assists);
            DetailText.Add("");
            DetailText.Add("  Yellow  -" + SelectedData.YellowC);
            DetailText.Add("  Red     -" + SelectedData.RedC);
            DetailText.Add("  Subst   -" + SelectedData.Subst);
        }
        private void SetPlayerDataList()
        {
            foreach (var p in PlayerInfo)
            {
                DataList.Add(p);
            }
        }

        private void SetPlayerDraftList()
        {
            SetPosition();
            DraftList.Clear();
            foreach (var p in PlayerInfo)
            {
                if (p.Position == _position && !p.Drafted)
                    DraftList.Add(p);
            }
        }

        private void SetPlaydayList()
        {
            Calculate calc = new Calculate(_playday);
            calc.PlaydayInfo();
            PlaydayList = calc.PlaydayList;
        }

        private void FastLineUp()
        {
            for (int i = 0; i < 11; i++)
            {
                if (LineUp[i] != null)
                {
                    LineUp[i].Drafted = false;
                }
            }

            for (int i = 0; i < 11; i++)
            {
                Info.Player temp = new Info.Player();
                switch (i)
                {
                    case 0:
                        foreach (var p in PlayerInfo)
                        {
                            if (p.Position == "Goalkeeper" && !p.Drafted)
                            {
                                temp = p;
                                temp.Drafted = true;
                                break;
                            }
                        }
                        LineUp[i] = temp;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        foreach (var p in PlayerInfo)
                        {
                            if (p.Position == "Defender" && !p.Drafted)
                            {
                                temp = p;
                                temp.Drafted = true;
                                break;
                            }
                        }
                        LineUp[i] = temp;
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        foreach (var p in PlayerInfo)
                        {
                            if (p.Position == "Midfielder" && !p.Drafted)
                            {
                                temp = p;
                                temp.Drafted = true;
                                break;
                            }
                        }
                        LineUp[i] = temp;
                        break;
                    case 9:
                    case 10:
                        foreach (var p in PlayerInfo)
                        {
                            if (p.Position == "Attacker" && !p.Drafted)
                            {
                                temp = p;
                                temp.Drafted = true;
                                break;
                            }
                        }
                        LineUp[i] = temp;
                        break;
                }
            }
            OnPropertyChanged("LineUp");
            _lineCount = GetLineUpIndex();
            _play.RaiseCanExecuteChanged();
        }

        private void SetPosition()
        {
            switch (_lineCount)
            {
                case 0:
                    _position = "Goalkeeper";
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    _position = "Defender";
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    _position = "Midfielder";
                    break;
                case 9:
                case 10:
                    _position = "Attacker";
                    break;
            }
        }

        private void WriteData()
        {
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamPk == UserTeam.UserTeamPk).FirstOrDefault();

                userTeam.UserTeamPlayday = UserTeam.UserTeamPlayday;

                var performance = context.UserTeamPerformance.Where(p => p.UserTeamPerformanceUserTeamFk == UserTeam.UserTeamPk).FirstOrDefault();

                performance.UserTeamPerformanceGk1 = PlayerInfo[0].Points; performance.UserTeamPerformanceAt1 = PlayerInfo[9].Points;
                performance.UserTeamPerformanceDf1 = PlayerInfo[1].Points; performance.UserTeamPerformanceAt2 = PlayerInfo[10].Points;
                performance.UserTeamPerformanceDf2 = PlayerInfo[2].Points; performance.UserTeamPerformanceGk2 = PlayerInfo[11].Points;
                performance.UserTeamPerformanceDf3 = PlayerInfo[3].Points; performance.UserTeamPerformanceDf5 = PlayerInfo[12].Points;
                performance.UserTeamPerformanceDf4 = PlayerInfo[4].Points; performance.UserTeamPerformanceMf5 = PlayerInfo[13].Points;
                performance.UserTeamPerformanceMf1 = PlayerInfo[5].Points; performance.UserTeamPerformanceMf6 = PlayerInfo[14].Points;
                performance.UserTeamPerformanceMf2 = PlayerInfo[6].Points; performance.UserTeamPerformanceAt3 = PlayerInfo[15].Points;
                performance.UserTeamPerformanceMf3 = PlayerInfo[7].Points; performance.UserTeamPerformanceAt4 = PlayerInfo[16].Points;
                performance.UserTeamPerformanceMf4 = PlayerInfo[8].Points;

                context.SaveChanges();
            }
        }

        private void SetUser()
        {
            using (FootballContext context = new FootballContext())
            {
                var user = context.User.Where(u => u.UserPk == UserTeam.UserTeamUserFk).FirstOrDefault();
                _user = user;
            }
        }

        private bool CheckComplete()
        {
            for (int i = 0; i < 11; i++)
            {
                if (LineUp[i] == null)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
