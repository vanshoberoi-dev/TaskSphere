using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Auth
{
    public class RegisterUserRequestDTO
    {
        public string? Name { get; set; }

        [Required, EmailAddress, StringLength(512)]
        public required string Email { get; set; }

        [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)]
        public required string Password { get; set; }

        public int RoleId { get; set; }
    }
}