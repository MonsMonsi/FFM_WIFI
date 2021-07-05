using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        // Properties
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string _userPassword;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }

        // Attribute
        private User _user1;
        private League _league;
        private Season _season;

        // Commands
        public ICommand LoginCommand { get; set; }
        public ICommand NewUserCommand { get; set; }

        // Konstruktor
        public LoginViewModel(Window window, User user, League league, Season season)
        {
            _user1 = user;
            _league = league;
            _season = season;
            LoginCommand = new RelayCommand(CheckLogin);
            NewUserCommand = new RelayCommand(CheckNewUser);
        }

        private void GoToDraft(User user2)
        {
            DraftWindow dWindow = new DraftWindow(_user1, user2, _league, _season);
            dWindow.ShowDialog();
        }

        private void GoToUserHome(User user)
        {
            UserHomeWindow uhWindow = new UserHomeWindow(user);
            uhWindow.ShowDialog();
        }

        private void CheckLogin()
        {
            if (_user1 != null && _league != null && _season != null)
            {
                using (FootballContext context = new FootballContext())
                {
                    var existingUser = context.User.Where(u => u.UserName == UserName && u.UserPassword == UserPassword).FirstOrDefault();

                    if (existingUser != null)
                    {
                        User user2 = new User();
                        user2.UserName = existingUser.UserName;
                        GoToDraft(user2);
                    }
                    else
                    {
                        MessageBox.Show("Benutzer nicht gefunden!");
                    }
                }
            }
            else
            {
                using (FootballContext context = new FootballContext())
                {
                    var existingUser = context.User.Where(u => u.UserName == UserName && u.UserPassword == UserPassword).FirstOrDefault();

                    if (existingUser != null)
                    {
                        User user1 = new User();
                        user1.UserName = existingUser.UserName;
                        GoToUserHome(user1);
                    }
                    else
                    {
                        MessageBox.Show("Benutzer nicht gefunden!");
                    }
                }
            }
        }

        private void CheckNewUser()
        {
            if (_user1 != null && _league != null && _season != null)
            {
                using (FootballContext context = new FootballContext())
                {
                    var existingUserName = context.User.Where(u => u.UserName == UserName).FirstOrDefault();

                    if (existingUserName == null) // Name noch nicht vergeben -> neuen Nutzer anlegen
                    {
                        User user2 = new User();
                        user2.UserName = UserName;
                        user2.UserPassword = UserPassword;
                        context.User.Add(user2);
                        context.SaveChanges();
                        user2.UserPassword = null;
                        GoToDraft(user2);
                    }
                    else
                    {
                        MessageBox.Show("Benutzername ist bereits vergeben");
                    }
                }
            }
            else
            {
                using (FootballContext context = new FootballContext())
                {
                    var existingUserName = context.User.Where(u => u.UserName == UserName).FirstOrDefault();

                    if (existingUserName == null) // Name noch nicht vergeben -> neuen Nutzer anlegen
                    {
                        User user = new User();
                        user.UserName = UserName;
                        user.UserPassword = UserPassword;
                        context.User.Add(user);
                        context.SaveChanges();
                        user.UserPassword = null;
                        GoToUserHome(user);
                    }
                    else
                    {
                        MessageBox.Show("Benutzername ist bereits vergeben");
                    }
                }
            }
        }
    }
}
