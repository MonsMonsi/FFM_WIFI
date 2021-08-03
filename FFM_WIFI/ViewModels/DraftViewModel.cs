using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataViewModel;
using FFM_WIFI.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class DraftViewModel : BaseViewModel
    {
        #region Properties

        // Properties für User
        // Datenstruktur überdenken
        private _userTeam _userTeam;
        public _userTeam UserTeam
        {
            get { return _userTeam; }
            set
            {
                _userTeam = value;
                OnPropertyChanged();
            }
        }

        // Properties für Draft
        private Info.Draft[] _draftedTeam;
        public Info.Draft[] DraftedTeam
        {
            get { return _draftedTeam; }
            set
            {
                _draftedTeam = value;
            }
        }

        // Property für Progressbar
        private double _moneyMax;
        public double MoneyMax
        {
            get { return _moneyMax; }
            set
            {
                _moneyMax = value;
                OnPropertyChanged();
            }
        }

        // Properties für TextBlock
        private string _draftText;
        public string DraftText
        {
            get { return _draftText; }
            set
            {
                _draftText = value;
                OnPropertyChanged();
            }
        }

        // Properties für Datagrid
        public ObservableCollection<Info.Draft> PlayerList { get; set; }
        private Info.Draft _selectedPlayer;
        public Info.Draft SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged();
                _draft.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Attributes

        private Season _season;
        private League _league;
        private List<Info.Draft> _allPlayers;
        private string _position;
        private int _draftIndex;
        private int _draftCount;
        private Window _window;
        #endregion

        #region Commands

        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } }
        private RelayCommand<object> _sub;
        public ICommand SubCommand { get { return _sub; } }
        private RelayCommand _save;
        public ICommand SaveCommand { get { return _save; } }
        #endregion

        // Konstruktor
        public DraftViewModel(Window window, _userTeam userTeam)
        {
            // Attribute setzen
            _window = window;
            UserTeam = userTeam;
            MoneyMax = 300;
            DraftedTeam = new Info.Draft[17];
            _allPlayers = new List<Info.Draft>();
            _draftIndex = GetDraftIndex();
            _draftCount = GetDraftCount();
            SetPosition();
            SetSeasonLeague();
            // Commands
            _draft = new RelayCommand(DraftPlayer, () => SelectedPlayer != null && GetDraftIndex() < 17);
            _sub = new RelayCommand<object>(SubPlayer);
            _save = new RelayCommand(SetUserTeam, () => GetDraftIndex() == 17);
            PlayerList = new ObservableCollection<Info.Draft>();
            ShowPlayers();
            SetDraftText();
        }

        // noch zu implemetieren




        // 2: Neustart Button

        #region Methods
        private void GoToUserHome(User user)
        {
            UserHomeWindow uhWindow = new UserHomeWindow(user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void DraftPlayer()
        {
            if (SelectedPlayer != null)
            {
                _draftedTeam[_draftIndex] = _selectedPlayer;
                MoneyMax -= _selectedPlayer.Value;  // _moneyMax funktioniert nicht
                OnPropertyChanged("DraftedTeam");
                _draft.RaiseCanExecuteChanged();
            }
            ShowPlayers();
        }

        private void SubPlayer(object position)
        {
            string t = position.ToString();
            int i = int.Parse(t);

            if (_draftedTeam[i] != null)
            {
                MoneyMax += _draftedTeam[i].Value;
                _draftedTeam[i] = null;
                OnPropertyChanged("DraftedTeam");
            }
            ShowPlayers();
        }

        private void SetSeasonLeague()
        {
            using (FootballContext context = new FootballContext())
            {
                var season = context.Season.Where(s => s.SeasonPk == _userTeam.UserTeamSeason).FirstOrDefault();
                var league = context.League.Where(l => l.LeaguePk == _userTeam.UserTeamLeague).FirstOrDefault();

                _season = season;
                _league = league;
            }
        }

        private int GetDraftIndex()
        {
            for (int i = 0; i < _draftedTeam.Length; i++)
            {
                if (_draftedTeam[i] == null)
                    return i;
            }
            return _draftedTeam.Length;
        }

        private int GetDraftCount()
        {
            int count = 0;
            for (int i = 0; i < _draftedTeam.Length; i++)
            {
                if (_draftedTeam[i] == null)
                    count++;
            }
            return count;
        }

        private void ShowPlayers()
        {
            _draftIndex = GetDraftIndex();
            _draftCount = GetDraftCount();
            _save.RaiseCanExecuteChanged();
            SetPosition();
            SetDraftText();
            PlayerList.Clear();

            if (_allPlayers.Count == 0)
            {
                using (FootballContext context = new FootballContext())
                {
                    var players = context.TeamPlayerAssignment.Where(p => p.TeaPlaTeamFkNavigation.SeaLeaTeaSeasonFk == _season.SeasonPk
                                                                     && p.TeaPlaTeamFkNavigation.SeaLeaTeaLeagueFk == _league.LeaguePk)
                                                                     .Include(p => p.TeaPlaPlayerFkNavigation);

                    foreach (var p in players)
                    {
                        Info.Draft temp = new Info.Draft(p.TeaPlaPlayerFkNavigation.PlayerPk, p.TeaPlaPlayerFkNavigation.PlayerFirstName + " " + p.TeaPlaPlayerFkNavigation.PlayerLastName,
                                                       p.TeaPlaPlayerFkNavigation.PlayerImage, p.TeaPlaPlayerFkNavigation.PlayerPosition, p.TeaPlaPlayerValue / 1000000);

                        if (!_allPlayers.Contains(temp))
                            _allPlayers.Add(temp);
                    }
                }
            }

            foreach (var player in _allPlayers)
            {
                if (player.Position == _position && player.Value < _moneyMax && !_draftedTeam.Contains(player))
                    PlayerList.Add(player);
            }
            // PlayerList.BubbleSort();
        }

        private void SetDraftText()
        {
            if (_draftCount > 0)
                DraftText = $"Bitte wähle einen {_position}! Du hast noch {_draftCount} Drafts";
            else
                DraftText = $"Dein Team ist komplett!\nDu kannst es nun speichern";
        }

        private void SetPosition()
        {
            switch (_draftIndex)
            {
                case 0:
                case 11:
                    _position = "Goalkeeper";
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 12:
                    _position = "Defender";
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 13:
                case 14:
                    _position = "Midfielder";
                    break;
                case 9:
                case 10:
                case 15:
                case 16:
                    _position = "Attacker";
                    break;
            }
        }

        private void SetUserTeam()
        {
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamUserFk == UserTeam.UserTeamUserFkNavigation.UserPk && u.UserTeamName == UserTeam.UserTeamName).Include(u => u.UserTeamUserFkNavigation).FirstOrDefault();

                if (userTeam != null)
                {
                    // Player PK hinterlegen
                    userTeam.UserTeamGk1 = _draftedTeam[0].Id;
                    userTeam.UserTeamDf1 = _draftedTeam[1].Id;
                    userTeam.UserTeamDf2 = _draftedTeam[2].Id;
                    userTeam.UserTeamDf3 = _draftedTeam[3].Id;
                    userTeam.UserTeamDf4 = _draftedTeam[4].Id;
                    userTeam.UserTeamMf1 = _draftedTeam[5].Id;
                    userTeam.UserTeamMf2 = _draftedTeam[6].Id;
                    userTeam.UserTeamMf3 = _draftedTeam[7].Id;
                    userTeam.UserTeamMf4 = _draftedTeam[8].Id;
                    userTeam.UserTeamAt1 = _draftedTeam[9].Id;
                    userTeam.UserTeamAt2 = _draftedTeam[10].Id;
                    userTeam.UserTeamGk2 = _draftedTeam[11].Id;
                    userTeam.UserTeamDf5 = _draftedTeam[12].Id;
                    userTeam.UserTeamMf5 = _draftedTeam[13].Id;
                    userTeam.UserTeamMf6 = _draftedTeam[14].Id;
                    userTeam.UserTeamAt3 = _draftedTeam[15].Id;
                    userTeam.UserTeamAt4 = _draftedTeam[16].Id;
                    userTeam.UserTeamNumberPlayers = 17;
                }

                var userTeamPerformance = context.UserTeamPerformance.Where(u => u.UserTeamPerformanceUserTeamFk == UserTeam.UserTeamPk).FirstOrDefault();

                if (userTeamPerformance != null)
                {
                    // Punkte auf null
                    userTeamPerformance.UserTeamPerformanceGk1 = 0;
                    userTeamPerformance.UserTeamPerformanceDf1 = 0;
                    userTeamPerformance.UserTeamPerformanceDf2 = 0;
                    userTeamPerformance.UserTeamPerformanceDf3 = 0;
                    userTeamPerformance.UserTeamPerformanceDf4 = 0;
                    userTeamPerformance.UserTeamPerformanceMf1 = 0;
                    userTeamPerformance.UserTeamPerformanceMf2 = 0;
                    userTeamPerformance.UserTeamPerformanceMf3 = 0;
                    userTeamPerformance.UserTeamPerformanceMf4 = 0;
                    userTeamPerformance.UserTeamPerformanceAt1 = 0;
                    userTeamPerformance.UserTeamPerformanceAt2 = 0;
                    userTeamPerformance.UserTeamPerformanceGk2 = 0;
                    userTeamPerformance.UserTeamPerformanceDf5 = 0;
                    userTeamPerformance.UserTeamPerformanceMf5 = 0;
                    userTeamPerformance.UserTeamPerformanceMf6 = 0;
                    userTeamPerformance.UserTeamPerformanceAt3 = 0;
                    userTeamPerformance.UserTeamPerformanceAt4 = 0;
                }

                var user = context.User.Where(u => u.UserPk == UserTeam.UserTeamUserFkNavigation.UserPk).FirstOrDefault();

                context.SaveChanges();
                GoToUserHome(user);
            }
        }
        #endregion
    }
}


