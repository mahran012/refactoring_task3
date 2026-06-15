using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class CustomerService
{
    public Customer AddCustomer(List<Customer> customers, CustomerContactDetails contactDetails)
    {
        var customer = new Customer();
        customer.UpdateContact(contactDetails);
        customers.Add(customer);
        return customer;
    }

    public void UpdateCustomer(IEnumerable<RepairOrder> orders, Customer customer, CustomerContactDetails contactDetails)
    {
        customer.UpdateContact(contactDetails);
        foreach (var order in orders.Where(x => x.CustomerId == customer.Id))
            order.Customer = customer;
    }

    public void DeleteCustomer(List<Customer> customers, List<Car> cars, List<RepairOrder> orders, Customer customer)
    {
        customers.Remove(customer);
        foreach (var car in cars.Where(x => x.CustomerId == customer.Id).ToList())
            cars.Remove(car);
        foreach (var order in orders.Where(x => x.CustomerId == customer.Id).ToList())
            orders.Remove(order);
    }
}
