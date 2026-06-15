namespace AutoServiceApp.Models;

public class VehicleDetails
{
    public string Make { get; init; } = "";
    public string Model { get; init; } = "";
    public int Year { get; init; }
    public string Vin { get; init; } = "";
    public int Mileage { get; init; }
    public string LicensePlate { get; init; } = "";

    public void ApplyTo(Car car)
    {
        car.Make = Make;
        car.Model = Model;
        car.Year = Year;
        car.Vin = Vin;
        car.Mileage = Mileage;
        car.LicensePlate = LicensePlate;
    }
}
