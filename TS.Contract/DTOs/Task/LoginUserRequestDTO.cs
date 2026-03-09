using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Task
{
    public class LoginUserRequestDTO
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}