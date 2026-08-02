using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiOdonto.DTOs.VIPs;

namespace ApiOdonto.Services.Interfaces
{
    public interface IMembroVipAuthService
    {
        Task<LoginVipResponseDto?> LoginAsync(LoginVipRequestDto dto);
    }
}