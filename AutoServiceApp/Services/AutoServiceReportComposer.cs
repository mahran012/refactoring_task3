using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class AutoServiceReportComposer
{
    public string Build(
        ReportService reportService,
        List<RepairOrder> orders,
        List<Mechanic> mechanics,
        List<Part> parts,
        DateTime from,
        DateTime to)
    {
        return reportService.BuildRevenueReport(orders, from, to) + "\n"
            + reportService.BuildPopularWorks(orders) + "\n\n"
            + reportService.BuildMechanicsLoad(mechanics, orders) + "\n"
            + reportService.BuildPartsStock(parts);
    }
}
