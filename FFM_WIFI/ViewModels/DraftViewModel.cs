using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FFM_WIFI.Models.Utility;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using FFM_WIFI.Views;

namespace FFM_WIFI.ViewModels
{
    class DraftViewModel : BaseViewModel
    {
        // Properties für User
        // Datenstruktur überdenken
        private UserTeam _userTeam;
        public UserTeam UserTeam
        {
            get { return _userTeam; }
            set
            {
                _userTeam = value;
                OnPropertyChanged("UserTeam");
            }
        }

        private Player[] _teamUser;
        public Player[] TeamUser
        {
            get { return _teamUser; }
            set
            {
                _teamUser = value;
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
                OnPropertyChanged("DraftText");
            }
        }

        // Properties für Datagrid
        public ObservableCollection<Player> PlayerList { get; set; }
        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");
                _draft.RaiseCanExecuteChanged();
            }
        }

        // Attribute
        private Season _season;
        private League _league;
        private string _position;
        private bool _positionChanged;
        private int _draftCount;
        private Window _window;

        // Commands
        private RelayCommand _draft;
        public ICommand DraftCommand { get { return _draft; } }
        private RelayCommand _save;
        public ICommand SaveCommand { get { return _save; } }

        // Konstruktor
        public DraftViewModel(Window window, UserTeam userTeam)
        {
            // Attribute setzen
            _window = window;
            _userTeam = userTeam;
            _draftCount = 0;
            _position = "Goalkeeper";
            _positionChanged = false;
            SetSeasonLeague();
            // Commands
            _draft = new RelayCommand(DraftPlayer, () => SelectedPlayer != null && _draftCount != 17);
            _save = new RelayCommand(SetUserTeam, () => _draftCount > 16);
            // Listen initialisieren
            _teamUser = new Player[17];
            PlayerList = new ObservableCollection<Player>();
            ShowPlayers();
            SetDraftText();
        }

        // noch zu implemetieren
        // 1: Undo Button
        // 2: Neustart Button
        // 3: Marktwerte ausgeben und Maximalbudget hinzufügen

        private void GoToUserHome(User user)
        {
            UserHomeWindow uhWindow = new UserHomeWindow(user);
            uhWindow.ShowDialog();
            
        }

        private void SetUserTeam()
        {
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamUserFk == UserTeam.UserTeamUserFkNavigation.UserPk).Include(u => u.UserTeamUserFkNavigation).FirstOrDefault();

                if (userTeam != null)
                {
                    // Player PK hinterlegen
                    userTeam.UserTeamGk1 = _teamUser[0].PlayerPk;
                    userTeam.UserTeamDf1 = _teamUser[1].PlayerPk;
                    userTeam.UserTeamDf2 = _teamUser[2].PlayerPk;
                    userTeam.UserTeamDf3 = _teamUser[3].PlayerPk;
                    userTeam.UserTeamDf4 = _teamUser[4].PlayerPk;
                    userTeam.UserTeamMf1 = _teamUser[5].PlayerPk;
                    userTeam.UserTeamMf2 = _teamUser[6].PlayerPk;
                    userTeam.UserTeamMf3 = _teamUser[7].PlayerPk;
                    userTeam.UserTeamMf4 = _teamUser[8].PlayerPk;
                    userTeam.UserTeamAt1 = _teamUser[9].PlayerPk;
                    userTeam.UserTeamAt2 = _teamUser[10].PlayerPk;
                    userTeam.UserTeamGk2 = _teamUser[11].PlayerPk;
                    userTeam.UserTeamDf5 = _teamUser[12].PlayerPk;
                    userTeam.UserTeamMf5 = _teamUser[13].PlayerPk;
                    userTeam.UserTeamMf6 = _teamUser[14].PlayerPk;
                    userTeam.UserTeamAt3 = _teamUser[15].PlayerPk;
                    userTeam.UserTeamAt4 = _teamUser[16].PlayerPk;
                    userTeam.UserTeamNumberPlayers = 17;
                }

                var user = context.User.Where(u => u.UserPk == UserTeam.UserTeamUserFkNavigation.UserPk).FirstOrDefault();

                context.SaveChanges();
                GoToUserHome(user);
            }
        }

        private void DraftPlayer()
        {
            if (SelectedPlayer != null)
            {
                SetPosition();
                SetDraftText();
                _teamUser[_draftCount] = _selectedPlayer;
                OnPropertyChanged("TeamUser");
                PlayerList.Remove(_selectedPlayer);
                _draftCount++;

                if (_draftCount == 17)
                {
                    _draft.RaiseCanExecuteChanged();
                    _save.RaiseCanExecuteChanged();
                }

                ShowPlayers();
            }
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

        private void SetPosition()
        {
            switch (_draftCount)
            {
                case 0:
                    _position = "Goalkeeper";
                    _positionChanged = true;
                    break;
                case 1:
                    _position = "Defender";
                    _positionChanged = true;
                    break;
                case 5:
                    _position = "Midfielder";
                    _positionChanged = true;
                    break;
                case 9:
                    _position = "Attacker";
                    _positionChanged = true;
                    break;
                case 11:
                    _position = "Goalkeeper";
                    _positionChanged = true;
                    break;
                case 12:
                    _position = "Defender";
                    _positionChanged = true;
                    break;
                case 13:
                    _position = "Midfielder";
                    _positionChanged = true;
                    break;
                case 15:
                    _position = "Attacker";
                    _positionChanged = true;
                    break;
            }
        }

        private void ShowPlayers()
        {
            SetPosition();
            if (_positionChanged)
            {
                PlayerList.Clear();
                using (FootballContext context = new FootballContext())
                {
                    var players = context.TeamPlayerAssignment.Where(p => p.TeaPlaTeamFkNavigation.SeaLeaTeaSeasonFk == _season.SeasonPk && p.TeaPlaTeamFkNavigation.SeaLeaTeaLeagueFk == _league.LeaguePk && p.TeaPlaPlayerFkNavigation.PlayerPosition == _position).Include(p => p.TeaPlaPlayerFkNavigation).Select(p => p.TeaPlaPlayerFkNavigation);

                    foreach (var item in players)
                    {
                        Player temp = new Player();
                        temp = item;
                        PlayerList.Add(item);
                    }
                }
                _positionChanged = false;
            }
        }

        private void SetDraftText()
        {
            DraftText = $"{UserTeam.UserTeamUserFkNavigation.UserName}, bitte wähle einen {_position}!\nDu hast noch {17 - _draftCount} Drafts";
            
        }
    }
}


