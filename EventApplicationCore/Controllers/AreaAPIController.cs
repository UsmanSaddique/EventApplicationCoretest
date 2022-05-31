using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApplicationCore.Controllers
{
    [Route("api/[controller]")]
    public class AreaAPIController
    {
        

        

            private readonly DatabaseContext dbcontext;

            public AreaAPIController(DatabaseContext db)
            {
                dbcontext = db;
            }

            // POST api/values
            [HttpPost]
            public List<Area> Post(string id)
            {
                try
                {
                var ListArea = dbcontext.Area.Where(x => x.CityId == Convert.ToInt32(id)).ToList() ;
                    return ListArea;
                }
                catch (Exception)
                {
                    throw;
                }
            }
         
        [HttpGet]
        public IEnumerable<Area> Get()
        {
            try
            {
                var listofCountry = dbcontext.Area.ToList();
                listofCountry.Insert(0, new Area { Id = 0, AreaName = "----Select----" });
                return listofCountry;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
