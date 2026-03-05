using System;
using TS.Model.Enums;

namespace TS.Model.Domain
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}