﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseTesting.Models
{
    public partial class TeamPlayerAssignment
    {
        public int TeaPlaTeamFk { get; set; }
        public int TeaPlaPlayerFk { get; set; }
        public int TeaPlaSeasonFk { get; set; }
        public double TeaPlaPlayerRating { get; set; }
        public int TeaPlaPlayerValue { get; set; }

        public virtual Player TeaPlaPlayerFkNavigation { get; set; }
        public virtual Season TeaPlaSeasonFkNavigation { get; set; }
        public virtual SeasonLeagueTeamAssignment TeaPlaTeamFkNavigation { get; set; }
    }
}