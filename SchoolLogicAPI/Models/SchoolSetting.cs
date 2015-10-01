using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI.Models
{
    public class SchoolSetting
    {
        public int SettingID { get; set; }
        public char DataTypeIndicator { get; set; } 
        public int SchoolDatabaseID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}