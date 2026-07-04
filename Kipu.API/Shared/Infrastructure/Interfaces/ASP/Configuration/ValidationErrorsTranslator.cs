using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Shared.Infrastructure.Interfaces.ASP.Configuration;

/// <summary>
/// Helper utility to translate framework-level model validation and deserialization error messages.
/// </summary>
public static class ValidationErrorsTranslator
{
    public static void TranslateProblemDetails(ProblemDetails problemDetails)
    {
        if (problemDetails == null) return;

        if (!string.IsNullOrEmpty(problemDetails.Title))
        {
            problemDetails.Title = TranslateTitle(problemDetails.Title, problemDetails.Status);
        }

        if (!string.IsNullOrEmpty(problemDetails.Detail))
        {
            problemDetails.Detail = TranslateDetail(problemDetails.Detail, problemDetails.Status);
        }
    }

    private static string TranslateTitle(string title, int? status)
    {
        if (string.Equals(title, "One or more validation errors occurred.", StringComparison.OrdinalIgnoreCase))
        {
            return "Ocurrieron uno o más errores de validación.";
        }

        switch (status)
        {
            case 400:
                return string.Equals(title, "Bad Request", StringComparison.OrdinalIgnoreCase) ? "Solicitud Incorrecta" : title;
            case 401:
                return string.Equals(title, "Unauthorized", StringComparison.OrdinalIgnoreCase) ? "No Autorizado" : title;
            case 403:
                return string.Equals(title, "Forbidden", StringComparison.OrdinalIgnoreCase) ? "Prohibido" : title;
            case 404:
                return string.Equals(title, "Not Found", StringComparison.OrdinalIgnoreCase) ? "No Encontrado" : title;
            case 405:
                return string.Equals(title, "Method Not Allowed", StringComparison.OrdinalIgnoreCase) ? "Método No Permitido" : title;
            case 406:
                return string.Equals(title, "Not Acceptable", StringComparison.OrdinalIgnoreCase) ? "No Aceptable" : title;
            case 415:
                return string.Equals(title, "Unsupported Media Type", StringComparison.OrdinalIgnoreCase) ? "Tipo de Medio No Soportado" : title;
        }

        return title;
    }

    private static string TranslateDetail(string detail, int? status)
    {
        if (detail.StartsWith("The specified resource was not found.", StringComparison.OrdinalIgnoreCase))
        {
            return "El recurso especificado no fue encontrado.";
        }
        return detail;
    }

    public static void TranslateValidationErrors(IDictionary<string, string[]> errors)
    {
        foreach (var key in errors.Keys.ToList())
        {
            var messages = errors[key];
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = TranslateMessage(messages[i]);
            }
        }
    }

    public static string TranslateMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return message;

        // 1. "The {field} field is required." -> "El campo {field} es obligatorio."
        var requiredMatch = Regex.Match(message, @"^The (.+) field is required\.$", RegexOptions.IgnoreCase);
        if (requiredMatch.Success)
        {
            var fieldName = requiredMatch.Groups[1].Value;
            if (fieldName.Equals("resource", StringComparison.OrdinalIgnoreCase))
            {
                return "El cuerpo de la solicitud es obligatorio.";
            }
            return $"El campo {fieldName} es obligatorio.";
        }

        // 2. "The JSON value could not be converted to {type}. Path: {path} | LineNumber: {line} | BytePositionInLine: {pos}."
        var convertMatch = Regex.Match(message, @"^The JSON value could not be converted to ([^.]+)\. Path: ([^|]+) \| LineNumber: (\d+) \| BytePositionInLine: (\d+)\.$", RegexOptions.IgnoreCase);
        if (!convertMatch.Success)
        {
            convertMatch = Regex.Match(message, @"^The JSON value could not be converted to (.+)\. Path: ([^|]+)(.*)$", RegexOptions.IgnoreCase);
        }
        if (convertMatch.Success)
        {
            var typeName = convertMatch.Groups[1].Value;
            if (typeName.Contains('.'))
            {
                typeName = typeName.Substring(typeName.LastIndexOf('.') + 1);
            }
            var path = convertMatch.Groups[2].Value.Trim();
            var extra = convertMatch.Groups.Count > 3 ? convertMatch.Groups[3].Value : "";

            extra = extra.Replace("LineNumber", "Línea")
                         .Replace("BytePositionInLine", "Posición de byte")
                         .Replace("|", " | ");

            return $"El valor JSON no pudo ser convertido al tipo {typeName}. Ruta: {path}{extra}";
        }

        // 3. "The value '{value}' is not valid for {field}." -> "El valor '{value}' no es válido para {field}."
        var invalidMatch = Regex.Match(message, @"^The value '(.+)' is not valid for (.+)\.$", RegexOptions.IgnoreCase);
        if (invalidMatch.Success)
        {
            return $"El valor '{invalidMatch.Groups[1].Value}' no es válido para {invalidMatch.Groups[2].Value}.";
        }

        // 4. "The field {field} must be a number." -> "El campo {field} debe ser un número."
        var mustBeNumberMatch = Regex.Match(message, @"^The field (.+) must be a number\.$", RegexOptions.IgnoreCase);
        if (mustBeNumberMatch.Success)
        {
            return $"El campo {mustBeNumberMatch.Groups[1].Value} debe ser un número.";
        }

        return message;
    }
}
