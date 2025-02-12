namespace Infrastructure.Notifications;

public class NotificationModel
{
    public string Message { get; }

    public NotificationModel(string message)
    {
        Message = message;
    }
}