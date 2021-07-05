using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FFM_WIFI.ViewModels
{
    class UserHomeViewModel : BaseViewModel
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

        // Properties für Combobox
        public ObservableCollection<League> LeagueList { get; set; }

        private League _selectedLeague;
        public League SelectedLeague
        {
            get { return _selectedLeague; }
            set
            {
                _selectedLeague = value;
                _login.RaiseCanExecuteChanged();
                // OnPropertyChanged("SelectedLeague");
            }
        }

        public ObservableCollection<Season> SeasonList { get; set; }
        private Season _selectedSeason;
        public Season SelectedSeason
        {
            get { return _selectedSeason; }
            set
            {
                _selectedSeason = value;
                _login.RaiseCanExecuteChanged();
                // OnPropertyChanged("SelectedSeason");
            }
        }

        // Attribute

        // Commands
        public ICommand EditDBCommand { get; set; }

        protected RelayCommand _login;
        public ICommand LoginCommand { get { return _login; } }
            
        // Konstruktor
        public UserHomeViewModel(UserHomeWindow window, User user)
        {
            //Daten initialisieren
            _selectedLeague = null;
            _selectedSeason = null;
            // Commands
            EditDBCommand = new RelayCommand(GoToEditDatabase);
            _login = new RelayCommand(GoToLogin, () => SelectedLeague != null && SelectedSeason != null);
            // User
            User1 = user;
            // Listen initialisieren und füllen
            SeasonList = new ObservableCollection<Season>();
            LeagueList = new ObservableCollection<League>();
            GetLeagueSeason();
        }

        private void GoToEditDatabase()
        {
            EditDatabaseWindow edbWindow = new EditDatabaseWindow();
            edbWindow.ShowDialog();
        }

        private void GoToLogin()
        {
            LoginWindow lWindow = new LoginWindow(User1, SelectedLeague, SelectedSeason);
            lWindow.ShowDialog();
        }

        private void GetLeagueSeason()
        {
            using (FootballContext context = new FootballContext())
            {
                var leagues = context.League;
                var seasons = context.Season;

                foreach (var item in leagues)
                {
                    League temp = new League();
                    temp = item;
                    LeagueList.Add(temp);
                }

                foreach (var item in seasons)
                {
                    Season temp = new Season();
                    temp = item;
                    SeasonList.Add(temp);
                }
            }
        }
    }
}
