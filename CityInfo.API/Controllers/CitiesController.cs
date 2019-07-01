using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class Controllers: Controller
    {
        [HttpGet]
        public ActionResult GetCities()
        {
               return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetCity(int id)
        {
               var city = CitiesDataStore.Current.Cities.Where(c=>c.Id==id).FirstOrDefault();
               if(city == null)
               {
                   return NotFound();
               }
               return Ok(city);
        }

    }

}