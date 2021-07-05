using FFM_WIFI.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FFM_WIFI.ViewModels
{
    class GameHomeViewModel : BaseViewModel
    {
        // Properties für User
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
                OnPropertyChanged("TeamUser1");
            }
        }

        private Player[] _teamUser2;
        public Player[] TeamUser2
        {
            get { return _teamUser2; }
            set
            {
                _teamUser2 = value;
                OnPropertyChanged("TeamUser2");
            }
        }

        // Attribute
        private Season _season;
        private League _league;

        // Konstruktor
        public GameHomeViewModel(Window window, User user1, User user2, Player[] teamUser1, Player[] teamUser2, Season season, League league)
        {
            // Attribute setzen
            _user1 = user1;
            _user2 = user2;
            _teamUser1 = teamUser1;
            _teamUser2 = teamUser2;
            _season = season;
            _league = league;
        }
    }
}
