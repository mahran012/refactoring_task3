using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class InventoryService
{
    public Part AddPart(List<Part> parts, PartDetails details)
    {
        var part = new Part();
        part.UpdateDetails(details);
        parts.Add(part);
        return part;
    }

    public void UpdatePart(Part part, PartDetails details)
    {
        part.UpdateDetails(details);
    }

    public void DeletePart(List<Part> parts, Part part)
    {
        parts.Remove(part);
    }

    public bool UsePartForOrder(RepairOrder order, Part part, int quantity)
    {
        if (!part.TryTakeFromStock(quantity))
            return false;

        order.AddPartUsage(part, quantity, OrderCostCalculator.ImmediatePartUsageMarkup);
        return true;
    }
}
