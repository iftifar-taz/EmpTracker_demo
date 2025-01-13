using Asp.Versioning;
using EmpTracker.Identity.Application.Behaviors.Validators;
using EmpTracker.Identity.Application.Features.Sessions.Commands;
using EmpTracker.Identity.Core.Interfaces;
using EmpTracker.Identity.Infrastructure.Persistence;
using EmpTracker.Identity.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EmpTracker.Identity.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<CreateSessionCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateSessionCommandValidator>();
            services.AddFluentValidationAutoValidation();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            return services;
        }
    }
}
