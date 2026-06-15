namespace AutoServiceApp.Models;

public static class OrderStatus
{
    public const string New = "New";
    public const string Diagnostics = "Diagnostics";
    public const string InProgress = "In Progress";
    public const string WaitingForParts = "Waiting for Parts";
    public const string Ready = "Ready";
    public const string Released = "Released";

    public static readonly string[] All =
    {
        New,
        Diagnostics,
        InProgress,
        WaitingForParts,
        Ready,
        Released
    };

    public static bool IsReady(string status) => status == Ready;
}
