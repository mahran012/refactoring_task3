namespace AutoServiceApp.Models;

public static class NotificationType
{
    public const string Sms = "sms";
    public const string Email = "email";
    public const string Both = "both";

    public static bool IsSms(string type) => type == Sms;

    public static bool IsEmail(string type) => type == Email;
}
