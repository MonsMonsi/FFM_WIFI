using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class GameHomeViewModel : BaseViewModel
    {
        // Properties für User
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

        // Properties für Datagrids
        public ObservableCollection<Player> SubstitutionList { get; set; }
        private Player _selectedSubstitution;
        public Player SelectedSubstitution
        {
            get { return _selectedSubstitution; }
            set
            {
                _selectedSubstitution = value;
                OnPropertyChanged("SelectedSubstitution");
            }
        }

        // Property für Canvas
        private Player[] _lineUp;
        public Player[] LineUp
        {
            get { return _lineUp; }
            set
            {
                _lineUp = value;
            }
        }

        private RelayCommand<object> _sub;
        public ICommand SubCommand { get { return _sub; } }

        // Attribute

        // Konstruktor
        public GameHomeViewModel(Window window, User user)
        {
            // Property setzen
            GetUserTeam(user);
            // Commands

            _sub = new RelayCommand<object>(SubPlayer);
            // Listen initialisieren
            LineUp = new Player[11];
            
            SubstitutionList = new ObservableCollection<Player>();
            SetUserPlayer();
        }

        private void SubPlayer (object position)
        {
            if (position != null)
            {
                string p = position.ToString();
                int index = int.Parse(p);

                Player temp = new Player();
                if (LineUp[index] != null)
                {
                    temp = LineUp[index];
                    LineUp[index] = null;
                    OnPropertyChanged("LineUp");
                }

                if (temp != null)
                {
                    SubstitutionList.Add(temp);
                }
            }
        }

        private void GetUserTeam(User user)
        {
            using (FootballContext context = new FootballContext())
            {
                var userTeam = context.UserTeam.Where(u => u.UserTeamUserFk == user.UserPk).FirstOrDefault();

                if (userTeam != null)
                {
                    UserTeam = userTeam;
                }
            }
        }

        private void SetUserPlayer()
        {
            using (FootballContext context = new FootballContext())
            {
                int?[] players = new int?[]
                {
                    UserTeam.UserTeamGk1, UserTeam.UserTeamDf1, UserTeam.UserTeamDf2, UserTeam.UserTeamDf3, UserTeam.UserTeamDf4, UserTeam.UserTeamMf1,
                    UserTeam.UserTeamMf2, UserTeam.UserTeamMf3, UserTeam.UserTeamMf4, UserTeam.UserTeamAt1, UserTeam.UserTeamAt2, UserTeam.UserTeamGk2,
                    UserTeam.UserTeamDf5, UserTeam.UserTeamMf5, UserTeam.UserTeamMf6, UserTeam.UserTeamAt3, UserTeam.UserTeamAt4
                };

                for (int i = 0; i < 11; i++)
                {
                    if (players[i] != null)
                    {
                        var player = context.Player.Where(pl => pl.PlayerPk == players[i]).FirstOrDefault();
                        LineUp[i] = player;
                    }
                }
                OnPropertyChanged("LineUp");

                SubstitutionList.Clear();
                for (int i = 11; i < 17; i++)
                {
                    if (players[i] != null)
                    {
                        var player = context.Player.Where(pl => pl.PlayerPk == players[i]).FirstOrDefault();
                        SubstitutionList.Add(player);
                    }
                }
            }
        }
    }
}
