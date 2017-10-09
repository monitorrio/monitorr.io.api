using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using EntityFramework.Filters;
using NLog;

namespace Web
{
	public static class DbInterceptionConfig
	{
		public static void Bootstrap() {
			DbInterception.Add(new FilterInterceptor());
			DbInterception.Add(new LoggingEFInterceptor());
		}

		public class LoggingEFInterceptor : DatabaseLogFormatter, IDbInterceptor
		{
			public LoggingEFInterceptor() : base(logger.Debug) { }
			public LoggingEFInterceptor(DbContext context) : base(context, logger.Debug) { }

			static readonly Logger logger = LogManager.GetLogger(typeof(LoggingEFInterceptor).FullName	);
		}
	}
}