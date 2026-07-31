using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOdonto.DTOs
{
    public class ErrorResponseDto
    {
        public string Message { get; set; }

        public ErrorResponseDto(string message)
        {
            Message = message;
        }
    }
}