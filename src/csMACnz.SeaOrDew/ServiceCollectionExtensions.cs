using Microsoft.Extensions.DependencyInjection;

namespace csMACnz.SeaOrDew
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSeaOrDewHandlers(this IServiceCollection services)
        {
            services.AddSingleton<QueryHandler>();
            services.AddSingleton<CommandHandler>();
        }
    }
}