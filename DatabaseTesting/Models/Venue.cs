﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseTesting.Models
{
    public partial class Venue
    {
        public Venue()
        {
            Team = new HashSet<Team>();
        }

        public int VenuePk { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public string VenueCity { get; set; }
        public int VenueCapacity { get; set; }
        public string VenueImage { get; set; }

        public virtual ICollection<Team> Team { get; set; }
    }
}