using System.ComponentModel.DataAnnotations;

namespace FlightBooking.DTOs
{
    public class AdminFlightDto
    {
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public string DepartureAirport { get; set; }
        [Required]
        public string ArrivalAirport { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}