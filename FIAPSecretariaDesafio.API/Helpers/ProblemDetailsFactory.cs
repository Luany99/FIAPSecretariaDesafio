using FIAPSecretariaDesafio.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FIAPSecretariaDesafio.API.Helpers
{
    public class ProblemDetailsFactory : IProblemDetailsFactory
    {
        public ProblemDetails CreateProblemDetails(
            HttpContext context,
            int? statusCode = null,
            string? title = null,
            string? detail = null,
            string? type = null,
            string? instance = null)
        {
            statusCode ??= 500;

            return new ProblemDetails
            {
                Status = statusCode.Value,
                Title = title ?? GetDefaultTitle(statusCode.Value),
                Detail = detail,
                Type = type,
                Instance = instance
            };
        }

        public ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext context,
            ModelStateDictionary modelStateDictionary)
        {
            var details = new ValidationProblemDetails(modelStateDictionary)
            {
                Type = "https://example.com/probs/validation",
                Title = "Um ou mais erros de validação ocorreram.",
                Status = 400
            };

            return details;
        }

        private static string GetDefaultTitle(int statusCode)
        {
            return statusCode switch
            {
                400 => "Requisição inválida",
                401 => "Não autorizado",
                403 => "Proibido",
                404 => "Recurso não encontrado",
                409 => "Conflito",
                500 => "Erro interno do servidor",
                _ => "Erro"
            };
        }
    }
}
