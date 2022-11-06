using Core.SaveFiles.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class CoreServiceExtensions
    {
        public static IServiceCollection AddClasses(this IServiceCollection services)
        {
            bool isDemoMode = Boolean.Parse(Environment.GetEnvironmentVariable("DEMO") ?? "false");

            if (isDemoMode)
            {
                services.AddSingleton<ISaveFileManagerService, DemoSaveFileManagerService>();
            }
            else
            {
                services.AddSingleton<ISaveFileManagerService, LocalSaveFileManagerService>();
            }

            return services;
        }
    }
}
