using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GiftSuggestionService.Data;
using GiftSuggestionService.Services;
using GiftSuggestionService.Configurations;
using GiftSuggestionService.Filters;

namespace GiftSuggestionService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGiftSuggestionRepo, GiftSuggestionRepo>();
            services.AddSingleton<IAmazonProductRepo, AmazonProductRepo>();
            services.AddSingleton<KeyvaultAccessorService>();
            services.AddSingleton<GptManagementService>();
            services.AddSingleton<AmazonProductManagementService>();
            services.AddSingleton<RequestAccessorService>();

            services.Configure<Dbconfiguration>(Configuration.GetSection("GiftSuggestionsDatabase"));
            services.Configure<KeyvaultConfiguration>(Configuration.GetSection("Keyvault"));
            services.Configure<EnivronmentConfiguration>(Configuration.GetSection("Environment"));
            services.Configure<GptConfiguration>(Configuration.GetSection("GptCongiguration"));
            services.Configure<AmazonProductConfiguration>(Configuration.GetSection("AmazonProductConfiguration"));
            services.AddHttpContextAccessor();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GiftSuggestionService", Version = "v1" });
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ServiceExceptionFilter));
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GiftSuggestionService v1"));
            }

            //app.UseHttpsRedirection();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            //app.UseAuthorization();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app);
        }
    }
}
