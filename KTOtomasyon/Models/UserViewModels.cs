﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Models
{
    public class DisplayUsers
    {
        public IList<Users> UsersList { get; set; }

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }
}