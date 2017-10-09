using System.Web.Mvc;
namespace Web.Infrastructure.Extensions
{
    public static class QuickAccessData
    {
        static IDependencyResolver _scope;
	    public static void Initialize(IDependencyResolver appScope) {
		    _scope = appScope;
	    }
    }
}