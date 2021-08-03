namespace FFM_WIFI.Models.DataJson
{
    public class JsonPlayers
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
            public string Team { get; set; }
            public string Season { get; set; }
        }

        public class Paging
        {
            public int Current { get; set; }
            public int Total { get; set; }
        }

        public class Response
        {
            public Player Player { get; set; }
            public Statistic[] Statistics { get; set; }
        }

        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public int Age { get; set; }
            public Birth Birth { get; set; }
            public string Nationality { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }
            public bool Injured { get; set; }
            public string Photo { get; set; }
        }

        public class Birth
        {
            public string Date { get; set; }
            public string Place { get; set; }
            public string Country { get; set; }
        }

        public class Statistic
        {
            public Team Team { get; set; }
            public League League { get; set; }
            public Games Games { get; set; }
            public Substitutes Substitutes { get; set; }
            public Shots Shots { get; set; }
            public Goals Goals { get; set; }
            public Passes Passes { get; set; }
            public Tackles Tackles { get; set; }
            public Duels Duels { get; set; }
            public Dribbles Dribbles { get; set; }
            public Fouls Fouls { get; set; }
            public Cards Cards { get; set; }
            public Penalty Penalty { get; set; }
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public class League
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string Logo { get; set; }
            public string Flag { get; set; }
            public object Season { get; set; }
        }

        public class Games
        {
            public int? Appearences { get; set; }
            public int? Lineups { get; set; }
            public int? Minutes { get; set; }
            public object Number { get; set; }
            public string Position { get; set; }
            public string Rating { get; set; }
            public bool Captain { get; set; }
        }

        public class Substitutes
        {
            public int _in { get; set; }
            public int _out { get; set; }
            public int? Bench { get; set; }
        }

        public class Shots
        {
            public int? Total { get; set; }
            public int? On { get; set; }
        }

        public class Goals
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
            public int? Accuracy { get; set; }
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
            public object Past { get; set; }
        }

        public class Fouls
        {
            public int? Drawn { get; set; }
            public int? Committed { get; set; }
        }

        public class Cards
        {
            public int? Yellow { get; set; }
            public int? Yellowred { get; set; }
            public int? Red { get; set; }
        }

        public class Penalty
        {
            public object Won { get; set; }
            public object Commited { get; set; }
            public int? Scored { get; set; }
            public int? Missed { get; set; }
            public int? Saved { get; set; }
        }

    }
}
