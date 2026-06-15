using AutoServiceApp.Models;

namespace AutoServiceApp.Helpers;

public class PartNameFormatter : IDisplayFormatter<Part>
{
    public string Format(Part part) => $"{part.Name} ({part.Article})";
}
