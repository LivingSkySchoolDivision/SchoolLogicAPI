using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolLogicAPI.Models;
using SchoolLogicAPI.Repositories;

namespace SchoolLogicAPI.Controllers
{
    public class ActiveReportPeriodsController : ApiController
    {
        private ReportPeriodRepository repository = new ReportPeriodRepository();

        public IEnumerable<ReportPeriod> Get()
        {
            // Find report periods currently in progress
            List<ReportPeriod> allReportPeriods = repository.GetAll();

            // We only need report periods that are currently in progress
            List<ReportPeriod> activeReportPeriods = allReportPeriods.Where(
                r => r.StartDate <= DateTime.Now &&
                    r.DateCloses >= DateTime.Now).OrderBy(r => r.DateOpens).ToList();

            return activeReportPeriods;

        }
        
        public ReportPeriod Get(int id)
        {
            return repository.Get(id);
        }
    }
}
