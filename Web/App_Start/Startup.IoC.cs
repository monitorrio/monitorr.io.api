using System;
using System.Web.Mvc;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Practices.ServiceLocation;
using Owin;
using Web.Infrastructure.Extensions;
using Hangfire;

namespace Web
{
    public partial class Startup
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class DoNotAutoWireAttribute : Attribute { }
        void ConfigureIoC(IAppBuilder app)
        {
            var container = AutofacContainer.Instance.Container;
            app.UseAutofacMiddleware(container);
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            QuickAccessData.Initialize(DependencyResolver.Current);
            container.ActivateGlimpse();
            GlobalConfiguration.Configuration.UseAutofacActivator(container);
        }

        public class NullIdentity : System.Security.Principal.IIdentity
        {
            public string AuthenticationType => null;
            public bool IsAuthenticated => false;
            public string Name => null;
        }

    }
}