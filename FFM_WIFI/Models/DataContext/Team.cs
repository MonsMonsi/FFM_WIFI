// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace FFM_WIFI.Models.DataContext
{
    public partial class Team
    {
        public Team()
        {
            SeasonLeagueTeamAssignment = new HashSet<SeasonLeagueTeamAssignment>();
        }

        public int TeamPk { get; set; }
        public string TeamName { get; set; }
        public int TeamFounded { get; set; }
        public string TeamLogo { get; set; }
        public int TeamVenueFk { get; set; }

        public virtual Venue TeamVenueFkNavigation { get; set; }
        public virtual ICollection<SeasonLeagueTeamAssignment> SeasonLeagueTeamAssignment { get; set; }
    }
}