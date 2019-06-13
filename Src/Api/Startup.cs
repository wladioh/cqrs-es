using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Controllers;
using CQRSlite.Caching;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Messages;
using CQRSlite.Queries;
using CQRSlite.Routing;
using Domain.Base;
using Domain.Employee;
using Domain.Employee.Commands;
using Domain.Employee.Events;
using Domain.Location;
using Domain.Location.Commands;
using Domain.Location.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Infra;
using Mapster;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using ISession = CQRSlite.Domain.ISession;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSingleton<IMapper, MapperService>();

            services.Scan(scan => scan
                .FromAssemblyOf<EmployeeRepository>()
                .AddClasses(classes => classes.AssignableTo(typeof(IBaseRepository<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddSingleton(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
            services.AddSingleton<IQueryProcessor>(y => y.GetService<Router>());
            services.AddSingleton<IEventStore, Infra.EventStore>();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));

            services.AddScoped<ISession, Session>();
            services.Scan(scan => scan
                .FromAssemblies(typeof(EmployeeEventHandler).GetTypeInfo().Assembly)
                .AddClasses(classes => classes.Where(x =>
                {
                    var allInterfaces = x.GetInterfaces();
                    return
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryHandler<,>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableQueryHandler<,>));
                }))
                .AsSelf()
                .WithTransientLifetime()
            );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddSingleton<IConnectionMultiplexer>(s => ConnectionMultiplexer.Connect("localhost:9503"));
            var serviceProvider = services.BuildServiceProvider();
            var registrar = new RouteRegistrar(new Provider(serviceProvider));
            registrar.RegisterInAssemblyOf(typeof(EmployeeEventHandler));
            TypeAdapterConfig.GlobalSettings.Scan(GetType().Assembly);
            return serviceProvider;
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    //This makes scoped services work inside router.
    public class Provider : IServiceProvider
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public Provider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
        }

        public object GetService(Type serviceType)
        {
            return _contextAccessor?.HttpContext?.RequestServices.GetService(serviceType) ??
                   _serviceProvider.GetService(serviceType);
        }
    }
    public class MapperService : IMapper
    {
        public Task<TD> Map<TD>(object message)
        {
            return Task.FromResult(message.Adapt<TD>());
        }
    }

    public class CommandMapperRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CreateEmployeeRequest, CreateEmployeeCommand>()
                .ConstructUsing(src => new CreateEmployeeCommand(Guid.NewGuid(),
                    src.EmployeeId, src.FirstName, src.LastName, src.DateOfBirth, src.JobTitle));

            config.ForType<CreateLocationRequest, CreateLocationCommand>()
                .ConstructUsing(src => new CreateLocationCommand(Guid.NewGuid(), src.LocationId, src.StreetAddress, src.City,
                    src.State, src.PostalCode));

            config.ForType<LocationCreatedEvent, LocationRM>()
                .Map(it => it.AggregateId, it => it.Id)
                .Map(it => it.Id, it => it.LocationId);

            config.ForType<EmployeeCreatedEvent, EmployeeRM>()
                .Map(it => it.AggregateId, it => it.Id)
                .Map(it => it.Id, it => it.EmployeeId);
        }
    }
}
