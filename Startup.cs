using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace temp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseFileServer(); // index.html
            app.UseMvc();
        }
    }
}
