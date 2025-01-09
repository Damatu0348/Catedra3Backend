using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Src.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } = null!;
    }
}