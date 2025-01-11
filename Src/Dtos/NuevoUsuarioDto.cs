using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Src.Dtos
{
    public class NuevoUsuarioDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
