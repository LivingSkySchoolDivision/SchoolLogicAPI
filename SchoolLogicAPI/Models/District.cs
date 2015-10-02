using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI.Models
{
    public class District
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AlternateCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}