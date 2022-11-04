using Core.SaveFiles.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class CoreServiceExtensions
    {
        public static IServiceCollection AddClasses(this IServiceCollection services)
        {
            services.AddSingleton<LocalSaveFileManagerService>();

            return services;
        }
    }
}
