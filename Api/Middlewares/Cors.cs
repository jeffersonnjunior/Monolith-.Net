namespace Api.Middlewares;

public static class Cors
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Policy", builder =>
            {
                builder.WithOrigins("https://localhost:5099", "https://localhost:7056")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors("Policy");
        return app;
    }
}