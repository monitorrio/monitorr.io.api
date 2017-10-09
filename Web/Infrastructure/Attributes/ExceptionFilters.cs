using System;
using System.Net;
using Core;
using Glimpse.Core.Tab.Assist;

namespace Web.Controllers
{
	using System.Net.Http;
	using System.Web.Http.Filters;
	public class AppExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public HttpStatusCode StatusCode { get; set; }
		public Type ExceptionType { get; set; }

		public override void OnException(HttpActionExecutedContext context) {
			var ex = context.Exception;
			this.Logger().Error("Error on route {0}".FormatWith(context.Request.RequestUri), ex);
			if (ExceptionHelpers.ShouldRespondTo(ex, ExceptionType) && context.Response == null) {
				context.Response = context.Request.CreateErrorResponse(StatusCode, ex);
				Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
			}
			base.OnException(context);
		}

		public AppExceptionFilterAttribute() {
			StatusCode = HttpStatusCode.InternalServerError;
		}
	}
}

namespace Web.Controllers
{
	using System.Web.Mvc;
	public class AppMvcExceptionFilterAttribute : FilterAttribute, IExceptionFilter
	{
		public HttpStatusCode? StatusCode { get; set; }
		public Type ExceptionType { get; set; }

		public void OnException(ExceptionContext filterContext) {
			var ex = filterContext.Exception;
			this.Logger().Error("Error on route {0}".FormatWith(filterContext.HttpContext.Request.Url), ex);
			if (ExceptionHelpers.ShouldRespondTo(ex, ExceptionType) && !filterContext.ExceptionHandled) {
				Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
				filterContext.HttpContext.Response.StatusCode = StatusCode.HasValue
					? (int)StatusCode.Value
					: (int)HttpStatusCode.InternalServerError;

				filterContext.HttpContext.Response.Write(contentFor(ex));
				filterContext.ExceptionHandled = true;
			}
		}
		string contentFor(Exception ex) {
			return "<html><body><pre>Error: {0}</pre></body></html>".FormatWith(ex.Message);
		}
	}

	static class ExceptionHelpers
	{
		public static bool ShouldRespondTo(Exception ex, Type exceptionType = null) {
			return (ex != null) && (exceptionType == null || exceptionType.IsInstanceOfType(ex));
		}
	}
}