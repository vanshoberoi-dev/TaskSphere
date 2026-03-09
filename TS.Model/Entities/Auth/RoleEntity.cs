using System.ComponentModel.DataAnnotations;

namespace TS.Model.Entities.Auth
{
    public class RoleEntity : BaseEntity
    {

        [Required]
        public required string Role { get; set; }
    }
}