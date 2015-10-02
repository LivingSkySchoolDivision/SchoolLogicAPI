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
    public class TrackController : ApiController
    {
        private TrackRepository repository = new TrackRepository();

        public IEnumerable<Track> Get()
        {
            return repository.GetAll();
        }
        
        public Track Get(int id)
        {
            return repository.Get(id);
        }
    }
}
