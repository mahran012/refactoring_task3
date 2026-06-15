using System.Text;
using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class OrderDetailsBuilder
{
    public string Build(RepairOrder order)
    {
        var details = new StringBuilder();
        details.AppendLine(order.ToString());
        details.AppendLine(order.ProblemDescription);
        AppendWorks(details, order);
        AppendHistory(details, order);
        AppendOwnerPhone(details, order);

        return details.ToString();
    }

    private static void AppendWorks(StringBuilder details, RepairOrder order)
    {
        details.AppendLine("Works:");
        foreach (var work in order.Works)
            details.AppendLine(" - " + work);
    }

    private static void AppendHistory(StringBuilder details, RepairOrder order)
    {
        details.AppendLine("History:");
        foreach (var historyItem in order.StatusHistory)
            details.AppendLine(" - " + historyItem);
    }

    private static void AppendOwnerPhone(StringBuilder details, RepairOrder order)
    {
        if (order.Customer?.Cars.Count > 0)
            details.AppendLine("First car owner phone: " + order.Customer.Cars[0].Owner?.Phone);
    }
}
