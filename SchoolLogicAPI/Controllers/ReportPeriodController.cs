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
    public class ReportPeriodController : ApiController
    {
        private ReportPeriodRepository repository = new ReportPeriodRepository();

        public IEnumerable<ReportPeriod> Get()
        {
            return repository.GetAll();
        }
    }
}
