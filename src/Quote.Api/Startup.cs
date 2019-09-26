using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using Dictum.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Dictum.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddCustomServices();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    AllowCachingResponses = false,
                    ResponseWriter = JsonHealthCheckWriter
                }
            );
        }

        private static async Task JsonHealthCheckWriter(HttpContext context, HealthReport report)
        {
            var status = new
            {
                status = report.Status.ToString(),
                errors = report.Entries.Select(e => new
                    {key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status)})
            };

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(JsonSerializer.Serialize(status));
        }
    }
}