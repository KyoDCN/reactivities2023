﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Reactivities.Application;
using Reactivities.Application.Interfaces;
using Reactivities.Infrastructure.Photos;
using Reactivities.Infrastructure.Security;
using Reactivities.Persistence;

namespace Reactivities.Server.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>();

            services.AddCors(o =>
            {
                o.DefaultPolicyName = "default";
                o.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyOrigin();
                });
            });

            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(typeof(ApplicationEntry).Assembly);
            });

            services.AddAutoMapper(typeof(ApplicationEntry).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(ApplicationEntry).Assembly);
            services.AddHttpContextAccessor();

            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            
            services.Configure<CloudinarySettings>(x =>
            {
                x.CloudName = config["Cloudinary:CloudName"];
                x.API.Key = config["Cloudinary:Key"];
                x.API.Secret = config["Cloudinary:Secret"];
            });

            return services;
        }
    }
}
