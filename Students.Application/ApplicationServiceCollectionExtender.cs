using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Students.Application.Database;
using Students.Application.Services;
using Students.DataAccess.exe;
using Students.DataAccess.providers;
using Students.DataAccess.Repositories;
using Students.Domain.Repositories;
using Students.Query.Queries;
using Students.Query.Queries.exe;

namespace Students.Application;

public static class ApplicationServiceCollectionExtender
{
    public static IServiceCollection AddApplication(IServiceCollection services)
    {
        services.AddSingleton<IStudentRepository, StudentRepository>();
        services.AddSingleton<IStudentCommandService, StudentCommandService>();
		services.AddSingleton<IStudentQuery, StudentQuery>();
		services.AddSingleton<IStudentQueryService, StudentQueryService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    } 

    public static IServiceCollection AddQueryDataAccess(IServiceCollection services, string connectionString)
    {
        services.AddSingleton<Query.Queries.providers.IDbConnectionFactory>(_ => 
            new Query.Queries.providers.SqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
		services.AddSingleton<Query.Queries.providers.IDbConnectionFactory>(_ => new Query.Queries.providers.SqlConnectionFactory(connectionString));

		services.AddSingleton<IQueryExecutor, QueryExecutor>();
        return services;
    }

    public static IServiceCollection AddWriteDataAccess(IServiceCollection services, string connectionString)
    {
        services.AddSingleton<DataAccess.providers.IDbConnectionFactory>(_ => 
            new DataAccess.providers.SqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
		services.AddSingleton<DataAccess.providers.IDbConnectionFactory>(_ => new DataAccess.providers.SqlConnectionFactory(connectionString));
		services.AddSingleton<ICommandExecutor, CommandExecutor>();
        return services;
    }
}
