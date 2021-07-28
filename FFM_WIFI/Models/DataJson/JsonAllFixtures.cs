using System;

namespace FFM_WIFI.Models.DataJson
{
    class JsonAllFixtures
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
            public string League { get; set; }
            public string Season { get; set; }
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
            public int First { get; set; }
            public int Second { get; set; }
        }

        public class Venue
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
        }

        public class Status
        {
            public string _long { get; set; }
            public string _short { get; set; }
            public int Elapsed { get; set; }
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
            public int Home { get; set; }
            public int Away { get; set; }
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
            public int Home { get; set; }
            public int Away { get; set; }
        }

        public class Fulltime
        {
            public int Home { get; set; }
            public int Away { get; set; }
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
    }
}
