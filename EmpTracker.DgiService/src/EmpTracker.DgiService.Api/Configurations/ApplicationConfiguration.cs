using Asp.Versioning;
using EmpTracker.DgiService.Application.Behaviors.Validators;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Core.Interfaces;
using EmpTracker.DgiService.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EmpTracker.DgiService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<CreateDesignationCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateDesignationCommandValidator>();
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
