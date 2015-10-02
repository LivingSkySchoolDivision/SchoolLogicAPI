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
    public class TermController : ApiController
    {
        private TermRepository repository = new TermRepository();

        public IEnumerable<Term> Get()
        {
            return repository.GetAll();
        }

        public Term Get(int id)
        {
            return repository.Get(id);
        }
    }
}
