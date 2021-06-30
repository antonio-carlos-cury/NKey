using BookStore.Domain.Interfaces;
using BookStore.Domain.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.UI.Mvc.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
            return services;
        }
    }
}
