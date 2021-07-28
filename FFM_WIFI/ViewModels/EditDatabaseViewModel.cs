using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FFM_WIFI.ViewModels
{
    class EditDatabaseViewModel : BaseViewModel
    {
        // Properties
        // Properties für Image
        private BitmapImage _clubImage;
        public BitmapImage ClubImage
        {
            get { return _clubImage; }
            set
            {
                _clubImage = value;
                OnPropertyChanged("ClubImage");
            }
        }

        private BitmapImage _venueImage;
        public BitmapImage VenueImage
        {
            get { return _venueImage; }
            set
            {
                _venueImage = value;
                OnPropertyChanged("VenueImage");
            }
        }

        // Properties für Combobox
        public ObservableCollection<League> LeagueList { get; set; }
        public League SelectedLeague { get; set; }
        public ObservableCollection<Season> SeasonList { get; set; }
        public Season SelectedSeason { get; set; }

        // Properties für Datagrids
        public ObservableCollection<Team> TeamList { get; set; }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                ShowPlayers();
                OnPropertyChanged("SelectedTeam");
            }
        }

        private Venue _selTeamVenue;
        public Venue SelTeamVenue
        {
            get { return _selTeamVenue; }
            set
            {
                _selTeamVenue = value;
                OnPropertyChanged("SelTeamVenue");
            }
        }

        public ObservableCollection<Player> PlayerList { get; set; }
        public Player SelectedPlayer { get; set; }

        // Commands
        public ICommand ShowClubsCommand { get; set; }
        public ICommand EditClubCommand { get; set; }
        public ICommand EditPlayerCommand { get; set; }

        // Konstruktor
        public EditDatabaseViewModel()
        {
            // Daten initialisieren
            SelectedLeague = null;
            SelectedSeason = null;
            // Commands
            ShowClubsCommand = new RelayCommand(ShowClubs);
            // Listen initialisieren und füllen
            LeagueList = new ObservableCollection<League>();
            SeasonList = new ObservableCollection<Season>();
            TeamList = new ObservableCollection<Team>();
            PlayerList = new ObservableCollection<Player>();
            GetLeagueSeason();
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
                    temp.LeaguePk = item.LeaguePk;
                    temp.LeagueName = item.LeagueName;
                    LeagueList.Add(temp);
                }

                foreach (var item in seasons)
                {
                    Season temp = new Season();
                    temp.SeasonPk = item.SeasonPk;
                    temp.SeasonName = item.SeasonName;
                    SeasonList.Add(temp);
                }
            }
        }

        private BitmapImage GetImage(string path)
        {
            WebClient client = new WebClient();
            Uri url = new Uri($"{path}");
            BitmapImage image = new BitmapImage(url);
            return image;
        }

        private void ShowClubs()
        {
            if (SelectedLeague != null && SelectedSeason != null)
            {
                using (FootballContext context = new FootballContext())
                {
                    var teams = context.SeasonLeagueTeamAssignment.Include(t => t.SeaLeaTeaTeamFkNavigation).Where(t => t.SeaLeaTeaSeasonFk == SelectedSeason.SeasonPk && t.SeaLeaTeaLeagueFk == SelectedLeague.LeaguePk).Select(t => t.SeaLeaTeaTeamFkNavigation);

                    foreach (var item in teams)
                    {
                        TeamList.Add(item);
                    }
                }
            }
        }

        private void ShowPlayers()
        {
            PlayerList.Clear();
            if (SelectedTeam != null)
            {
                // Teaminfo ausgeben
                ClubImage = GetImage(SelectedTeam.TeamLogo);

                using (FootballContext context = new FootballContext())
                {
                    var teamPk = context.SeasonLeagueTeamAssignment.Where(t => t.SeaLeaTeaTeamFk == SelectedTeam.TeamPk).Select(t => t.SeaLeaTeaPk).FirstOrDefault();
                    var players = context.TeamPlayerAssignment.Include(p => p.TeaPlaPlayerFkNavigation).Where(p => p.TeaPlaTeamFk == teamPk).Select(p => p.TeaPlaPlayerFkNavigation);

                    foreach (var item in players)
                    {
                        PlayerList.Add(item);
                    }

                    var venue = context.Team.Include(v => v.TeamVenueFkNavigation).Where(v => v.TeamPk == SelectedTeam.TeamPk).Select(v => v.TeamVenueFkNavigation).FirstOrDefault();
                    SelTeamVenue = venue;

                    VenueImage = GetImage(SelTeamVenue.VenueImage);
                }
            }
        }
    }
}
