using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolLogicAPI
{
    public class Helpers
    {
        public static string TimeUntil(DateTime date)
        {
            TimeSpan timePeriod = date.Subtract(DateTime.Today);
            
            int daysUntil = timePeriod.Days;
            
            string suffix = "";
            string prefix = "";
            
            if (daysUntil == 0)
            {
                return "Today";
            }

            if (daysUntil == 1)
            {
                return "Tomorrow";
            }

            if (daysUntil == -1)
            {
                return "Yesterday";
            }

            if (daysUntil > 0)
            {
                prefix = "in ";
            }
            else
            {
                suffix = " ago";
            }


            return prefix + Math.Abs(daysUntil) + " days" + suffix;
        }

        public static string TimeSince(DateTime date)
        {
            return TimeUntil(date);
        }
    }
}