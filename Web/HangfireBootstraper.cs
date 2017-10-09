using System.Configuration;
using System.Web.Hosting;
using Hangfire;
using Hangfire.Mongo;

namespace Web
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object _lockObject = new object();
        private bool _started;

        private BackgroundJobServer _backgroundJobServer;

        private HangfireBootstrapper()
        {
        }

        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;

                HostingEnvironment.RegisterObject(this);

                var connectionString = ConfigurationManager.AppSettings["hangfire:ConnectionString"];
                var databaseName = ConfigurationManager.AppSettings["hangfire:DatabaseName"];

                GlobalConfiguration.Configuration.UseMongoStorage(connectionString, databaseName);

                GlobalConfiguration.Configuration
                    .UseMongoStorage(connectionString, databaseName);
                var container = AutofacContainer.Instance.Container;
                GlobalConfiguration.Configuration.UseAutofacActivator(container);

                _backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}