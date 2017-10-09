using System;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SharpAuth0;

namespace Web
{

    public sealed class AutofacContainer
    {
        private static readonly Lazy<AutofacContainer> Lazy =
            new Lazy<AutofacContainer>(() => new AutofacContainer());

        public static AutofacContainer Instance => Lazy.Value;
        public IContainer Container;

        private AutofacContainer()
        {
            var webAssembly = typeof(MvcApplication).Assembly;
            var b = new ContainerBuilder();
            b.RegisterAssemblyTypes(webAssembly).Where(t => t.GetCustomAttribute<Startup.DoNotAutoWireAttribute>() == null);
            b.RegisterAssemblyTypes(webAssembly).Where(t => t.GetCustomAttribute<Startup.DoNotAutoWireAttribute>() == null).AsImplementedInterfaces();
            b.RegisterControllers(webAssembly);
            b.RegisterApiControllers(webAssembly);
            b.Register(c => HttpContext.Current.GetOwinContext().Authentication);
            b.RegisterType<IdentityGateway>().As<IIdentityGateway>().InstancePerRequest();
            Container = b.Build();
        }
    }
}