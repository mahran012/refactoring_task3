using AutoServiceApp.Helpers;
using AutoServiceApp.Models;
using AutoServiceApp.Storage;

namespace AutoServiceApp.Services;

public class AutoServiceManager
{
    private List<Customer> _customers = new();
    private List<Car> _cars = new();
    private List<RepairOrder> _orders = new();
    private List<Part> _parts = new();
    private List<Mechanic> _mechanics = new();
    private List<string> _notifications = new();

    public IReadOnlyList<Customer> Customers => _customers;
    public IReadOnlyList<Car> Cars => _cars;
    public IReadOnlyList<RepairOrder> Orders => _orders;
    public IReadOnlyList<Part> Parts => _parts;
    public IReadOnlyList<Mechanic> Mechanics => _mechanics;
    public IReadOnlyList<string> Notifications => _notifications;


    public JsonFileStore<Customer> CustomerStore { get; set; } = new();
    public JsonFileStore<Car> CarStore { get; set; } = new();
    public JsonFileStore<RepairOrder> OrderStore { get; set; } = new();
    public JsonFileStore<Part> PartStore { get; set; } = new();
    public JsonFileStore<Mechanic> MechanicStore { get; set; } = new();
    public SmsNotifier SmsNotifier { get; set; } = new();
    public EmailSender EmailSender { get; set; } = new();
    public ReportService ReportService { get; set; } = new();
    public OrderStatusHelper StatusHelper { get; set; } = new();
    public OrderCostCalculator OrderCostCalculator { get; set; } = new();
    public OrderDetailsBuilder OrderDetailsBuilder { get; set; } = new();

    public void Load()
    {
        _customers = CustomerStore.Load("customers.json");
        _cars = CarStore.Load("cars.json");
        _orders = OrderStore.Load("orders.json");
        _parts = PartStore.Load("parts.json");
        _mechanics = MechanicStore.Load("mechanics.json");
        RelinkEverything();
        if (_customers.Count == 0 && _cars.Count == 0 && _mechanics.Count == 0)
            Seed();
    }

    public void SaveAll()
    {
        CustomerStore.Save("customers.json", _customers);
        CarStore.Save("cars.json", _cars);
        OrderStore.Save("orders.json", _orders);
        PartStore.Save("parts.json", _parts);
        MechanicStore.Save("mechanics.json", _mechanics);
    }

    private void PersistDomainChange(bool refreshRelationships = false)
    {
        if (refreshRelationships)
            RelinkEverything();

        SaveAll();
    }

    public void RelinkEverything()
    {
        foreach (var c in _customers)
            c.Cars = _cars.Where(x => x.CustomerId == c.Id).ToList();

        foreach (var car in _cars)
            car.Owner = _customers.FirstOrDefault(x => x.Id == car.CustomerId);

        foreach (var order in _orders)
        {
            order.Customer = _customers.FirstOrDefault(x => x.Id == order.CustomerId);
            order.Car = _cars.FirstOrDefault(x => x.Id == order.CarId);
            order.AssignedMechanic = _mechanics.FirstOrDefault(x => x.Id == order.AssignedMechanicId);
        }

        foreach (var m in _mechanics)
            m.AssignedOrderIds = _orders.Where(x => x.AssignedMechanicId == m.Id).Select(x => x.Id).ToList();
    }

    public Customer AddCustomer(string name, string phone, string email, string address)
    {
        return AddCustomer(new CustomerContactDetails
        {
            Name = name,
            Phone = phone,
            Email = email,
            Address = address
        });
    }

    public Customer AddCustomer(CustomerContactDetails contactDetails)
    {
        var customer = new Customer();
        customer.UpdateContact(contactDetails);
        _customers.Add(customer);
        PersistDomainChange();
        return customer;
    }

    public void UpdateCustomer(Customer customer, string name, string phone, string email, string address)
    {
        UpdateCustomer(customer, new CustomerContactDetails
        {
            Name = name,
            Phone = phone,
            Email = email,
            Address = address
        });
    }

    public void UpdateCustomer(Customer customer, CustomerContactDetails contactDetails)
    {
        customer.UpdateContact(contactDetails);
        foreach (var order in _orders.Where(x => x.CustomerId == customer.Id))
            order.Customer = customer;
        PersistDomainChange();
    }

    public void DeleteCustomer(Customer customer)
    {
        _customers.Remove(customer);
        foreach (var car in _cars.Where(x => x.CustomerId == customer.Id).ToList())
            _cars.Remove(car);
        foreach (var order in _orders.Where(x => x.CustomerId == customer.Id).ToList())
            _orders.Remove(order);
        PersistDomainChange();
    }

    public Car AddCar(Customer? owner, string make, string model, int year, string vin, int mileage, string licensePlate)
    {
        return AddCar(owner, new VehicleDetails
        {
            Make = make,
            Model = model,
            Year = year,
            Vin = vin,
            Mileage = mileage,
            LicensePlate = licensePlate
        });
    }

    public Car AddCar(Customer? owner, VehicleDetails vehicleDetails)
    {
        var car = new Car();
        car.AssignOwner(owner);
        car.UpdateVehicle(vehicleDetails);
        _cars.Add(car);
        owner?.AddCar(car);
        PersistDomainChange();
        return car;
    }

    public void UpdateCar(Car car, Customer? owner, string make, string model, int year, string vin, int mileage, string licensePlate)
    {
        UpdateCar(car, owner, new VehicleDetails
        {
            Make = make,
            Model = model,
            Year = year,
            Vin = vin,
            Mileage = mileage,
            LicensePlate = licensePlate
        });
    }

    public void UpdateCar(Car car, Customer? owner, VehicleDetails vehicleDetails)
    {
        car.AssignOwner(owner);
        car.UpdateVehicle(vehicleDetails);
        PersistDomainChange(refreshRelationships: true);
    }

    public void DeleteCar(Car car)
    {
        _cars.Remove(car);
        foreach (var c in _customers)
            c.RemoveCar(car);
        foreach (var order in _orders.Where(x => x.CarId == car.Id).ToList())
            _orders.Remove(order);
        PersistDomainChange();
    }

    public Mechanic AddMechanic(string name, string specialization, decimal hourRate)
    {
        return AddMechanic(new MechanicDetails
        {
            Name = name,
            Specialization = specialization,
            HourRate = hourRate
        });
    }

    public Mechanic AddMechanic(MechanicDetails details)
    {
        var mechanic = new Mechanic();
        mechanic.UpdateProfile(details);
        _mechanics.Add(mechanic);
        PersistDomainChange();
        return mechanic;
    }

    public void UpdateMechanic(Mechanic mechanic, string name, string specialization, decimal hourRate)
    {
        UpdateMechanic(mechanic, new MechanicDetails
        {
            Name = name,
            Specialization = specialization,
            HourRate = hourRate
        });
    }

    public void UpdateMechanic(Mechanic mechanic, MechanicDetails details)
    {
        mechanic.UpdateProfile(details);
        PersistDomainChange();
    }

    public void DeleteMechanic(Mechanic m)
    {
        _mechanics.Remove(m);
        foreach (var order in _orders.Where(o => o.AssignedMechanicId == m.Id))
        {
            order.AssignMechanic(null);
        }
        PersistDomainChange();
    }

    public Part AddPart(string name, string article, decimal price, int stock)
    {
        return AddPart(new PartDetails
        {
            Name = name,
            Article = article,
            Price = price,
            Stock = stock
        });
    }

    public Part AddPart(PartDetails details)
    {
        var part = new Part();
        part.UpdateDetails(details);
        _parts.Add(part);
        PersistDomainChange();
        return part;
    }

    public void UpdatePart(Part part, string name, string article, decimal price, int stock)
    {
        UpdatePart(part, new PartDetails
        {
            Name = name,
            Article = article,
            Price = price,
            Stock = stock
        });
    }

    public void UpdatePart(Part part, PartDetails details)
    {
        part.UpdateDetails(details);
        PersistDomainChange();
    }

    public void DeletePart(Part p)
    {
        _parts.Remove(p);
        PersistDomainChange();
    }

    public RepairOrder CreateOrder(Customer? customer, Car? car, string description, Mechanic? mechanic, string status, string paymentMethod)
    {
        return CreateOrder(new RepairOrderDetails
        {
            Customer = customer,
            Car = car,
            Description = description,
            Mechanic = mechanic,
            Status = status,
            PaymentMethod = paymentMethod
        });
    }

    public RepairOrder CreateOrder(RepairOrderDetails details)
    {
        var order = RepairOrder.Create(details, "RO-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        _orders.Add(order);
        details.Mechanic?.AssignOrder(order.Id);
        PersistDomainChange();
        return order;
    }

    public void UpdateOrder(RepairOrder order, Customer? customer, Car? car, string description, Mechanic? mechanic, string status, decimal cost, string paymentMethod)
    {
        UpdateOrder(order, new RepairOrderDetails
        {
            Customer = customer,
            Car = car,
            Description = description,
            Mechanic = mechanic,
            Status = status,
            Cost = cost,
            PaymentMethod = paymentMethod
        });
    }

    public void UpdateOrder(RepairOrder order, RepairOrderDetails details)
    {
        order.ApplyDetails(details);
        if (order.Status != details.Status)
            ChangeOrderStatus(order, details.Status, NotificationType.Both);
        PersistDomainChange(refreshRelationships: true);
    }

    public void DeleteOrder(RepairOrder order)
    {
        _orders.Remove(order);
        foreach (var mechanic in _mechanics)
            mechanic.UnassignOrder(order.Id);
        PersistDomainChange();
    }

    public void ChangeOrderStatus(RepairOrder order, string newStatus, string notificationType)
    {
        StatusHelper.MarkStatus(order, newStatus);
        if (OrderStatus.IsReady(newStatus))
            order.Cost = CalculateOrderCost(order, true, order.PaymentMethod);
        order.AssignedMechanic?.AssignOrder(order.Id);
        NotifyAboutStatus(order, notificationType);
        PersistDomainChange();
    }

    public void AddWorkToOrder(RepairOrder order, string name, double hours, decimal cost)
    {
        AddWorkToOrder(order, new RepairWorkDetails
        {
            Name = name,
            Hours = hours,
            Cost = cost
        });
    }

    public void AddWorkToOrder(RepairOrder order, RepairWorkDetails details)
    {
        order.AddWork(details.ToRepairWork());
        order.Cost = CalculateOrderCost(order, false, order.PaymentMethod);
        PersistDomainChange();
    }

    public bool UsePartForOrder(RepairOrder order, Part part, int qty)
    {
        if (!part.TryTakeFromStock(qty))
            return false;

        order.AddPartUsage(part, qty, OrderCostCalculator.ImmediatePartUsageMarkup);
        PersistDomainChange();
        return true;
    }

    public decimal CalculateOrderCost(RepairOrder order, bool final, string paymentMethod)
    {
        return OrderCostCalculator.Calculate(order, _parts, final, paymentMethod);
    }

    public string BuildOrderDetails(RepairOrder order)
    {
        return OrderDetailsBuilder.Build(order);
    }

    public Customer? GetOwnerForCar(Car car)
    {
        return car.Owner ?? FindCustomer(car.CustomerId);
    }

    public Customer? GetCustomerForOrder(RepairOrder order)
    {
        return order.Customer ?? FindCustomer(order.CustomerId);
    }

    public Car? GetCarForOrder(RepairOrder order)
    {
        return order.Car ?? FindCar(order.CarId);
    }

    public Mechanic? GetMechanicForOrder(RepairOrder order)
    {
        return order.AssignedMechanic ?? FindMechanic(order.AssignedMechanicId);
    }

    public Customer? FindCustomer(string customerId)
    {
        return _customers.FirstOrDefault(x => x.Id == customerId);
    }

    public Car? FindCar(string carId)
    {
        return _cars.FirstOrDefault(x => x.Id == carId);
    }

    public Mechanic? FindMechanic(string mechanicId)
    {
        return _mechanics.FirstOrDefault(x => x.Id == mechanicId);
    }

    public string BuildReports(DateTime from, DateTime to)
    {
        return ReportService.BuildRevenueReport(_orders, from, to) + "\n"
            + ReportService.BuildPopularWorks(_orders) + "\n\n"
            + ReportService.BuildMechanicsLoad(_mechanics, _orders) + "\n"
            + ReportService.BuildPartsStock(_parts);
    }

    public List<RepairOrder> GetOrdersForMechanic(Mechanic m)
    {
        var result = new List<RepairOrder>();
        foreach (var id in m.AssignedOrderIds)
        {
            var o = _orders.FirstOrDefault(x => x.Id == id);
            if (o != null)
                result.Add(o);
        }
        return result;
    }

    public void NotifyAboutStatus(RepairOrder order, string type)
    {
        var phone = order.Customer?.Phone ?? "";
        var email = order.Customer?.Email ?? "";
        var text = $"Order {order.OrderNumber}: new status {order.Status}";
        if (NotificationType.IsSms(type))
            SmsNotifier.SendSms(phone, text);
        else if (NotificationType.IsEmail(type))
            EmailSender.Send(email, "Order status", text);
        else
        {
            SmsNotifier.SendSms(phone, text);
            EmailSender.Send(email, "Order status", text);
        }
        _notifications.Add($"{DateTime.Now:g}: {type} {text}");
    }

    private void Seed()
    {
        var c1 = AddCustomer("John Parker", "+1 555 100-20-30", "john@example.com", "12 Market Street");
        var c2 = AddCustomer("Anna Stone", "+1 555 555-44-33", "anna@example.com", "45 Lake Avenue");
        var car1 = AddCar(c1, "Toyota", "Camry", 2018, "JTNB11HK303000001", 87000, "ABC123");
        AddCar(c2, "Kia", "Rio", 2021, "Z94CB41ABMR000002", 43000, "MOR777");
        var m1 = AddMechanic("Sam Miller", "engine", 1200);
        AddMechanic("Owen Lane", "electrical", 1500);
        AddPart("Oil filter", "OF-100", 650, 12);
        AddPart("Brake pads", "BR-500", 3200, 5);
        var order = CreateOrder(c1, car1, "Knock on startup, diagnostics required", m1, OrderStatus.Diagnostics, PaymentMethod.Card);
        AddWorkToOrder(order, "Computer diagnostics", 1.5, 2500);
        PersistDomainChange();
    }
}
