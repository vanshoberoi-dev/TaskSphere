using System.ComponentModel.DataAnnotations;

namespace TS.Contract.DTOs.Auth
{
    public class DeleteUserRequestDTO
    {
        [Required]
        public required int UserId { get; set; }

    }
}
