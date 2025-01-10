using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Models;

namespace api.Src.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Usuario usuario);
    }
}