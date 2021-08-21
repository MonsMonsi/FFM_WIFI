using System;

namespace FFM_WIFI.Models.DataJson
{
    public class JsonFixture
    {
        public class Root
        {
            public string Get { get; set; }
            public Parameters Parameters { get; set; }
            public object[] Errors { get; set; }
            public int Results { get; set; }
            public Paging Paging { get; set; }
            public Response[] Response { get; set; }
        }

        public class Parameters
        {
            public string Id { get; set; }
        }

        public class Paging
        {
            public int Current { get; set; }
            public int Total { get; set; }
        }

        public class Response
        {
            public Fixture Fixture { get; set; }
            public League League { get; set; }
            public Teams Teams { get; set; }
            public Goals Goals { get; set; }
            public Score Score { get; set; }
            public Event[] Events { get; set; }
            public Lineup[] Lineups { get; set; }
            public Statistic[] Statistics { get; set; }
            public Player4[] Players { get; set; }
        }

        public class Fixture
        {
            public int Id { get; set; }
            public string Referee { get; set; }
            public string Timezone { get; set; }
            public DateTime Date { get; set; }
            public int Timestamp { get; set; }
            public Periods Periods { get; set; }
            public Venue Venue { get; set; }
            public Status Status { get; set; }
        }

        public class Periods
        {
            public int? First { get; set; }
            public int? Second { get; set; }
        }

        public class Venue
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
        }

        public class Status
        {
            public string Long { get; set; }
            public string Short { get; set; }
            public int? Elapsed { get; set; }
        }

        public class League
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string Logo { get; set; }
            public string Flag { get; set; }
            public int Season { get; set; }
            public string Round { get; set; }
        }

        public class Teams
        {
            public Home Home { get; set; }
            public Away Away { get; set; }
        }

        public class Home
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
            public bool? Winner { get; set; }
        }

        public class Away
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
            public bool? Winner { get; set; }
        }

        public class Goals
        {
            public int? Home { get; set; }
            public int? Away { get; set; }
        }

        public class Score
        {
            public Halftime Halftime { get; set; }
            public Fulltime Fulltime { get; set; }
            public Extratime Extratime { get; set; }
            public Penalty Penalty { get; set; }
        }

        public class Halftime
        {
            public int? Home { get; set; }
            public int? Away { get; set; }
        }

        public class Fulltime
        {
            public int? Home { get; set; }
            public int? Away { get; set; }
        }

        public class Extratime
        {
            public object Home { get; set; }
            public object Away { get; set; }
        }

        public class Penalty
        {
            public object Home { get; set; }
            public object Away { get; set; }
        }

        public class Event
        {
            public Time Time { get; set; }
            public Team Team { get; set; }
            public Player Player { get; set; }
            public Assist Assist { get; set; }
            public string Type { get; set; }
            public string Detail { get; set; }
            public string Comments { get; set; }
        }

        public class Time
        {
            public int? Elapsed { get; set; }
            public object Extra { get; set; }
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public class Player
        {
            public int? Id { get; set; }
            public string Name { get; set; }
        }

        public class Assist
        {
            public int? Id { get; set; }
            public string Name { get; set; }
        }

        public class Lineup
        {
            public Team1 Team { get; set; }
            public string Formation { get; set; }
            public Startxi[] StartXI { get; set; }
            public Substitute[] Substitutes { get; set; }
            public Coach Coach { get; set; }
        }

        public class Team1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
            public Colors Colors { get; set; }
        }

        public class Colors
        {
            public Player1 Player { get; set; }
            public Goalkeeper Goalkeeper { get; set; }
        }

        public class Player1
        {
            public string Primary { get; set; }
            public string Number { get; set; }
            public string Border { get; set; }
        }

        public class Goalkeeper
        {
            public string Primary { get; set; }
            public string Number { get; set; }
            public string Border { get; set; }
        }

        public class Coach
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string Photo { get; set; }
        }

        public class Startxi
        {
            public Player2 Player { get; set; }
        }

        public class Player2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Number { get; set; }
            public string Pos { get; set; }
            public string Grid { get; set; }
        }

        public class Substitute
        {
            public Player3 Player { get; set; }
        }

        public class Player3
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Number { get; set; }
            public string Pos { get; set; }
            public object Grid { get; set; }
        }

        public class Statistic
        {
            public Team2 Team { get; set; }
            public Statistic1[] Statistics { get; set; }
        }

        public class Team2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public class Statistic1
        {
            public string Type { get; set; }
            public object Value { get; set; }
        }

        public class Player4
        {
            public Team3 Team { get; set; }
            public Player5[] Players { get; set; }
        }

        public class Team3
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
            public DateTime Update { get; set; }
        }

        public class Player5
        {
            public Player6 Player { get; set; }
            public Statistic2[] Statistics { get; set; }
        }

        public class Player6
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Photo { get; set; }
        }

        public class Statistic2
        {
            public Games Games { get; set; }
            public int? Offsides { get; set; }
            public Shots Shots { get; set; }
            public Goals1 Goals { get; set; }
            public Passes Passes { get; set; }
            public Tackles Tackles { get; set; }
            public Duels Duels { get; set; }
            public Dribbles Dribbles { get; set; }
            public Fouls Fouls { get; set; }
            public Cards Cards { get; set; }
            public Penalty1 Penalty { get; set; }
        }

        public class Games
        {
            public int? Minutes { get; set; }
            public int Number { get; set; }
            public string Position { get; set; }
            public string Rating { get; set; }
            public bool Captain { get; set; }
            public bool Substitute { get; set; }
        }

        public class Shots
        {
            public int? Total { get; set; }
            public int? On { get; set; }
        }

        public class Goals1
        {
            public int? Total { get; set; }
            public int? Conceded { get; set; }
            public int? Assists { get; set; }
            public int? Saves { get; set; }
        }

        public class Passes
        {
            public int? Total { get; set; }
            public int? Key { get; set; }
            public string Accuracy { get; set; }
        }

        public class Tackles
        {
            public int? Total { get; set; }
            public int? Blocks { get; set; }
            public int? Interceptions { get; set; }
        }

        public class Duels
        {
            public int? Total { get; set; }
            public int? Won { get; set; }
        }

        public class Dribbles
        {
            public int? Attempts { get; set; }
            public int? Success { get; set; }
            public int? Past { get; set; }
        }

        public class Fouls
        {
            public int? Drawn { get; set; }
            public int? Committed { get; set; }
        }

        public class Cards
        {
            public int? Yellow { get; set; }
            public int? Red { get; set; }
        }

        public class Penalty1
        {
            public object Won { get; set; }
            public object Commited { get; set; }
            public int? Scored { get; set; }
            public int? Missed { get; set; }
            public int? Saved { get; set; }
        }
    }
}
