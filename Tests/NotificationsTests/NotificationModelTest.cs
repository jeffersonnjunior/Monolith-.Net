using System.Collections.Generic;
using System.Linq;
using Infrastructure.Notifications;
using Xunit;

namespace Tests.NotificationsTests;

public class NotificationContextTest
{
    [Fact]
    public void AddNotification_ShouldAddNotification()
    {
        // Arrange
        var context = new NotificationContext();
        var message = "Test notification";

        // Act
        context.AddNotification(message);

        // Assert
        Assert.Single(context.GetNotifications());
        Assert.Equal(message, context.GetNotifications().First().Message);
    }

    [Fact]
    public void GetNotifications_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var context = new NotificationContext();
        var message = "Test notification";
        context.AddNotification(message);

        // Act
        var notifications = context.GetNotifications();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<NotificationModel>>(notifications);
        Assert.Single(notifications);
    }

    [Fact]
    public void HasNotifications_ShouldReturnTrue_WhenNotificationsExist()
    {
        // Arrange
        var context = new NotificationContext();
        var message = "Test notification";
        context.AddNotification(message);

        // Act
        var hasNotifications = context.HasNotifications();

        // Assert
        Assert.True(hasNotifications);
    }

    [Fact]
    public void HasNotifications_ShouldReturnFalse_WhenNoNotificationsExist()
    {
        // Arrange
        var context = new NotificationContext();

        // Act
        var hasNotifications = context.HasNotifications();

        // Assert
        Assert.False(hasNotifications);
    }
}