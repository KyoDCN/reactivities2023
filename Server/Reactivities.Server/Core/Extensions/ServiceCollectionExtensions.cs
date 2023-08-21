using Reactivities.Application;
using Reactivities.Persistence;

namespace Reactivities.Server.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>();

            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(typeof(ApplicationEntry).Assembly);
            });

            services.AddAutoMapper(typeof(ApplicationEntry).Assembly);


            services.AddCors(o =>
            {
                o.DefaultPolicyName = "default";
                o.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyOrigin();
                });
            });

            return services;
        }
    }
}
