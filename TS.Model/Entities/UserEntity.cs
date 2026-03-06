using System;
using TS.Model.Common;
using TS.Model.Enums;

namespace TS.Model.Domain
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}