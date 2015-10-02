using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI.Models
{
    public class Term
    {
        public int ID { get; set; }
        public int TrackID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public int SchoolID { get; set; }
    }
}