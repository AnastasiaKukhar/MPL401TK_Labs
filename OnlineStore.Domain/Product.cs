namespace OnlineStore.Domain;

public class Product
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public decimal Price { get; private set; }

    public Product(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва товару не може бути порожньою.", nameof(name));

        Name = name;
        ChangePrice(price);
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Ціна товару має бути більшою за 0.", nameof(newPrice));

        Price = newPrice;
    }

    public override string ToString() => $"{Name} ({Price:F2} грн)";
}