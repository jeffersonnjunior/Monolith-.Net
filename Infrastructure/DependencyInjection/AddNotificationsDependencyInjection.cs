using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Notifications;

namespace Infrastructure.DependencyInjection;

public static class AddNotificationsDependencyInjection
{
    public static void NotificationsDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<NotificationContext>();
    }
}