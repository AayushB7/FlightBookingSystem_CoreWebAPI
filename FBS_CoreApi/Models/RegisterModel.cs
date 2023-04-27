using System.ComponentModel.DataAnnotations;

namespace FBS_CoreApi.Model
{
    public class RegisterModel
    {
     /*   [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }*/

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}