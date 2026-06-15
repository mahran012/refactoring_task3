namespace AutoServiceApp.Models;

public class PartDetails
{
    public string Name { get; init; } = "";
    public string Article { get; init; } = "";
    public decimal Price { get; init; }
    public int Stock { get; init; }

    public void ApplyTo(Part part)
    {
        part.UpdateDetails(this);
    }
}
