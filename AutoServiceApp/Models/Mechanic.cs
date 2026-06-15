namespace AutoServiceApp.Models;

public class Mechanic : BaseEntity
{
    public string Name { get; set; } = "";
    public string Specialization { get; set; } = "";
    public decimal HourRate { get; set; }
    public List<string> AssignedOrderIds { get; set; } = new();

    public void UpdateProfile(MechanicDetails details)
    {
        Name = details.Name;
        Specialization = details.Specialization;
        HourRate = details.HourRate;
    }

    public void AssignOrder(string orderId)
    {
        if (!AssignedOrderIds.Contains(orderId))
            AssignedOrderIds.Add(orderId);
    }

    public void UnassignOrder(string orderId)
    {
        AssignedOrderIds.Remove(orderId);
    }

    public string GetProfileDisplayText() => $"{Name} - {Specialization}";

    public override string ToString() => $"{Name} - {Specialization}, {HourRate:C}/h";
}
