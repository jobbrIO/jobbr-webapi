using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    internal class TraceSourceExceptionLogger : IExceptionLogger
    {
        private readonly ILogger<TraceSourceExceptionLogger> _logger;

        public TraceSourceExceptionLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TraceSourceExceptionLogger>();
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var logAsync = new Task(() => _logger.LogCritical("Unhandled Exception while processing request '{0}'", context.Exception, context.Request));

            logAsync.Start();

            return logAsync;
        }
    }
}