using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Auth
{
    public class CreateRoleRequestDTO
    {
        [Required, StringLength(50)]
        public required string Role { get; set; } = "user";
    }
}