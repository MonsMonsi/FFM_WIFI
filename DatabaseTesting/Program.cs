using DatabaseTesting.Models;
using FFM_WIFI.Models.DataJson;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

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

                var players = JsonSerializer.Deserialize<JsonPlayer.Root>(reader.ReadToEnd());
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
                                a.TeaPlaPlayerValue = 31000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 45000000;
                            break;
                        case > 6700000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 13000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 30000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 43000000;
                            break;
                        case > 6600000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 10000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 25000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 38000000;
                            break;
                        case > 6400000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 8000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 20000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 30000000;
                            break;
                        case > 6200000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 7000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 15000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 24000000;
                            break;
                        case <= 6200000:
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Goalkeeper")
                                a.TeaPlaPlayerValue = 5000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Defender")
                                a.TeaPlaPlayerValue = 10000000;
                            if (a.TeaPlaPlayerFkNavigation.PlayerPosition == "Midfielder" || a.TeaPlaPlayerFkNavigation.PlayerPosition == "Attacker")
                                a.TeaPlaPlayerValue = 18000000;
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
        static void Main(string[] args)
        {
            string path = @"D:\VS_Projects\FFM_WIFI\JsonFiles\Images\";
            string star = "https://thumbs.dreamstime.com/z/goldener-stern-8482955.jpg";

            Images starImg = new Images(path, star);

            var jsonObject = JsonSerializer.Serialize<Images>(starImg);

            File.WriteAllText(path + "Images.json", jsonObject);

            


        }
    }
}
