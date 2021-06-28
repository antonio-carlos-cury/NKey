using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookStore.Api.Configuration
{
    public static class LoggerConfiguration
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "e8c234c4373d4f75b86fa1c533d148ce";
                o.LogId = new Guid("573024b5-2c32-47d9-9027-4851d962709c");
            });
            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}
