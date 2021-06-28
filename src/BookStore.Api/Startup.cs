using BookStore.Api.Configuration;
using BookStore.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace BookStore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentityConfig(Configuration);
            services.AddLoggingConfig();
            services.AddApiConfig();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerConfig();
            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {

            app.UseApiConfig(env);
            app.UseLoggingConfiguration();
            app.UseSwaggerConfig(provider);
        }
    }
}
