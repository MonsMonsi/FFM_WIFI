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
    class DraftViewModelOld : BaseViewModel
    {
        // Properties für User
        // Datenstruktur überdenken
        private User _user1;
        public User User1
        {
            get { return _user1; }
            set
            {
                _user1 = value;
                OnPropertyChanged("User1");
            }
        }

        private User _user2;
        public User User2
        {
            get { return _user2; }
            set
            {
                _user2 = value;
                OnPropertyChanged("User2");
            }
        }

        private Player[] _teamUser1;
        public Player[] TeamUser1
        {
            get { return _teamUser1; }
            set
            {
                _teamUser1 = value;
            }
        }

        private Player[] _teamUser2;
        public Player[] TeamUser2
        {
            get { return _teamUser2; }
            set
            {
                _teamUser2 = value;
            }
        }

        // Properties für TextBlock
        private string _draftTextUser1;
        public string DraftTextUser1
        {
            get { return _draftTextUser1; }
            set
            {
                _draftTextUser1 = value;
                OnPropertyChanged("DraftTextUser1");
            }
        }

        private string _draftTextUser2;
        public string DraftTextUser2
        {
            get { return _draftTextUser2; }
            set
            {
                _draftTextUser2 = value;
                OnPropertyChanged("DraftTextUser2");
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
            }
        }

        // Attribute
        private int _indexUser1;
        private int _indexUser2;
        private bool _turnUser1;
        private bool _turnUser2;
        private League _league;
        private Season _season;
        private string _position;
        private bool _positionChanged;
        private int _draftCount;

        // Commands
        private RelayCommand _draftUser1;
        public ICommand DraftUser1Command { get { return _draftUser1; } }
        private RelayCommand _draftUser2;
        public ICommand DraftUser2Command { get { return _draftUser2; } }

        // Konstruktor
        public DraftViewModelOld(Window window, User user1, User user2, League league, Season season)
        {
            SetTurns();
            // Attribute setzen
            _user1 = user1;
            _user2 = user2;
            _indexUser1 = 0;
            _indexUser2 = 0;
            _league = league;
            _season = season;
            _draftCount = 0;
            _position = "Goalkeeper";
            _positionChanged = false;
            // Commands
            _draftUser1 = new RelayCommand(DraftPlayer, () => _turnUser1);
            _draftUser2 = new RelayCommand(DraftPlayer, () => _turnUser2);
            // Listen initialisieren
            _teamUser1 = new Player[11];
            _teamUser2 = new Player[11];
            PlayerList = new ObservableCollection<Player>();
            ShowPlayers();
            SetDraftText();
        }

        // noch zu implemetieren
        // 1: Undo Button
        // 2: Neustart Button
        // 3: Marktwerte ausgeben und Maximalbudget hinzufügen

        private void GoToGameHome()
        {
            GameHomeWindow ghWindow = new GameHomeWindow(_user1, _user2, _teamUser1, _teamUser2, _season, _league);
            ghWindow.ShowDialog();
        }

        private void DraftPlayer()
        {
            if (SelectedPlayer != null)
            {
                _draftCount++;
                SetTurns();
                SetPosition();
                SetDraftText();
                if (_turnUser1)
                {
                    Player temp = new Player();
                    temp = _selectedPlayer;
                    _teamUser2[_indexUser2] = temp;
                    OnPropertyChanged("TeamUser2");
                    PlayerList.Remove(_selectedPlayer);
                    _indexUser2++;
                    _draftUser1.RaiseCanExecuteChanged();
                    _draftUser2.RaiseCanExecuteChanged();
                }
                if (_turnUser2)
                {
                    Player temp = new Player();
                    temp = _selectedPlayer;
                    _teamUser1[_indexUser1] = temp;
                    OnPropertyChanged("TeamUser1");
                    PlayerList.Remove(_selectedPlayer);
                    _indexUser1++;
                    _draftUser1.RaiseCanExecuteChanged();
                    _draftUser2.RaiseCanExecuteChanged();
                }
                if (_draftCount == 22)
                {
                    GoToGameHome();
                    // Draft schöner abschließen, z.B. kleine Zusammenfassung 
                }
                ShowPlayers();
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
                case 2:
                    _position = "Defender";
                    _positionChanged = true;
                    break;
                case 10:
                    _position = "Midfielder";
                    _positionChanged = true;
                    break;
                case 18:
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
            if (_turnUser1)
            {
                DraftTextUser1 = $"{User1.UserName}, bitte wähle einen {_position}!\nDu hast noch {11 - _indexUser1} Drafts";
                DraftTextUser2 = "Anderer Spieler wählt!";
            }
            else
            {
                DraftTextUser2 = $"{User2.UserName}, bitte wähle einen {_position}!\nDu hast noch {11 - _indexUser2} Drafts";
                DraftTextUser1 = "Anderer Spieler wählt!";
            }
        }

        private void SetTurns()
        {
            if (_draftCount % 2 == 0)
            {
                _turnUser1 = true;
                _turnUser2 = false;
            }
            if (_draftCount % 2 == 1)
            {
                _turnUser2 = true;
                _turnUser1 = false;
            }
        }
    }
}


