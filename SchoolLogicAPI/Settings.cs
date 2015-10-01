﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI
{
    public static class Settings
    {
        public static string DatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SchoolLogicDatabase"].ConnectionString; }
        }
    }
}