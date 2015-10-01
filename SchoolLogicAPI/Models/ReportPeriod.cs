using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI.Models
{
    public class ReportPeriod
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ID { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public int SchoolGovId { get; set; }
        public int SchoolInternalId { get; set; }
        public int DaysOpenBeforeEnd { get; set; }
        public int DaysOpenAfterEnd { get; set; }
        public DateTime DateOpens
        {
            get
            {
                return EndDate.AddDays(this.DaysOpenBeforeEnd * -1);
            }
        }
        public DateTime DateCloses
        {
            get
            {
                return EndDate.AddDays(this.DaysOpenAfterEnd);
            }
        }

        // Parameterless constructor for serialization
        public ReportPeriod() { }
    }
}