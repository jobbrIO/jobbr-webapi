using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Jobbr.Server.WebAPI.App_Packages.LibLog._3._1;

namespace Jobbr.Server.WebAPI
{
    public class TraceSourceExceptionLogger : IExceptionLogger
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