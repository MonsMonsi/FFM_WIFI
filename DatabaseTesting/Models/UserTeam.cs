﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseTesting.Models
{
    public partial class UserTeam
    {
        public int UserTeamPk { get; set; }
        public string UserTeamName { get; set; }
        public int UserTeamLeague { get; set; }
        public int UserTeamSeason { get; set; }
        public int UserTeamUserFk { get; set; }
        public int UserTeamPlayday { get; set; }
        public int UserTeamGk1 { get; set; }
        public int UserTeamDf1 { get; set; }
        public int UserTeamDf2 { get; set; }
        public int UserTeamDf3 { get; set; }
        public int UserTeamDf4 { get; set; }
        public int UserTeamMf1 { get; set; }
        public int UserTeamMf2 { get; set; }
        public int UserTeamMf3 { get; set; }
        public int UserTeamMf4 { get; set; }
        public int UserTeamAt1 { get; set; }
        public int UserTeamAt2 { get; set; }
        public int UserTeamGk2 { get; set; }
        public int UserTeamDf5 { get; set; }
        public int UserTeamMf5 { get; set; }
        public int UserTeamMf6 { get; set; }
        public int UserTeamAt3 { get; set; }
        public int UserTeamAt4 { get; set; }
        public int? UserTeamNumberPlayers { get; set; }

        public virtual User UserTeamUserFkNavigation { get; set; }
    }
}