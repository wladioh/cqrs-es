using ApiEF.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EventStores.Files;
using EventFlow.Extensions;

namespace ApiEF
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddLogging(logging => logging
                .AddConsole()
                .SetMinimumLevel(LogLevel.Debug));

            services
                .AddEventFlow(o => o
                    .AddDefaults(typeof(Employee).Assembly)
                    .UseFilesEventStore(FilesEventStoreConfiguration.Create("./evt-store"))
                    .ConfigureJson(j => j
                        .AddSingleValueObjects())
                    .AddAspNetCore(c => c
                        .RunBootstrapperOnHostStartup()
                        .UseMvcJsonOptions()
                        .UseModelBinding()
                        .AddUserClaimsMetadata()
                        .UseLogging()
                        
                    ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<CommandPublishMiddleware>();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
