using Api.Filters;
using Api.Middlewares;
using Api.Versioning;
using Application.DependencyInjection;
using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomCors();
builder.Services.AddNotificationActionFilter();
builder.Services.DependencyInjectionApplication(configuration);
builder.Services.DependencyInjectionInfrastructure(configuration);
builder.Services.AddApiVersioningConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseCustomCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();