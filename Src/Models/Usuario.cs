using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Src.Models
{
    public class Usuario : IdentityUser
    {
        [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public new string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^[a-zA-Z0-9]{8,20}$",
            ErrorMessage = "La contraseña debe ser alfanumérica y tener entre 8 y 20 caracteres."
        )]
        public string Password { get; set; } = string.Empty;
    }
}