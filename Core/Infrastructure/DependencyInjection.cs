using Core.Business.Services.Base;
using Core.Context;
using Core.Infrastructure.Middleware;
using Core.Infrastructure.Pipelines;
using Core.Infrastructure.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTesteElawDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<TesteElawDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    b => {
                        b.MigrationsAssembly(typeof(TesteElawDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }),
                ServiceLifetime.Scoped);

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            foreach (var implementationType in typeof(TesteElawDbContext)
            .Assembly
            .ExportedTypes
            .Where(t => t.IsClass && !t.IsAbstract))
            {
                foreach (var serviceType in implementationType
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
                {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));
                }
            }

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));

            return services;
        }

        public static void AddErrorMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
