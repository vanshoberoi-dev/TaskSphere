using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TS.Model.Entities.Auth
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(256), EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(500)]
        public string PasswordHash { get; set; }

        public int RoleId { get; set; } = 1; // Default to "User" role

        [ForeignKey("RoleId")]
        public virtual RoleEntity Role { get; set; }
    }
}