using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI.Models
{
    public class Track
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAttendanceDaily { get; set; }
        public int SchoolID { get; set; }
        public int DaysInCycle { get; set; }
        public int BlocksPerDay { get; set; }
        public int DailyBlocksPerDay { get; set; }
        public int EffortLegendID { get; set; }

    }
}