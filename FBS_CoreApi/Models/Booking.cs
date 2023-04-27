using FlightBooking.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FBS_CoreApi.Data;

namespace FBS_CoreApi.Models
{
    public class Booking
    {
        //primary key
        public int Id { get; set; }

        //foreign key
        public string PassengerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string CabinClass { get; set; }
        public int NoOfTicket { get; set; }
        public decimal TotalPrice { get; set; }

        [Required]
        public int FlightId { get; set; }
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        
    }
}
