namespace AutoServiceApp.Models;

public class RepairWorkDetails
{
    public string Name { get; init; } = "";
    public double Hours { get; init; }
    public decimal Cost { get; init; }

    public RepairWork ToRepairWork()
    {
        return new RepairWork
        {
            Name = Name,
            Hours = Hours,
            Cost = Cost
        };
    }
}
