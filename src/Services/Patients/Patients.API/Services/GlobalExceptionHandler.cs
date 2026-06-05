using BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Patients.Application.Exceptions;

namespace Patients.API.Services;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception while processing request.");

        var (statusCode, title, detail, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation failed",
                "One or more validation errors occurred.",
                validationException.Errors
                    .Select(error => (object)new { field = error.PropertyName, message = error.ErrorMessage })
                    .ToArray()),
            DuplicatePatientException duplicatePatientException => (
                StatusCodes.Status409Conflict,
                "Duplicate patient",
                duplicatePatientException.Message,
                Array.Empty<object>()),
            TherapeuticGoalNotFoundException therapeuticGoalNotFoundException => (
                StatusCodes.Status404NotFound,
                "Not found",
                therapeuticGoalNotFoundException.Message,
                Array.Empty<object>()),
            SessionNotFoundException sessionNotFoundException => (
                StatusCodes.Status404NotFound,
                "Not found",
                sessionNotFoundException.Message,
                Array.Empty<object>()),
            SessionSchedulingConflictException sessionSchedulingConflictException => (
                StatusCodes.Status409Conflict,
                "Schedule conflict",
                sessionSchedulingConflictException.Message,
                Array.Empty<object>()),
            ReportNotFoundException reportNotFoundException => (
                StatusCodes.Status404NotFound,
                "Not found",
                reportNotFoundException.Message,
                Array.Empty<object>()),
            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Not found",
                notFoundException.Message,
                Array.Empty<object>()),
            UnauthorizedAccessException unauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                unauthorizedAccessException.Message,
                Array.Empty<object>()),
            InvalidOperationException invalidOperationException => (
                StatusCodes.Status400BadRequest,
                "Bad request",
                invalidOperationException.Message,
                Array.Empty<object>()),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Server error",
                "An unexpected error occurred.",
                Array.Empty<object>())
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (errors.Length > 0)
        {
            problemDetails.Extensions["errors"] = errors;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
