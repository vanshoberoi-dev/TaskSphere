using System.ComponentModel.DataAnnotations.Schema;

namespace TS.Model.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public RoleEntity Roles { get; set; }
    }
}