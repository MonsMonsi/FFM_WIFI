using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFM_WIFI.Models.DataJson
{
    public class JsonStandings
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
            public League League { get; set; }
        }

        public class League
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string Logo { get; set; }
            public string Flag { get; set; }
            public int Season { get; set; }
            public Standing[][] Standings { get; set; }
        }

        public class Standing
        {
            public int Rank { get; set; }
            public Team Team { get; set; }
            public int Points { get; set; }
            public int GoalsDiff { get; set; }
            public string Group { get; set; }
            public string Form { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
            public All All { get; set; }
            public Home Home { get; set; }
            public Away Away { get; set; }
            public DateTime Update { get; set; }
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public class All
        {
            public int Played { get; set; }
            public int Win { get; set; }
            public int Draw { get; set; }
            public int Lose { get; set; }
            public Goals Goals { get; set; }
        }

        public class Goals
        {
            public int For { get; set; }
            public int Against { get; set; }
        }

        public class Home
        {
            public int Played { get; set; }
            public int Win { get; set; }
            public int Draw { get; set; }
            public int Lose { get; set; }
            public Goals1 Goals { get; set; }
        }

        public class Goals1
        {
            public int _for { get; set; }
            public int Against { get; set; }
        }

        public class Away
        {
            public int Played { get; set; }
            public int Win { get; set; }
            public int Draw { get; set; }
            public int Lose { get; set; }
            public Goals2 Goals { get; set; }
        }

        public class Goals2
        {
            public int _for { get; set; }
            public int Against { get; set; }
        }

    }
}
