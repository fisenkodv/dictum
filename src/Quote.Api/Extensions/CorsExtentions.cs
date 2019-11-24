using Microsoft.Extensions.DependencyInjection;

namespace Dictum.Api.Extensions
{
    public static class CorsExtensions
    {
        public static readonly string DictumCorsPolicyName = "_myAllowSpecificOrigins";

        public static IServiceCollection AddDictumCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DictumCorsPolicyName,
                    builder =>
                    {
                        builder
                            .WithOrigins("https://*.fisenko.page", "https://*.fisenko.io")
                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });
            return services;
        }
    }
}