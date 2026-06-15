namespace AutoServiceApp.Models;

public class Part : BaseEntity
{
    public string Name { get; set; } = "";
    public string Article { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public void UpdateDetails(PartDetails details)
    {
        Name = details.Name;
        Article = details.Article;
        Price = details.Price;
        Stock = details.Stock;
    }

    public bool TryTakeFromStock(int quantity)
    {
        if (Stock < quantity)
            return false;

        Stock -= quantity;
        return true;
    }

    public override string ToString() => $"{Name} [{Article}], {Price:C}, stock {Stock}";
}
