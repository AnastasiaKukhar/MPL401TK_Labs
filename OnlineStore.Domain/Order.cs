namespace OnlineStore.Domain;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; } = Guid.NewGuid();
    public Customer Customer { get; }                       // composition
    public OrderStatus Status { get; private set; } = OrderStatus.New;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Total);

    public Order(Customer customer)
    {
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
    }

    public void AddItem(Product product, int quantity)
    {
        EnsureModifiable();
        ArgumentNullException.ThrowIfNull(product);

        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing is null)
            _items.Add(new OrderItem(product, quantity));
        else
            existing.IncreaseQuantity(quantity);
    }

    public void RemoveItem(Product product)
    {
        EnsureModifiable();
        ArgumentNullException.ThrowIfNull(product);

        var item = _items.FirstOrDefault(i => i.Product.Id == product.Id)
                   ?? throw new InvalidOperationException("Такого товару немає в замовленні.");
        _items.Remove(item);
    }

    public string Pay(IPaymentMethod paymentMethod)
    {
        ArgumentNullException.ThrowIfNull(paymentMethod);

        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Замовлення вже оплачено.");
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Скасоване замовлення не можна оплатити.");
        if (_items.Count == 0)
            throw new InvalidOperationException("Не можна оплатити порожнє замовлення.");

        var receipt = paymentMethod.Pay(Total);   // polymorphic call
        Status = OrderStatus.Paid;
        return receipt;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Оплачене замовлення не можна скасувати.");

        Status = OrderStatus.Cancelled;
    }

    private void EnsureModifiable()
    {
        if (Status != OrderStatus.New)
            throw new InvalidOperationException($"Замовлення в статусі {Status} не можна змінювати.");
    }
}