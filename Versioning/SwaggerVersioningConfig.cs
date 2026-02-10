using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NET_API_with_Redis.Versioning
{
    public class SwaggerVersioningConfig : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerVersioningConfig(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new()
                {
                    Title = $"NET API with Redis {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
                //options.DocInclusionPredicate((docName, apiDesc) => true);

            }
         
        }
    }
}
