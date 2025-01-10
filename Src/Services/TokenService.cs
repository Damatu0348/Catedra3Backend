using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Src.Interfaces;
using api.Src.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Src.Services
{
    public class TokenService : ITokenService
    {
        
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            string signingKey = _config["JWT:SigningKey"] ?? throw new Exception();
            byte[] key = Encoding.UTF8.GetBytes(signingKey) ?? throw new Exception();
            _key = new SymmetricSecurityKey(key);
        }

        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Email,
                    usuario.Email
                        ?? throw new ArgumentNullException(
                            nameof(usuario.Email),
                            "User email cannot be null"
                        )
                ),
                new Claim(
                    JwtRegisteredClaimNames.GivenName,
                    usuario.UserName
                        ?? throw new ArgumentNullException(
                            nameof(usuario.UserName),
                            "User name cannot be null"
                        )
                ),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}