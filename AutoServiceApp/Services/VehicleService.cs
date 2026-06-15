using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class VehicleService
{
    public Car AddCar(List<Car> cars, Customer? owner, VehicleDetails vehicleDetails)
    {
        var car = new Car();
        car.AssignOwner(owner);
        car.UpdateVehicle(vehicleDetails);
        cars.Add(car);
        owner?.AddCar(car);
        return car;
    }

    public void UpdateCar(Car car, Customer? owner, VehicleDetails vehicleDetails)
    {
        car.AssignOwner(owner);
        car.UpdateVehicle(vehicleDetails);
    }

    public void DeleteCar(List<Car> cars, IEnumerable<Customer> customers, List<RepairOrder> orders, Car car)
    {
        cars.Remove(car);
        foreach (var customer in customers)
            customer.RemoveCar(car);
        foreach (var order in orders.Where(x => x.CarId == car.Id).ToList())
            orders.Remove(order);
    }
}
