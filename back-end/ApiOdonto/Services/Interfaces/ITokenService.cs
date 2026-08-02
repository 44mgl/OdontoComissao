using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.Models;

namespace ApiOdonto.Services.Interfaces
{
    public interface ITokenService
    {
        // Qualquer classe que seja um serviço de token precisa receber um administrador e devolver um token em formato de texto.
        string GenerateToken(Administrador administrador); 

        string GenerateToken(MembroVip membroVip);
    }
}