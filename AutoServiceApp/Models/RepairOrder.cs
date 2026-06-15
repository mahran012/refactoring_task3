namespace AutoServiceApp.Models;

public class RepairOrder : BaseEntity
{
    public string OrderNumber { get; set; } = "";
    public string CustomerId { get; set; } = "";
    public string CarId { get; set; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public Customer? Customer { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Car? Car { get; set; }
    public string ProblemDescription { get; set; } = "";
    public string Status { get; set; } = OrderStatus.New;
    public string AssignedMechanicId { get; set; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public Mechanic? AssignedMechanic { get; set; }
    public DateTime AcceptedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public decimal Cost { get; set; }
    public string PaymentMethod { get; set; } = AutoServiceApp.Models.PaymentMethod.Cash;
    public List<RepairWork> Works { get; set; } = new();
    public List<string> UsedPartIds { get; set; } = new();
    public List<string> StatusHistory { get; set; } = new();

    public static RepairOrder Create(RepairOrderDetails details, string orderNumber)
    {
        var order = new RepairOrder
        {
            OrderNumber = orderNumber,
            Status = details.Status,
            Cost = details.Cost
        };
        order.ApplyDetails(details);
        order.RecordHistory($"order created with status {details.Status}");
        return order;
    }

    public void ApplyDetails(RepairOrderDetails details)
    {
        AssignCustomerAndCar(details.Customer, details.Car);
        AssignMechanic(details.Mechanic);
        UpdateDescription(details.Description);
        ChangePaymentMethod(details.PaymentMethod);
        SetCost(details.Cost);
    }

    public void AssignCustomerAndCar(Customer? customer, Car? car)
    {
        CustomerId = customer?.Id ?? "";
        CarId = car?.Id ?? "";
        Customer = customer;
        Car = car;
    }

    public void AssignMechanic(Mechanic? mechanic)
    {
        AssignedMechanicId = mechanic?.Id ?? "";
        AssignedMechanic = mechanic;
    }

    public void UpdateDescription(string description)
    {
        ProblemDescription = description;
    }

    public void ChangePaymentMethod(string paymentMethod)
    {
        PaymentMethod = paymentMethod;
    }

    public void SetCost(decimal cost)
    {
        Cost = cost;
    }

    public void AddWork(RepairWork work)
    {
        Works.Add(work);
    }

    public void AddPartUsage(Part part, int quantity, decimal markup)
    {
        for (var i = 0; i < quantity; i++)
            UsedPartIds.Add(part.Id);

        Cost += part.Price * quantity * markup;
        RecordHistory($"part used {part.Name} x{quantity}");
    }

    public void RecordHistory(string message)
    {
        StatusHistory.Add($"{DateTime.Now:g}: {message}");
    }

    public override string ToString()
    {
        var client = Customer?.Name ?? CustomerId;
        var car = Car == null ? CarId : $"{Car.Make} {Car.Model}";
        return $"{OrderNumber}: {client}, {car}, {Status}, {Cost:C}";
    }
}

public class UrgentRepairOrder : RepairOrder
{
    public bool NeedTaxi { get; set; }
    public decimal UrgentFee { get; set; } = 500;
}

public class WarrantyRepairOrder : RepairOrder
{
    public string WarrantyNumber { get; set; } = "";
    public bool ApprovedByDealer { get; set; }
}
