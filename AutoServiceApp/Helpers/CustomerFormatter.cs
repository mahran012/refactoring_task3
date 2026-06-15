using AutoServiceApp.Models;

namespace AutoServiceApp.Helpers;

public class CustomerFormatter : IDisplayFormatter<Customer>
{
    public string Format(Customer customer) => $"{customer.Name} / {customer.Phone}";
}
