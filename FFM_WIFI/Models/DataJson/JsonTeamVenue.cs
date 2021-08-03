namespace FFM_WIFI.Models.DataJson
{
    public class JsonTeamVenue
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
            public Team Team { get; set; }
            public Venue Venue { get; set; }
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public int Founded { get; set; }
            public bool National { get; set; }
            public string Logo { get; set; }
        }

        public class Venue
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public int Capacity { get; set; }
            public string Surface { get; set; }
            public string Image { get; set; }
        }
    }
}

