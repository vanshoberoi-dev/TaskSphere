using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Contract.DTOs.Auth
{
    public class LoginUserResponseDTO
    {
        public required string Token { get; set; }
    }
}
