﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace FFM_WIFI.Models.DataContext
{
    public partial class User
    {
        public User()
        {
            UserPlayerAssignment = new HashSet<UserPlayerAssignment>();
        }

        public int UserPk { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public virtual ICollection<UserPlayerAssignment> UserPlayerAssignment { get; set; }
    }
}