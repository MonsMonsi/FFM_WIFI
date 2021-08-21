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
        // Property für TextBlock und Image
        private Info.Team _teamInfo;
        public Info.Team TeamInfo
        {
            get { return _teamInfo; }
            set
            {
                _teamInfo = value;
                OnPropertyChanged();
            }
        }

        // Property für DraftList 
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

        // Property für FixtureWindow
        public ObservableCollection<Info.Playday> PlaydayList { get; set; }

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

        // Property für Zeitangabe
        private string _untilPlayday;
        public string UntilPlayday
        {
            get { return _untilPlayday; }
            set
            {
                _untilPlayday = value;
                OnPropertyChanged();
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
        private GetFrom.Database _get;
        private WriteTo.Database _write;
        private Info.Player[] _playerInfo;
        private int _playday;
        private TimeSpan _time;
        private int _lineCount;
        private string _position;

        #endregion
        // Konstruktor
        public GameHomeViewModel(Window window, Info.Team teamInfo, Info.Player[] playerInfo)
        {
            TeamInfo = teamInfo;
            LineUp = new Info.Player[11];
            _playerInfo = new Info.Player[17];
            DraftList = new ObservableCollection<Info.Player>();
            _get = new GetFrom.Database(null, TeamInfo.UserTeam);
            _playday = TeamInfo.UserTeam.UserTeamPlayday;
            _lineCount = GetLineUpIndex();
            // Spielerdaten setzen oder updaten
            if (playerInfo == null)
            {
                SetPlayerInfo();
            }
            else
            {
                _playerInfo = playerInfo;
                ResetDrafted();
            }
            _write = new WriteTo.Database(TeamInfo.UserTeam, _playerInfo);
            PlaydayList = new ObservableCollection<Info.Playday>();
            SetPlayerDraftList();
            SetPlaydayList();
            if (!CheckPlaydayComplete())
            {
                SetPlaydayDate();
            }
            // Commands
            _sub = new RelayCommand<object>(SubPlayer);
            _line = new RelayCommand(LineUpPlayer, () => !CheckLineUpComplete() && SelectedDraft != null);
            FastCommand = new RelayCommand(FastLineUp);
            _play = new RelayCommand(GoToFixture, () => CheckLineUpComplete() && CheckPlaydayComplete());
            _save = new RelayCommand(GoToUserHome);
            _play.RaiseCanExecuteChanged();
            _window = window;
        }

        #region Methods
        private void GoToFixture()
        {
            FixtureWindow fWindow = new FixtureWindow(TeamInfo, _playerInfo);
            _window.Close();
            fWindow.ShowDialog();
        }

        private void GoToUserHome()
        {
            WriteData();
            UserHomeWindow uhWindow = new UserHomeWindow(_get.User);
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
            _playerInfo = _get.PlayerInfo();
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

        private void ResetDrafted()
        {
            int poi = 0;
            foreach (var p in _playerInfo)
            {
                p.Drafted = false;
                poi += p.Points;
            }
            TeamInfo.Points = poi;
        }

        private void SetPlayerDraftList()
        {
            SetPosition();
            DraftList.Clear();
            foreach (var p in _playerInfo)
            {
                if (p.Position == _position && !p.Drafted)
                    DraftList.Add(p);
            }
        }

        private void SetPlaydayList()
        {
            Calculate calc = new Calculate(_playday, TeamInfo.UserTeam.UserTeamLeague, TeamInfo.Season);
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
                        foreach (var p in _playerInfo)
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
                        foreach (var p in _playerInfo)
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
                        foreach (var p in _playerInfo)
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
                        foreach (var p in _playerInfo)
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
            _write.UserTeamPerformance();
        }

        private void SetPlaydayDate()
        {
            var playday = PlaydayList.Last();
            var now = DateTime.Now;
            _time = playday.Date - now;

            if (_time.Days > 0 && _time.Hours > 0 && _time.Minutes > 0)
            {
                UntilPlayday = $"Noch {_time.Days} Tage und {_time.Hours} Stunden!";
            }
        }

        private bool CheckPlaydayComplete()
        {
            foreach (var p in PlaydayList)
            {
                if (p.Status != "Match Finished")
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckLineUpComplete()
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
