using System.ComponentModel.DataAnnotations;

namespace FBS_CoreApi.DTOs
{
    public class BookingDTO
    {
        [Required]
        public string PassengerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }
        public string CabinClass { get; set; }

        [Required]
        public int NoOfTicket { get; set; }

        
    }
}