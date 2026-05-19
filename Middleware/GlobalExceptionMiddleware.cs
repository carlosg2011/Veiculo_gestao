using Microsoft.AspNetCore.Mvc;

namespace Gestao_veiculos.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção não tratada: {Method} {Path}", context.Request.Method, context.Request.Path);

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title  = "Erro interno do servidor.",
                    Detail = _env.IsDevelopment() ? ex.Message : "Ocorreu um erro interno. Tente novamente mais tarde."
                };

                if (_env.IsDevelopment())
                    problem.Extensions["stackTrace"] = ex.StackTrace;

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
