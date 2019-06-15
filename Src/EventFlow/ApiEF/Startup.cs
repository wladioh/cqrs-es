using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiEF.Controllers;
using DomainEF.Model.EmployeeModel;
using EventFlow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EventStores.Files;
using EventFlow.Extensions;
using EventFlow.Logs;
using EventFlow.Queries;
using EventFlow.ReadStores;
using EventFlow.ReadStores.InMemory;
using EventFlow.ReadStores.InMemory.Queries;
using Microsoft.EntityFrameworkCore;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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
                    .AddDefaults(typeof(EmployeeId).Assembly)
                    .UseFilesEventStore(FilesEventStoreConfiguration.Create("./evt-store"))
                    //.UseInMemoryReadStoreFor<EmployeeReadModel>()
                    .UseEntityFrameworkReadModel()
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

    public static class asdasd
    {
        public static IEventFlowOptions UseEntityFrameworkReadModel(
            this IEventFlowOptions eventFlowOptions)
        {
            return eventFlowOptions
                .RegisterServices(f =>
                {
                    f.Register<IEmployeeRepository, Repository>(Lifetime.Singleton);
                    f.Register<IReadModelStore<EmployeeReadModel>>(r => r.Resolver.Resolve<IEmployeeRepository>());
                })
                .UseReadStoreFor<IEmployeeRepository, EmployeeReadModel>();
        }
    }
    public class Repository : InMemoryReadStore<EmployeeReadModel>, IEmployeeRepository
    {
        public async Task<EmployeeReadModel> GetById(Guid id, CancellationToken ct = default)
        {
            return (await GetAsync(EmployeeId.With(id).Value, ct)).ReadModel;
        }

        public Task<IReadOnlyCollection<EmployeeReadModel>> GetAll(CancellationToken ct = default)
        {
            return this.FindAsync(model => true, ct);
        }

        public Repository(ILog log) : base(log)
        {
        }
    }
}
