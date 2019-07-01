using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private IMailService mailService;
        private CityInfoContext dbContext;
        public PointsOfInterestController(IMailService mailService, CityInfoContext ctx){
          this.mailService = mailService;
          dbContext = ctx;
        }
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);


            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult PointsOfInterest(int cityId, [FromBody] PointOfInterestCreationDto PointOfInterestCreationDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var currentPointOfInterest = city.NumberOfPointsOfInterest;

            var pointOfInterest = new PointOfInterestDto()
            {
                Id= ++currentPointOfInterest,
                Name = PointOfInterestCreationDto.Name,
                Description = PointOfInterestCreationDto.Description
            };

            city.PointsOfInterest.Add(pointOfInterest);

          return CreatedAtRoute("GetPointOfInterest", new {cityId=city.Id, id=currentPointOfInterest}, pointOfInterest);
        }
    
        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult PointsOfInterest(int cityId, int id, [FromBody] PointOfInterestCreationDto PointOfInterestCreationDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var poitntOfInterest = city.PointsOfInterest.FirstOrDefault(c=>c.Id == id);
             if(poitntOfInterest == null){
                 return NotFound();
             }

             poitntOfInterest.Name= PointOfInterestCreationDto.Name;
             poitntOfInterest.Description= PointOfInterestCreationDto.Description;
          return NoContent();
        }
    
        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartialPointsOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestCreationDto> PointOfInterestCreationDto){
         if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var poitntOfInterest = city.PointsOfInterest.FirstOrDefault(c=>c.Id == id);
             if(poitntOfInterest == null){
                 return NotFound();
             }
             var poitntOfInterestToUpdate = new PointOfInterestCreationDto(){
                 Name=poitntOfInterest.Name,
                 Description=poitntOfInterest.Description
             };

             PointOfInterestCreationDto.ApplyTo(poitntOfInterestToUpdate, ModelState);
             poitntOfInterest.Name = poitntOfInterestToUpdate.Name;
             poitntOfInterest.Description = poitntOfInterestToUpdate.Description;

          return NoContent();
        }


        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult PointsOfInterest(int cityId, int id)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var poitntOfInterest = city.PointsOfInterest.FirstOrDefault(c=>c.Id == id);
             if(poitntOfInterest == null){
                 return NotFound();
             }

             city.PointsOfInterest.Remove(poitntOfInterest);
             mailService.Send("Deleting", "email");
          return NoContent();
        }
    }
}