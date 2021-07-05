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
    class StartViewModel: BaseViewModel
    {
        // Properties
        // Properties für Login
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

        // Commands
        public ICommand EditDBCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand NewUserCommand { get; set; }

        // Konstruktor
        public StartViewModel()
        {
            LoginCommand = new RelayCommand(CheckLogin);
            NewUserCommand = new RelayCommand(CheckNewUser);
        }

        private void GoToUserHome(User user)
        {
            UserHomeWindow uhWindow = new UserHomeWindow(user);
            uhWindow.ShowDialog();
        }

        private void CheckLogin()
        {
            using (FootballContext context = new FootballContext())
            {
                var existingUser = context.User.Where(u => u.UserName == UserName && u.UserPassword == UserPassword).FirstOrDefault();

                if (existingUser != null)
                {
                    User user = new User();
                    user.UserName = existingUser.UserName;
                    GoToUserHome(user);
                }
                else
                {
                    MessageBox.Show("Benutzer nicht gefunden!");
                }
            }
            
        }

        private void CheckNewUser()
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
