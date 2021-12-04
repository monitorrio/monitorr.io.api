using System;
using System.Collections.Generic;
using Core;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Optimization;
using Web.Controllers;
using System.Web.Routing;
using Antlr.Runtime;
using Core.Configuration;
using Core.Domain;
using Web.Infrastructure.Migrations.Seeders;
using DataTables.AspNet.WebApi2;
using Web.Infrastructure.Extensions;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            this.LogDebug("Application Initializing");
            //GrowlHelper.SimpleGrowl("Application Initializing");
            AppPaths.SetCurrent(new AppPaths.PathsInstance(
                new DirectoryInfo(Server.MapPath("~")),
                new DirectoryInfo(Path.Combine(Path.GetTempPath(), "Web"))
            ));
            AppDeployment.Instance.Modules().ForEach(i => this.LogInfo("Integrating with {0} at {1}", i.Name, i.Uri));


            var options = new Options()
                .EnableRequestAdditionalParameters()
                .EnableResponseAdditionalParameters();

            var binder = new ModelBinder { ParseAdditionalParameters = Parser };

            GlobalConfiguration.Configuration.RegisterDataTables(options);
           

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configuration.Filters.Add(new System.Web.Http.AuthorizeAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new AppExceptionFilterAttribute());
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Initialize();
            Seeder.Initialize();

        }

        private IDictionary<string, object> Parser(HttpActionContext arg1,
            System.Web.Http.ModelBinding.ModelBindingContext modelBindingContext)
        {
            var model = new DataTableAdditionalParametersModel();

            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = modelBindingContext.ValueProvider.GetValue(property.Name.ToLowerInvariant());
                if (value != null)
                {
                    if (property.PropertyType.Name == typeof(Severity).Name)
                    {
                        //var inst = Activator.CreateInstance(property.PropertyType, value.AttemptedValue);
                        property.SetValue(model, value.AttemptedValue);
                    }
                    else
                    {
                        property.SetValue(model, value.AttemptedValue);
                    }
                }
            }
            return model.ToDictionary();
            return null;
           
        }

        protected void Application_End(object sender, EventArgs e)
        {
            HangfireBootstrapper.Instance.Stop();
        }

    }

    public class DataTableAdditionalParametersModel
    {
        public string LogId { get; set; }
        public Severity Severity { get; set; }
    }
}
