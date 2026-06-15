namespace AutoServiceApp.Models;

public class MechanicDetails
{
    public string Name { get; init; } = "";
    public string Specialization { get; init; } = "";
    public decimal HourRate { get; init; }

    public void ApplyTo(Mechanic mechanic)
    {
        mechanic.Name = Name;
        mechanic.Specialization = Specialization;
        mechanic.HourRate = HourRate;
    }
}
