using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Auth
{
    public class LoginUserRequestDTO
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, StringLength(100,MinimumLength = 8)]
        public required string Password { get; set; }
    }
}