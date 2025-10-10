using System.ComponentModel.DataAnnotations;

namespace Company.G05.PL.DTOs
{
    public class ForgetPasswordDTO
    {
        [Required(ErrorMessage = "Email is Required !")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
