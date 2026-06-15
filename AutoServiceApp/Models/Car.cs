namespace AutoServiceApp.Models;

public class Car : BaseEntity
{
    public string CustomerId { get; set; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public Customer? Owner { get; set; }
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    public string Vin { get; set; } = "";
    public string LicensePlate { get; set; } = "";
    public int Mileage { get; set; }

    public void AssignOwner(Customer? owner)
    {
        CustomerId = owner?.Id ?? "";
        Owner = owner;
    }

    public void UpdateVehicle(VehicleDetails details)
    {
        Make = details.Make;
        Model = details.Model;
        Year = details.Year;
        Vin = details.Vin;
        Mileage = details.Mileage;
        LicensePlate = details.LicensePlate;
    }

    public override string ToString() => $"{Make} {Model}, {LicensePlate}, {Year}, {Mileage} km";
}
