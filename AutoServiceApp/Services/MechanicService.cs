using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class MechanicService
{
    public Mechanic AddMechanic(List<Mechanic> mechanics, MechanicDetails details)
    {
        var mechanic = new Mechanic();
        mechanic.UpdateProfile(details);
        mechanics.Add(mechanic);
        return mechanic;
    }

    public void UpdateMechanic(Mechanic mechanic, MechanicDetails details)
    {
        mechanic.UpdateProfile(details);
    }

    public void DeleteMechanic(List<Mechanic> mechanics, IEnumerable<RepairOrder> orders, Mechanic mechanic)
    {
        mechanics.Remove(mechanic);
        foreach (var order in orders.Where(o => o.AssignedMechanicId == mechanic.Id))
            order.AssignMechanic(null);
    }

    public List<RepairOrder> GetOrdersForMechanic(IEnumerable<RepairOrder> orders, Mechanic mechanic)
    {
        var result = new List<RepairOrder>();
        foreach (var id in mechanic.AssignedOrderIds)
        {
            var order = orders.FirstOrDefault(x => x.Id == id);
            if (order != null)
                result.Add(order);
        }
        return result;
    }
}
