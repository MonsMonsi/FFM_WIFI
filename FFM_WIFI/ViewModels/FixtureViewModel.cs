using FFM_WIFI.Commands;
using FFM_WIFI.Models.DataContext;
using FFM_WIFI.Models.DataJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FFM_WIFI.Views;

namespace FFM_WIFI.ViewModels
{
    class FixtureViewModel : BaseViewModel
    {
        // Property für User Team und Performance
        public PlayerInfo[] PlayerData { get; set; }
        public ObservableCollection<PlayerInfo> DraftedPlayers { get; set; }
        private PlayerInfo _selectedPlayer;
        public PlayerInfo SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");
                _detail.RaiseCanExecuteChanged();
            }
        }

        // Property für TextBlock und Image
        private TeamInfo _teamData;
        public TeamInfo TeamData
        {
            get { return _teamData; }
            set
            {
                _teamData = value;
                OnPropertyChanged("TeamData");
            }
        }


        // Property für ListView
        public ObservableCollection<string> ListViewText { get; set; }

        // Attribute
        private Window _window;
        private int[] _fixtures;
        private UserTeam _userTeam;
        private int _playday;
        private bool _back;

        // Strings für Ausgabe
        private string _headerPlayday;
        private string _headerDetail;
        private string _line;

        // Commands
        private RelayCommand _start;
        public ICommand StartCommand { get { return _start; } }
        private RelayCommand _detail;
        public ICommand DetailCommand { get { return _detail; } }
        private RelayCommand _home;
        public ICommand HomeCommand { get { return _home; } }

        // Konstruktor
        public FixtureViewModel(Window window, TeamInfo teamData, PlayerInfo[] playerData, int[] fixtures)
        {
            // Daten setzen und initialisieren
            _window = window;
            _teamData = teamData;
            _userTeam = _teamData.Team;
            _fixtures = fixtures;
            PlayerData = playerData;
            DraftedPlayers = new ObservableCollection<PlayerInfo>();
            SetDraftedPlayersList();
            _playday = _userTeam.UserTeamPlayday;
            _back = false;
            ListViewText = new ObservableCollection<string>();
            SetStrings();
            // Commands
            _start = new RelayCommand(CalculatePlayday, () => _playday == _userTeam.UserTeamPlayday);
            _detail = new RelayCommand(ShowDetailText, () => _playday != _userTeam.UserTeamPlayday && SelectedPlayer != null);
            _home = new RelayCommand(GoToGameHome, () => _back);
            _home.RaiseCanExecuteChanged();
        }

        private void GoToGameHome()
        {
            _teamData.Playday++;
            _teamData.Team.UserTeamPlayday++;
            GameHomeWindow uhWindow = new GameHomeWindow(_teamData, PlayerData);
            _window.Close();
            uhWindow.ShowDialog();   
        }

        private void CalculatePlayday()
        {
            SetPlaydayText();
            _playday++;
            _detail.RaiseCanExecuteChanged();
            _start.RaiseCanExecuteChanged();
            _back = true;
            _home.RaiseCanExecuteChanged();
        }

        private void ShowDetailText()
        {
            ListViewText.Clear();
            ListViewText.Add(_headerDetail + "\n" + SelectedPlayer.Name);
            ListViewText.Add("Punkte gesamt: " + SelectedPlayer.Points);
            ListViewText.Add(_line);

            if (SelectedPlayer.LineUp > 0)
            {
                if (SelectedPlayer.LineUp == 6)
                    ListViewText.Add($"LineUp-Punkte: {SelectedPlayer.LineUp} (Startelf)");
                else
                    ListViewText.Add($"LineUp-Punkte: {SelectedPlayer.LineUp} (Bankplatz)");
            }
            if (SelectedPlayer.Goals > 0)
                ListViewText.Add($"Goal-Punkte: {SelectedPlayer.Goals * 16} ({SelectedPlayer.Goals} Tore)");
            if (SelectedPlayer.Assists > 0)
                ListViewText.Add($"Assist-Punkte: {SelectedPlayer.Assists * 8} ({SelectedPlayer.Assists} Assists)");
            if (SelectedPlayer.YellowC > 0)
                ListViewText.Add($"YellowCard-Strafe: {SelectedPlayer.YellowC * -1} ({SelectedPlayer.YellowC} Gelbe Karte)");
            if (SelectedPlayer.RedC > 0)
                ListViewText.Add($"RedCard-Strafe: {SelectedPlayer.RedC * -4} ({SelectedPlayer.RedC} Rote Karte)");
            if (SelectedPlayer.Subst > 0)
                ListViewText.Add($"Subst-Strafe: {SelectedPlayer.Subst * -2} (Spieler wurde 1.Halbzeit ausgewechselt)");
        }

        private void SetPlaydayText()
        {
            // Werte auf 0 setzen
            foreach (var p in PlayerData)
            {
                p.LineUp = 0;
                p.Subst = 0;
                p.YellowC = 0;
                p.RedC = 0;
                p.Goals = 0;
                p.Assists = 0;
            }

            foreach (var id in _fixtures)
            {
                string filePath = @$"D:\VS_Projects\FFM_WIFI\JsonFiles\FixtureDetailsBULI2020\BULI2020_Fixture{id}.json";

                StreamReader reader = File.OpenText(filePath);

                var fixture = JsonConvert.DeserializeObject<JsonFixture.Root>(reader.ReadToEnd());

                // Werte berechnen
                SetLineUpPoints(fixture);
                SetEventPoints(fixture);
                SetDraftedPlayersList();

                ListViewText.Add(_headerPlayday + "\n" + fixture.response[0].teams.home.name + " vs. " + fixture.response[0].teams.away.name);
                ListViewText.Add(_line);

                foreach (var e in fixture.response[0].events)
                {
                    string eventType1 = "";
                    string eventType2 = "";

                    switch (e.type)
                    {
                        case "Goal":
                            eventType1 = $"Torschütze: {e.player.name}";
                            eventType2 = $" - Assist: {e.assist.name}";
                            break;
                        case "subst":
                            eventType1 = $"Aus: {e.player.name}";
                            eventType2 = $" - Ein: {e.assist.name}";
                            break;
                        case "Card":
                            eventType1 = $"Karte für: {e.player.name}";
                            break;
                    }

                    string newEvent = $"Minute: {e.time.elapsed} - Team: {e.team.name}\n" +
                                      $"{e.type}!\n" +
                                      $"{eventType1}{eventType2}\n" +
                                      $"{_line}";
                    ListViewText.Add(newEvent);
                }
            }
        }

        private void SetEventPoints(JsonFixture.Root fixture)
        {
            PlayerInfo[] temp = PlayerData;
            foreach (var e in fixture.response[0].events)
            {
                switch (e.type)
                {
                    case "Goal":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.player.id && p.Drafted)
                            {
                                p.Points += 16;
                                p.Goals++;
                            }
                            if (p.Id == e.assist.id && p.Drafted)
                            {
                                p.Points += 8;
                                p.Assists++;
                            }
                        }
                        break;
                    case "subst":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.player.id && e.time.elapsed < 45 && p.Drafted)
                            {
                                p.Points -= 2;
                                p.Subst++;
                            }

                        }
                        break;
                    case "Card":
                        foreach (var p in temp)
                        {
                            if (p.Id == e.player.id && e.detail == "Yellow Card" && p.Drafted)
                            {
                                p.Points -= 1;
                                p.YellowC++;
                            }
                            if (p.Id == e.player.id && e.detail == "Red Card" && p.Drafted)
                            {
                                p.Points -= 4;
                                p.RedC++;
                            }
                        }
                        break;
                }
                PlayerData = temp;
            }
        }

        private void SetLineUpPoints(JsonFixture.Root fixture)
        {
            PlayerInfo[] temp = PlayerData;
            foreach (var s in fixture.response[0].lineups[0].startXI)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 6;
                        p.LineUp += 6;
                    }
                }
            }

            foreach (var s in fixture.response[0].lineups[1].startXI)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 6;
                        p.LineUp += 6;
                    }
                }

            }

            foreach (var s in fixture.response[0].lineups[0].substitutes)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 2;
                        p.LineUp += 2;
                    }
                }

            }

            foreach (var s in fixture.response[0].lineups[1].substitutes)
            {
                foreach (var p in temp)
                {
                    if (p.Id == s.player.id && p.Drafted)
                    {
                        p.Points += 2;
                        p.LineUp += 2;
                    }
                }

            }
            PlayerData = temp;
        }

        private void SetDraftedPlayersList()
        {
            DraftedPlayers.Clear();
            foreach (var p in PlayerData)
            {
                if (p.Drafted)
                    DraftedPlayers.Add(p);
            }
        }

        private void SetStrings()
        {
            _headerPlayday = "--- Paarung:";
            _headerDetail = "--- Spieler:";
            _line = "--------------------------------------------------------";
        }
    }
}
