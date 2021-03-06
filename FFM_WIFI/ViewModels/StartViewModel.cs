using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class StartViewModel : BaseViewModel
    {
        #region Properties
        // Properties für Login
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _userPassword;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Attributes
        private User _user;
        private Window _window;
        #endregion

        #region Commands
        public ICommand LoginCommand { get; set; } // Login für bereits bestehende User
        public ICommand NewUserCommand { get; set; } // Erstellt neuen User
        #endregion

        #region Constructor
        public StartViewModel(Window window)
        {
            _window = window;
            LoginCommand = new RelayCommand(CheckLogin);
            NewUserCommand = new RelayCommand(CheckNewUser);
        }
        #endregion

        #region Methods
        private void GoToUserHome()
        {
            UserHomeWindow uhWindow = new UserHomeWindow(_user);
            _window.Close();
            uhWindow.ShowDialog();
        }

        private void CheckLogin()
        {
            if (_userName != null)
            {
                // Checkt die Datenbank, ob User bereits vorhanden
                using (FootballContext context = new FootballContext())
                {
                    var existingUser = context.User.Where(u => u.UserName == UserName && u.UserPassword == UserPassword).FirstOrDefault();

                    if (existingUser != null)
                    {
                        _user = existingUser;
                        GoToUserHome();
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
            if (UserName != null && UserPassword != null)
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
                        _user = user;
                        context.SaveChanges();
                        GoToUserHome();
                    }
                    else
                    {
                        MessageBox.Show("Benutzername ist bereits vergeben");
                    }
                }
            }
        }
        #endregion
    }
}
