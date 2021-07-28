using System;

namespace FFM_WIFI.Models.DataJson
{
    class JsonFixture
    {
        public class Root
        {
            public string get { get; set; }
            public Parameters parameters { get; set; }
            public object[] errors { get; set; }
            public int results { get; set; }
            public Paging paging { get; set; }
            public Response[] response { get; set; }
        }

        public class Parameters
        {
            public string id { get; set; }
        }

        public class Paging
        {
            public int current { get; set; }
            public int total { get; set; }
        }

        public class Response
        {
            public Fixture fixture { get; set; }
            public League league { get; set; }
            public Teams teams { get; set; }
            public Goals goals { get; set; }
            public Score score { get; set; }
            public Event[] events { get; set; }
            public Lineup[] lineups { get; set; }
            public Statistic[] statistics { get; set; }
            public Player4[] players { get; set; }
        }

        public class Fixture
        {
            public int id { get; set; }
            public string referee { get; set; }
            public string timezone { get; set; }
            public DateTime date { get; set; }
            public int timestamp { get; set; }
            public Periods periods { get; set; }
            public Venue venue { get; set; }
            public Status status { get; set; }
        }

        public class Periods
        {
            public int first { get; set; }
            public int second { get; set; }
        }

        public class Venue
        {
            public int id { get; set; }
            public string name { get; set; }
            public string city { get; set; }
        }

        public class Status
        {
            public string _long { get; set; }
            public string _short { get; set; }
            public int elapsed { get; set; }
        }

        public class League
        {
            public int id { get; set; }
            public string name { get; set; }
            public string country { get; set; }
            public string logo { get; set; }
            public string flag { get; set; }
            public int season { get; set; }
            public string round { get; set; }
        }

        public class Teams
        {
            public Home home { get; set; }
            public Away away { get; set; }
        }

        public class Home
        {
            public int id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
            public bool? winner { get; set; }
        }

        public class Away
        {
            public int id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
            public bool? winner { get; set; }
        }

        public class Goals
        {
            public int home { get; set; }
            public int away { get; set; }
        }

        public class Score
        {
            public Halftime halftime { get; set; }
            public Fulltime fulltime { get; set; }
            public Extratime extratime { get; set; }
            public Penalty penalty { get; set; }
        }

        public class Halftime
        {
            public int home { get; set; }
            public int away { get; set; }
        }

        public class Fulltime
        {
            public int home { get; set; }
            public int away { get; set; }
        }

        public class Extratime
        {
            public object home { get; set; }
            public object away { get; set; }
        }

        public class Penalty
        {
            public object home { get; set; }
            public object away { get; set; }
        }

        public class Event
        {
            public Time time { get; set; }
            public Team team { get; set; }
            public Player player { get; set; }
            public Assist assist { get; set; }
            public string type { get; set; }
            public string detail { get; set; }
            public string comments { get; set; }
        }

        public class Time
        {
            public int elapsed { get; set; }
            public object extra { get; set; }
        }

        public class Team
        {
            public int id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
        }

        public class Player
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Assist
        {
            public int? id { get; set; }
            public string name { get; set; }
        }

        public class Lineup
        {
            public Team1 team { get; set; }
            public string formation { get; set; }
            public Startxi[] startXI { get; set; }
            public Substitute[] substitutes { get; set; }
            public Coach coach { get; set; }
        }

        public class Team1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
            public Colors colors { get; set; }
        }

        public class Colors
        {
            public Player1 player { get; set; }
            public Goalkeeper goalkeeper { get; set; }
        }

        public class Player1
        {
            public string primary { get; set; }
            public string number { get; set; }
            public string border { get; set; }
        }

        public class Goalkeeper
        {
            public string primary { get; set; }
            public string number { get; set; }
            public string border { get; set; }
        }

        public class Coach
        {
            public int? id { get; set; }
            public string name { get; set; }
            public string photo { get; set; }
        }

        public class Startxi
        {
            public Player2 player { get; set; }
        }

        public class Player2
        {
            public int id { get; set; }
            public string name { get; set; }
            public int number { get; set; }
            public string pos { get; set; }
            public string grid { get; set; }
        }

        public class Substitute
        {
            public Player3 player { get; set; }
        }

        public class Player3
        {
            public int id { get; set; }
            public string name { get; set; }
            public int number { get; set; }
            public string pos { get; set; }
            public object grid { get; set; }
        }

        public class Statistic
        {
            public Team2 team { get; set; }
            public Statistic1[] statistics { get; set; }
        }

        public class Team2
        {
            public int id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
        }

        public class Statistic1
        {
            public string type { get; set; }
            public object value { get; set; }
        }

        public class Player4
        {
            public Team3 team { get; set; }
            public Player5[] players { get; set; }
        }

        public class Team3
        {
            public int id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
            public DateTime update { get; set; }
        }

        public class Player5
        {
            public Player6 player { get; set; }
            public Statistic2[] statistics { get; set; }
        }

        public class Player6
        {
            public int id { get; set; }
            public string name { get; set; }
            public string photo { get; set; }
        }

        public class Statistic2
        {
            public Games games { get; set; }
            public int? offsides { get; set; }
            public Shots shots { get; set; }
            public Goals1 goals { get; set; }
            public Passes passes { get; set; }
            public Tackles tackles { get; set; }
            public Duels duels { get; set; }
            public Dribbles dribbles { get; set; }
            public Fouls fouls { get; set; }
            public Cards cards { get; set; }
            public Penalty1 penalty { get; set; }
        }

        public class Games
        {
            public int? minutes { get; set; }
            public int number { get; set; }
            public string position { get; set; }
            public string rating { get; set; }
            public bool captain { get; set; }
            public bool substitute { get; set; }
        }

        public class Shots
        {
            public int? total { get; set; }
            public int? on { get; set; }
        }

        public class Goals1
        {
            public int? total { get; set; }
            public int conceded { get; set; }
            public int? assists { get; set; }
            public int? saves { get; set; }
        }

        public class Passes
        {
            public int? total { get; set; }
            public int? key { get; set; }
            public string accuracy { get; set; }
        }

        public class Tackles
        {
            public int? total { get; set; }
            public int? blocks { get; set; }
            public int? interceptions { get; set; }
        }

        public class Duels
        {
            public int? total { get; set; }
            public int? won { get; set; }
        }

        public class Dribbles
        {
            public int? attempts { get; set; }
            public int? success { get; set; }
            public int? past { get; set; }
        }

        public class Fouls
        {
            public int? drawn { get; set; }
            public int? committed { get; set; }
        }

        public class Cards
        {
            public int yellow { get; set; }
            public int red { get; set; }
        }

        public class Penalty1
        {
            public object won { get; set; }
            public object commited { get; set; }
            public int scored { get; set; }
            public int missed { get; set; }
            public int? saved { get; set; }
        }
    }
}
