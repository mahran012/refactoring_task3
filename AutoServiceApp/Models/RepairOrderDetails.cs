namespace AutoServiceApp.Models;

public class RepairOrderDetails
{
    public Customer? Customer { get; init; }
    public Car? Car { get; init; }
    public string Description { get; init; } = "";
    public Mechanic? Mechanic { get; init; }
    public string Status { get; init; } = OrderStatus.New;
    public decimal Cost { get; init; }
    public string PaymentMethod { get; init; } = AutoServiceApp.Models.PaymentMethod.Cash;
}
