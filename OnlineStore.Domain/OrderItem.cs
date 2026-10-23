namespace OnlineStore.Domain;

public class OrderItem
{
    public Product Product { get; }
    public int Quantity { get; private set; }
    public decimal Total => Product.Price * Quantity;

    public OrderItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        ValidateQuantity(quantity);
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        ValidateQuantity(amount);
        Quantity += amount;
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Кількість має бути більшою за 0.", nameof(quantity));
    }
}