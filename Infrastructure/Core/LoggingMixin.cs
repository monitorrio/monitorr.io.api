using System;
using System.Diagnostics;
using NLog;

namespace Core
{
	public static class LoggingMixin
	{
		public static Logger Logger(this object loggingContext)
		{
			checkCanLogInContext(loggingContext);
			return GetLoggerForContext(loggingContext);
		}

		public static void LogDebug(this object loggingContext, Func<string> messageBuilder)
		{
			checkCanLogInContext(loggingContext);
			if (loggingContext.Logger().IsDebugEnabled)
				GetLoggerForContext(loggingContext).Debug(messageBuilder());
		}

		public static void LogDebug(this object loggingContext, string message, params object[] messageParameters)
		{
			checkCanLogInContext(loggingContext);
			GetLoggerForContext(loggingContext).Debug(message, messageParameters);
		}

        public static void LogDebug(this object loggingContext, string message, bool growl, params object[] messageParameters)
        {
            // if (growl) {
            //     GrowlHelper.SimpleGrowl(message);
            // }
            checkCanLogInContext(loggingContext);
            GetLoggerForContext(loggingContext).Debug(message, messageParameters);
        }

        public static void LogInfo(this object loggingContext, string message, params object[] messageParameters)
		{
			checkCanLogInContext(loggingContext);
			GetLoggerForContext(loggingContext).Debug(message, messageParameters);
		}

		public static void LogException(this object loggingContext, Exception ex)
		{
			LogError(loggingContext, ex.Message);
		}

		public static void LogError(this object loggingContext, string message, params object[] messageParameters)
		{
			checkCanLogInContext(loggingContext);
			GetLoggerForContext(loggingContext).Error(message, messageParameters);
		}

		static Logger GetLoggerForContext(object loggingContext) {
			return LogManager.GetLogger(loggingContext.GetType().FullName);
		}

		public static bool CheckLoggingContext
		{
			get { return checkCanLogInContext != noop; }
			set { checkCanLogInContext = value ? (Action<object>)checkStackframe : noop; }
		}

		static Action<object> checkCanLogInContext = noop;

		static void noop(object _) {}
		static void checkStackframe(object context)
		{
			var prevFrame = new StackTrace().GetFrame(2);
			if (!(prevFrame.GetMethod().DeclaringType == context.GetType()))
				throw new InvalidOperationException("Can only call Logger from owning class");
		}
	}
}