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
    public class SchoolController : ApiController
    {
        private SchoolRepository repository = new SchoolRepository();

        public IEnumerable<School> Get()
        {
            return repository.GetAll();
        }

        public School Get(int id)
        {
            return repository.Get(id);
        }
    }
}
