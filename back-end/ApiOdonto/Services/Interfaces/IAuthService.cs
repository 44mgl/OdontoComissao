using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.DTOs.Autenticacao;

namespace ApiOdonto.Services.Interfaces;

// recebe e-mail e senha; executa o login; devolve os dados da autenticação; ou devolve null se as credenciais forem inválidas.
public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}