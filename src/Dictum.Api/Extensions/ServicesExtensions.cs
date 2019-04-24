using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Services;
using Dictum.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dictum.Api.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IDictumRepository, DictumRepository>();

            services.AddTransient<DictumService>();
        }
    }
}