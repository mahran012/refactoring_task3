using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class OrderStatusNotificationService
{
    private const string StatusNotificationTitle = "Order status";

    public void Notify(
        RepairOrder order,
        string notificationType,
        SmsNotifier smsNotifier,
        EmailSender emailSender,
        ICollection<string> notificationHistory)
    {
        var phone = order.Customer?.Phone ?? string.Empty;
        var email = order.Customer?.Email ?? string.Empty;
        var message = $"Order {order.OrderNumber}: new status {order.Status}";

        if (NotificationType.IsSms(notificationType))
            smsNotifier.SendSms(phone, message);
        else if (NotificationType.IsEmail(notificationType))
            emailSender.Send(email, StatusNotificationTitle, message);
        else
        {
            smsNotifier.SendSms(phone, message);
            emailSender.Send(email, StatusNotificationTitle, message);
        }

        notificationHistory.Add($"{DateTime.Now:g}: {notificationType} {message}");
    }
}
