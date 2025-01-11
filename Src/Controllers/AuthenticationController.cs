using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Dtos;
using api.Src.Interfaces;
using api.Src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Usuario> _signInManager;

        /// <summary>
        /// Constructor de autenticacion COntroller
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        /// <param name="signInManager"></param>
        public AuthenticationController(
            UserManager<Usuario> userManager,
            ITokenService tokenService,
            SignInManager<Usuario> signInManager
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Metodo que registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="registerDto">el modelo de registro del usuario a ingresar</param>
        /// <returns>Ok si se registro exitosamente, error 500 de lo contrario</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioRegister = new Usuario
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                };

                if (string.IsNullOrEmpty(registerDto.Password))
                {
                    return BadRequest("La contraseña no DEBE estar vacia.");
                }
                var usuarioCrear = await _userManager.CreateAsync(
                    usuarioRegister,
                    registerDto.Password
                );
                if (usuarioCrear.Succeeded)
                {
                    var roleCrear = await _userManager.AddToRoleAsync(usuarioRegister, "User");
                    if (roleCrear.Succeeded)
                    {
                        return Ok(
                            new NuevoUsuarioDto
                            {
                                Email = usuarioRegister.Email,
                                Token = _tokenService.CreateToken(usuarioRegister),
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleCrear.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, usuarioCrear.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Metodo para ingresar a sesion o Logearse como usuario existente en el sistema
        /// </summary>
        /// <param name="loginDto">modelo de inicio de sesion para usuario</param>
        /// <returns>Ok si se pudo logear con exito, error 500 de lo contrario</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var usuarioCorreo = await _userManager.Users.FirstOrDefaultAsync(x =>
                    x.Email == loginDto.Email
                );
                if (usuarioCorreo == null)
                {
                    return Unauthorized("Correo invalido");
                }
                var usuarioContrasenha = await _signInManager.CheckPasswordSignInAsync(
                    usuarioCorreo,
                    loginDto.Password,
                    false
                );
                if (!usuarioContrasenha.Succeeded)
                {
                    return Unauthorized("Contraseña incorrecta");
                }
                return Ok(
                    new NuevoUsuarioDto
                    {
                        Email = usuarioCorreo.Email!,
                        Token = _tokenService.CreateToken(usuarioCorreo),
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
