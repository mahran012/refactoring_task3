using AutoServiceApp.Helpers;
using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class RepairOrderWorkflowService
{
    public RepairOrder CreateOrder(List<RepairOrder> orders, RepairOrderDetails details)
    {
        var order = RepairOrder.Create(details, "RO-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        orders.Add(order);
        details.Mechanic?.AssignOrder(order.Id);
        return order;
    }

    public bool ApplyOrderDetails(RepairOrder order, RepairOrderDetails details)
    {
        order.ApplyDetails(details);
        return order.Status != details.Status;
    }

    public void DeleteOrder(List<RepairOrder> orders, IEnumerable<Mechanic> mechanics, RepairOrder order)
    {
        orders.Remove(order);
        foreach (var mechanic in mechanics)
            mechanic.UnassignOrder(order.Id);
    }

    public void ChangeStatus(RepairOrder order, string newStatus, string paymentMethod, IEnumerable<Part> parts, OrderStatusHelper statusHelper, OrderCostCalculator costCalculator)
    {
        statusHelper.MarkStatus(order, newStatus);
        if (OrderStatus.IsReady(newStatus))
            order.Cost = costCalculator.Calculate(order, parts, true, paymentMethod);
        order.AssignedMechanic?.AssignOrder(order.Id);
    }

    public void AddWork(RepairOrder order, RepairWorkDetails details, IEnumerable<Part> parts, OrderCostCalculator costCalculator)
    {
        order.AddWork(details.ToRepairWork());
        order.Cost = costCalculator.Calculate(order, parts, false, order.PaymentMethod);
    }
}
