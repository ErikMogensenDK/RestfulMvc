using Students.Application;
using Students.Application.Database;
using Students.Api.Mapping;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services = ApplicationServiceCollectionExtender.AddApplication(services);
services = ApplicationServiceCollectionExtender.AddQueryDataAccess(services, config["Database:ConnectionString"]!);
services = ApplicationServiceCollectionExtender.AddWriteDataAccess(services, config["Database:ConnectionString"]!);

services.AddOpenApi();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Students API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
