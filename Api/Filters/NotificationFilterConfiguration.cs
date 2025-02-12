using Application.Filters;

namespace Api.Filters;

public static class NotificationFilterConfiguration
{
    public static IServiceCollection AddNotificationActionFilter(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<NotificationActionFilter>();
        });

        return services;
    }
}
