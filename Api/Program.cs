using Api.Filters;
using Api.Middlewares;
using Application.DependencyInjection;
using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomCors();
builder.Services.AddNotificationActionFilter();
builder.Services.DependencyInjectionApplication(configuration);
builder.Services.DependencyInjectionInfrastructure(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();