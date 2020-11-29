using AutoMapper;
using Infrastructure.API;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mappings
{
    public static class DataMappings
    {
        public static void AddDataMappings(this IServiceCollection services)
        {
            // add automapper
            services.AddAutoMapper(new[] {
                typeof(DataMappingProfile),
                typeof(APIModelsMappingProfile)
            });
        }
    }
}
