using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace NET_API_with_Redis.Versioning
{
    public static class ApiVersioningConfig
    {
        public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Default API version (v1)
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Assume default version when not specified
                options.AssumeDefaultVersionWhenUnspecified = true;

                // Report supported API versions in response headers
                options.ReportApiVersions = true;

                // Versioning methods
                options.ApiVersionReader = ApiVersionReader.Combine(
                    // URL segment versioning: /api/v1/products
                    new UrlSegmentApiVersionReader(),

                    // Query string versioning: ?api-version=1.0
                    new QueryStringApiVersionReader("api-version"),

                    // Header versioning: X-Version: 1.0
                    new HeaderApiVersionReader("X-Version")
                );
            });

            // Add versioned API explorer (for Swagger grouping)
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // v1, v2, etc.
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
