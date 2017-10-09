using Hangfire;
using Owin;
using Web.Infrastructure;

namespace Web
{
	public partial class Startup
	{
	    public void ConfigureHangfire(IAppBuilder app)
	    {
            HangfireBootstrapper.Instance.Start();
            app.UseHangfireDashboard("/jobs", new DashboardOptions());
        }

    }
}