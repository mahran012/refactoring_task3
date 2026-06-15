namespace AutoServiceApp.Models;

public class CustomerContactDetails
{
    public string Name { get; init; } = "";
    public string Phone { get; init; } = "";
    public string Email { get; init; } = "";
    public string Address { get; init; } = "";

    public void ApplyTo(Customer customer)
    {
        customer.Name = Name;
        customer.Phone = Phone;
        customer.Email = Email;
        customer.Address = Address;
    }
}
