namespace AutoServiceApp.Models;

public class Customer : BaseEntity
{
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public List<Car> Cars { get; set; } = new();
    public string LastPaymentMethod { get; set; } = PaymentMethod.Cash;

    public void UpdateContact(CustomerContactDetails details)
    {
        Name = details.Name;
        Phone = details.Phone;
        Email = details.Email;
        Address = details.Address;
    }

    public void AddCar(Car car)
    {
        if (Cars.All(x => x.Id != car.Id))
            Cars.Add(car);
    }

    public void RemoveCar(Car car)
    {
        Cars.RemoveAll(x => x.Id == car.Id);
    }

    public string? GetFirstCarOwnerPhone()
    {
        var firstCar = Cars.FirstOrDefault();
        return firstCar?.GetOwnerPhone();
    }

    public string GetContactDisplayText() => $"{Name} / {Phone}";

    public override string ToString() => string.IsNullOrWhiteSpace(Phone) ? Name : $"{Name} ({Phone})";
}
