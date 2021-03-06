using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Jobbr.Server.WebAPI.Logging;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    internal class TraceSourceExceptionLogger : IExceptionLogger
    {
        private readonly ILog logger;

        public TraceSourceExceptionLogger(ILog logger)
        {
            this.logger = logger;
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var logAsync = new Task(() => this.logger.FatalException("Unhandled Exception while processing request '{0}'", context.Exception, context.Request));

            logAsync.Start();

            return logAsync;
        }
    }
}