using FBS_CoreApi.DTOs;
using FBS_CoreApi.Repos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBS_CoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepo _searchRepo;

       
        public SearchController(ISearchRepo searchRepo)
        {
            _searchRepo = searchRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<FlightDto>>> GetFlights(string departureAirport, string arrivalAirport, DateTime departureTime)
        {
            var flights = await _searchRepo.GetFlightsAsync(departureAirport, arrivalAirport, departureTime);
            if (flights.Count == 0)
            {
                return NotFound("No flights available.");
            }
            return flights;
        }
    }
}