using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Src.Dtos
{
    public class PostGetDto
    {
        public string Titulo { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string UsuarioEmail { get; set; } = null!;
    }
}
