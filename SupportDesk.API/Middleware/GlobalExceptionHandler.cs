using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SupportDesk.Application.Exceptions;
using SupportDesk.Core.Exceptions;

namespace SupportDesk.API.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly IProblemDetailsService _problemDetailsService;
	private readonly ILogger<GlobalExceptionHandler> _logger;

	public GlobalExceptionHandler(
		IProblemDetailsService problemDetailsService,
		ILogger<GlobalExceptionHandler> logger)
	{
		_problemDetailsService = problemDetailsService;
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var (status, title) = exception switch
		{
			ValidationException => (StatusCodes.Status400BadRequest, "One or more validation errors occurred"),
			NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
			BusinessRuleException => (StatusCodes.Status409Conflict, "Business rule violation"),
			_ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
		};

		if (status == StatusCodes.Status500InternalServerError)
			_logger.LogError(exception, "Unhandled exception processing {Path}", httpContext.Request.Path);

		httpContext.Response.StatusCode = status;

		var problemDetails = new ProblemDetails
		{
			Status = status,
			Title = title,
			Detail = status == StatusCodes.Status500InternalServerError ? null : exception.Message
		};

		if (exception is ValidationException validationException)
		{
			problemDetails.Extensions["errors"] = validationException.Errors
				.GroupBy(e => e.PropertyName)
				.ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
		}

		return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
		{
			HttpContext = httpContext,
			Exception = exception,
			ProblemDetails = problemDetails
		});
	}
}
