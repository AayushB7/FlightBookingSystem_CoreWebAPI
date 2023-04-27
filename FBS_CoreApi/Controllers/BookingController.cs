using System.Security.Claims;
using System.Threading.Tasks;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBS_CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepo;

        public BookingController(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        [HttpPost("{flightId}")]
        [Authorize] // Require authentication for this action
        public async Task<IActionResult> BookFlight(int flightId, BookingDTO booking)
        {
            // Get the user ID of the currently authenticated user
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Call the BookFlight method with the flight ID, booking details, and user ID
            var result = await _bookingRepo.BookFlight(flightId, booking, userId);

            // Check if the booking was successful
            if (result.IsSuccessful)
            {
                return Ok(new { message = "Booking successful" });
            }
            else
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
        }

        [HttpDelete("{bookingId}")]
        [Authorize] // Require authentication for this action
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            // Get the user ID of the currently authenticated user
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Call the CancelBooking method with the booking ID and user ID
            var result = await _bookingRepo.CancelBooking(bookingId, userId);

            // Check if the cancellation was successful
            if (result.IsSuccessful)
            {
                return Ok(new { message = "Booking cancelled successfully" });
            }
            else
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
        }
    }
}