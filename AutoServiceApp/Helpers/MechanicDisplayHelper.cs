using AutoServiceApp.Models;

namespace AutoServiceApp.Helpers;

public class MechanicDisplayHelper : IDisplayFormatter<Mechanic>
{
    public string Format(Mechanic mechanic) => mechanic.Name + " - " + mechanic.Specialization;
}
