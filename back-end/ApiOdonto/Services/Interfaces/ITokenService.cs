using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.Models;

namespace ApiOdonto.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Administrador administrador);
    }
}