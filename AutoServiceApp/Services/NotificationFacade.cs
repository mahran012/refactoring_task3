using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class NotificationFacade
{
    public SmsNotifier Sms { get; set; }
    public EmailSender Email { get; set; }

    public NotificationFacade(SmsNotifier sms, EmailSender email)
    {
        Sms = sms;
        Email = email;
    }

    public void Notify(string type, string phone, string email, string title, string message)
    {
        if (NotificationType.IsSms(type))
            Sms.SendSms(phone, message);
        else if (NotificationType.IsEmail(type))
            Email.Send(email, title, message);
        else
        {
            Sms.SendSms(phone, message);
            Email.Send(email, title, message);
        }
    }
}
