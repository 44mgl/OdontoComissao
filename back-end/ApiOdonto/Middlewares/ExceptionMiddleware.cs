using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ApiOdonto.DTOs;
using ApiOdonto.Exceptions;
using System.Text.Json;

namespace ApiOdonto.Middlewares
{
    public class ExceptionMiddleware
    {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionMiddleware> _logger;

            public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (RegraNegocioException ex)
                {
                    _logger.LogWarning(ex, "Regra de negocio violada.");

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var resposta = new ErrorResponseDto(ex.Message);
                    var json = JsonSerializer.Serialize(resposta);

                    await context.Response.WriteAsync(json);
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogWarning(ex, "Acesso nao autorizado.");

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var resposta = new ErrorResponseDto("Não autorizado.");
                    var json = JsonSerializer.Serialize(resposta);

                    await context.Response.WriteAsync(json);
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogWarning(ex, "Recurso não encontrado.");

                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Response.ContentType = "application/json";

                    var resposta = new ErrorResponseDto("Não Encontrado.");

                    var json = JsonSerializer.Serialize(resposta);
                    await context.Response.WriteAsync(json);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocorreu um erro não tratado.");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new ErrorResponseDto("Ocorreu um erro interno no servidor.");

                    var json = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(json);
                }
        }

    }
}
