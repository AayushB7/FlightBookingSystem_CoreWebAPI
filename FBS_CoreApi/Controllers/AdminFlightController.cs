using FlightBooking.DTOs;
using FlightBooking.Models;
using FlightBooking.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepo _adminRepo;


        public AdminController(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;

        }

        [HttpGet("flights")]

        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            var flights = await _adminRepo.GetFlights();
            return Ok(flights);
        }

        [HttpGet("flights/{number}")]
        public async Task<ActionResult<Flight>> GetFlightByNumber(string number)
        {
            var flight = await _adminRepo.GetFlightByNumber(number);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost("flights")]
        public async Task<ActionResult<Flight>> CreateFlight(AdminFlightDto flightDto)
        {
            var flight = await _adminRepo.CreateFlight(flightDto);
            return Ok(flight);
        }

        [HttpPut("flights/{id}")]
        public async Task<ActionResult> UpdateFlight(int id, AdminFlightDto flightDto)
        {
            await _adminRepo.UpdateFlight(id, flightDto);
            return NoContent();
        }

        [HttpDelete("flights/{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            await _adminRepo.DeleteFlight(id);
            return NoContent();
        }
    }
}