namespace AutoServiceApp.Models;

public class RepairReport
{
    public string Title { get; set; } = "";
    public DateTime From { get; set; } = DateTime.Today.AddMonths(-1);
    public DateTime To { get; set; } = DateTime.Today;
    public string Text { get; set; } = "";
    public List<RepairOrder> Orders { get; set; } = new();
    public decimal UrgentExtraRevenue { get; set; }
    public int ApprovedWarrantyCount { get; set; }
}
