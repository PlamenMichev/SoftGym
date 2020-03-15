using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SoftGym.Web.Areas.Identity.IdentityHostingStartup))]

namespace SoftGym.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}