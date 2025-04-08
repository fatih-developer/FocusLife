using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FocusLifePlus.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling {RequestType}", typeof(TRequest).Name);

            var requestName = typeof(TRequest).Name;
            var uniqueId = Guid.NewGuid().ToString();
            var requestNameWithGuid = $"{requestName} [{uniqueId}]";

            _logger.LogInformation(
                "Begin Request {RequestNameWithGuid}",
                requestNameWithGuid);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var response = await next();

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                _logger.LogInformation(
                    "End Request {RequestNameWithGuid} ({ElapsedMilliseconds}ms)",
                    requestNameWithGuid,
                    elapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                _logger.LogError(
                    ex,
                    "Request {RequestNameWithGuid} failed ({ElapsedMilliseconds}ms)",
                    requestNameWithGuid,
                    elapsedMilliseconds);

                throw;
            }
        }
    }
} 