using DatabaseTesting.Models;
using FFM_WIFI.Models.DataJson;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace DatabaseTesting
{
    class Program
    {
        static void ResetTeamPlayerAssignment()
        {
            int team = 192;
            int pages = 2;

            for (int i = 1; i < pages + 1; i++)
            {
                string docPath = @"D:\VS_Projects\FFM_WIFI\JsonFiles\Squads";
                string doc = Path.Combine(docPath, $"Bundesliga2020Squad{team}Page{i}.json");

                FileStream stream = new FileStream(doc, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                var players = JsonSerializer.Deserialize<JsonPlayers.Root>(reader.ReadToEnd());
                pages = players.paging.total;

                using (FootballContext context = new FootballContext())
                {
                    foreach (var p in players.response)
                    {
                        var pk = context.SeasonLeagueTeamAssignment.Where(k => k.SeaLeaTeaTeamFk == p.statistics[0].team.id).Select(k => k.SeaLeaTeaPk).FirstOrDefault();
                        var assignment = new TeamPlayerAssignment();

                        assignment.TeaPlaPlayerFk = p.player.id;
                        assignment.TeaPlaTeamFk = pk;
                        if (p.statistics[0].games.rating != null)
                            assignment.TeaPlaPlayerRating = double.Parse(p.statistics[0].games.rating);

                        context.Add(assignment);
                    }
                    context.SaveChanges();
                }
            }
        }

        static void CalcRatings()
        {
            using (FootballContext context = new FootballContext())
            {
                var ratings = context.TeamPlayerAssignment.Select(r => r.TeaPlaPlayerRating);

                double max = 0;
                double min = 99999999999999999;
                double all = 0;
                double ave = 0;
                int count = 0;

                foreach (var r in ratings)
                {
                    if (r > 0)
                    {
                        if (r > max)
                            max = r;
                        if (r < min)
                            min = r;
                        all += r;
                        count++;
                    }
                }

                ave = all / count;

                Console.WriteLine($"Max: {max:0.00}  |  Min: {min:0.00}  |  All: {all:0.00}  |  Av: {ave:0.00}");
            }
        }

        static void SetValue()
        {
            using (FootballContext context = new FootballContext())
            {
                var assignment = context.TeamPlayerAssignment.Include(a => a.TeaPlaPlayerFkNavigation);

                foreach (var a in assignment)
                {
                    switch (a.TeaPlaPlayerRating)
                    {
                        case > 7300000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 21000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 40000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 65000000;
                            break;
                        case > 7200000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 19000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 38000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 52000000;
                            break;
                        case > 7100000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 17000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 36000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 50000000;
                            break;
                        case > 7000000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 16000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 35000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 48000000;
                            break;
                        case > 6900000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 15000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 33000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 46000000;
                            break;
                        case > 6800000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 14000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 30000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 40000000;
                            break;
                        case > 6700000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 13000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 25000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 33000000;
                            break;
                        case > 6600000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 10000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 20000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 29000000;
                            break;
                        case > 6400000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 8000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 15000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 23000000;
                            break;
                        case > 6200000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 6000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 10000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 18000000;
                            break;
                        case <= 6200000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 4000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 7000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 10000000;
                            break;
                    }
                }
                context.SaveChanges();
            }
        }

        static void ShowPlayers()
        {
            using (FootballContext context = new FootballContext())
            {
                var assignment = context.TeamPlayerAssignment.Include(a => a.TeaPlaPlayerFkNavigation);

                foreach (var a in assignment)
                {
                    if (a.TeaPlaPlayerValue > 40000000 && a.TeaPlaPlayerValue < 50000000)
                    {
                        Console.WriteLine(a.TeaPlaPlayerFkNavigation.PlayerLastName + " " + a.TeaPlaPlayerFkNavigation.PlayerPk);
                    }
                }
            }
        }


        public class Images
        {
            public string Self { get; set; }
            public string Star { get; set; }
            public Images (string self = null, string star = null)
            {
                Self = self;
                Star = star;
            }
        }

        static void WriteToConfig()
        {
            string path = @"D:\VS_Projects\FFM_WIFI\JsonFiles\Images\";
            string star = "https://thumbs.dreamstime.com/z/goldener-stern-8482955.jpg";

            Images starImg = new Images(path, star);

            var jsonObject = JsonSerializer.Serialize<Images>(starImg);

            File.WriteAllText(path + "Images.json", jsonObject);
        }

        static void WriteAllFixturesToFile()
        {
            WebClient client = new WebClient();
            client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

            var allFixtures = JsonSerializer.Deserialize<JsonAllFixtures.Root>(client.DownloadString($"https://v3.football.api-sports.io/fixtures?league=78&season=2021"),
                                                                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(docPath, @"JsonFiles\AllFixtures\L78S2021.json");

            File.WriteAllText(path, JsonSerializer.Serialize(allFixtures));
        }

        static void WriteToJsonSquads(int leagueId, List<int> teamIds, int seasonId)
        {
            foreach (var teamId in teamIds)
            {
                int page = 1;
                int maxPage = 2;

                if (teamId != 191)
                {
                    while (page <= maxPage)
                    {
                        WebClient client = new WebClient();
                        client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                        var squad = JsonSerializer.Deserialize<JsonPlayers.Root>(client.DownloadString($"https://v3.football.api-sports.io/players?league={leagueId}&team={teamId}&season={seasonId}&page={page}"),
                                                                                           new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        maxPage = squad.paging.total;

                        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string path = Path.Combine(docPath, @$"JsonFiles\Players\Players_L{leagueId}S{seasonId}T{teamId}P{page}.json");

                        Console.WriteLine(squad.response[0].statistics[0].team.name + squad.paging.current + squad.paging.total);

                        File.WriteAllText(path, JsonSerializer.Serialize(squad));
                        page++;
                    }
                    // Thread.Sleep(5000); // wegen api limit
                }
            }
        } 

        static List<int> GetTeamIds(int season)
        {
            List<int> idList = new List<int>();

            WebClient client = new WebClient();
            client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

            var teamIds = JsonSerializer.Deserialize<JsonTeamVenue.Root>(client.DownloadString($"https://v3.football.api-sports.io/teams?league=78&season={season}"),
                                                                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //string path = Path.Combine(docPath, @$"JsonFiles\TeamVenue\TeamVenue_L78S2021.json");

            //File.WriteAllText(path, JsonSerializer.Serialize(teamIds));

            foreach (var teamId in teamIds.response)
            {
                idList.Add(teamId.team.id);
            }

            return idList;
        }

        static void WriteInDb_TeamsVenues()
        {
            int leagueKey = 78;

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string name = Path.Combine(docPath, @"JsonFiles\TeamVenue\Bundesliga2021_TeamVenue.json");

            var teamsVenues = JsonSerializer.Deserialize<JsonTeamVenue.Root>(File.ReadAllText(name));

            using (FootballContext context = new FootballContext())
            {
                foreach (var item in teamsVenues.response)
                {
                    var testTeam = context.Team.Where(t => t.TeamPk == item.team.id).FirstOrDefault();
                    var testVenue = context.Venue.Where(t => t.VenuePk == item.venue.id).FirstOrDefault();

                    if (testTeam == null) // neuer Eintrag
                    {
                        Team team = new Team();
                        team.TeamPk = item.team.id;
                        team.TeamName = item.team.name;
                        team.TeamFounded = item.team.founded;
                        team.TeamLogo = item.team.logo;
                        team.TeamVenueFk = item.venue.id;

                        context.Team.Add(team);
                    }

                    
                    SeasonLeagueTeamAssignment assignment = new SeasonLeagueTeamAssignment();
                    assignment.SeaLeaTeaSeasonFk = 2021;
                    assignment.SeaLeaTeaLeagueFk = leagueKey;
                    assignment.SeaLeaTeaTeamFk = item.team.id;

                    context.SeasonLeagueTeamAssignment.Add(assignment);
                    

                    if (testVenue == null) // neuer Eintrag
                    {
                        Venue venue = new Venue();
                        venue.VenuePk = item.venue.id;
                        venue.VenueName = item.venue.name;
                        venue.VenueAddress = item.venue.address;
                        venue.VenueCity = item.venue.city;
                        venue.VenueCapacity = item.venue.capacity;
                        venue.VenueImage = item.venue.image;

                        context.Venue.Add(venue);
                    }
                    context.SaveChanges();
                }
            }
        }

        static void WriteInDB_Players()
        {
            int teamId = 192;
            int page = 1;
            int maxPage = 2;

            for (int i = page; i < maxPage + 1; i++)
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string name = Path.Combine(docPath, @$"JsonFiles\Squads\Bundesliga2021Squad{teamId}Page{page}.json");

                var players = JsonSerializer.Deserialize<JsonPlayers.Root>(File.ReadAllText(name));
                maxPage = players.paging.total;

                using (FootballContext context = new FootballContext())
                {
                    foreach (var item in players.response)
                    {
                        var testPlayer = context.Player.Where(p => p.PlayerPk == item.player.id).FirstOrDefault();

                        if (testPlayer == null) // neuer Eintrag
                        {
                            Player player = new Player();
                            player.PlayerPk = item.player.id;
                            player.PlayerFirstName = item.player.firstname;
                            player.PlayerLastName = item.player.lastname;
                            player.PlayerDateOfBirth = item.player.birth.date;
                            player.PlayerPosition = item.statistics[0].games.position;
                            player.PlayerNationality = item.player.nationality;
                            player.PlayerHeight = item.player.height;
                            player.PlayerImage = item.player.photo;

                            context.Player.Add(player);
                        }

                        TeamPlayerAssignment assignment = new TeamPlayerAssignment();

                        // den PK ermitteln auf den das assignment zeigen soll
                        var assignmentPk = context.SeasonLeagueTeamAssignment.Where(a => a.SeaLeaTeaLeagueFk == 78 && a.SeaLeaTeaSeasonFk == 2021 && a.SeaLeaTeaTeamFk == teamId).Select(a => a.SeaLeaTeaPk).FirstOrDefault();

                        assignment.TeaPlaTeamFk = assignmentPk;
                        assignment.TeaPlaPlayerFk = item.player.id;
                        if (item.statistics[0].games.rating != null)
                        {
                            assignment.TeaPlaPlayerRating = double.Parse(item.statistics[0].games.rating);
                        }
                        else
                        {
                            assignment.TeaPlaPlayerRating = 0;
                        }
                        assignment.TeaPlaPlayerValue = 0;
                        
                        context.TeamPlayerAssignment.Add(assignment);
                        context.SaveChanges();
                        
                    }
                }
                page++;
            }
        }

        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

            var allFixtures = JsonSerializer.Deserialize<JsonAllFixtures.Root>(client.DownloadString($"https://v3.football.api-sports.io/fixtures?league=78&season=2020"),
                                                                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            List<int> ids = new List<int>();

            foreach (var f in allFixtures.response)
            {
                ids.Add(f.fixture.id);
            }

            int i = 1;

            foreach (var id in ids)
            {
                //WebClient innerClient = new WebClient();
                //client.Headers.Add("x-apisports-key", "a3a80245cddcf074947be5c6ac43484f");

                var fixture = JsonSerializer.Deserialize<JsonFixture.Root>(client.DownloadString($"https://v3.football.api-sports.io/fixtures?id={id}"),
                                                                                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Response[0];

                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = Path.Combine(docPath, @$"JsonFiles\Fixture\Fixture_L78S2020I{id}.json");

                Console.WriteLine(i);
                i++;

                File.WriteAllText(path, JsonSerializer.Serialize(fixture));
                Thread.Sleep(50);
            }
        }
    }
}
