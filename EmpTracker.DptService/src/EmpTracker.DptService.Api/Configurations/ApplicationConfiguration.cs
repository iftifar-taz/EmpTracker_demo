using Asp.Versioning;
using EmpTracker.DptService.Application.Behaviors.Validators;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Core.Interfaces;
using EmpTracker.DptService.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EmpTracker.DptService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<CreateDepartmentCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateDepartmentCommandValidator>();
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
