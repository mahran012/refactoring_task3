namespace AutoServiceApp.Models;

public static class PaymentMethod
{
    public const string Cash = "cash";
    public const string Card = "card";
    public const string Transfer = "transfer";

    public static readonly string[] All =
    {
        Cash,
        Card,
        Transfer
    };

    public static bool IsCard(string paymentMethod) => paymentMethod == Card;
}
