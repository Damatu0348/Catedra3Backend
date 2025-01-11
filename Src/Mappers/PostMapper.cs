using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Dtos;
using api.Src.Models;

namespace api.Src.Mappers
{
    public static class PostMapper
    {
        public static Post ToModel(PostCreateDto dto, int userId)
        {
            return new Post
            {
                Titulo = dto.Titulo,
                FechaPublicacion = DateTime.UtcNow,
                ImageUrl = dto.ImageUrl,
                UsuarioId = userId,
            };
        }

        public static PostCreateDto ToDto(Post post)
        {
            return new PostCreateDto
            {
                Titulo = post.Titulo,
                FechaPublicacion = post.FechaPublicacion,
                ImageUrl = post.ImageUrl,
                UsuarioEmail = post.Usuario.Email!,
            };
        }
    }
}
