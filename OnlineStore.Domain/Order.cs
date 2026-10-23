using OnlineStore.Domain.Enums;
namespace OnlineStore.Domain;

public class Order
{
    private readonly List<OrderItem> _items = new(); //encapsulation

    public Guid Id { get; } = Guid.NewGuid();
    public Customer Customer { get; }                       // composition
    public OrderStatus Status { get; private set; } = OrderStatus.Created;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Total);
    public Address DeliveryAddress { get; set; };

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

    public string Pay(IPaymentMethod paymentMethod) // polymorphism
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
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException($"Замовлення в статусі {Status} не можна змінювати.");
    }

    public string GetStatusDescription() => Status switch
    {
        OrderStatus.Created => "Створено",
        OrderStatus.Paid => "Оплачено",
        OrderStatus.Cancelled => "Скасовано",
        OrderStatus.Shipped => "Відправлено",
        OrderStatus.Delivered => "Доставлено",
        _ => "Невідомий статус"
    };

    public string GetOrderCategory() => Total switch
    {
       <500 => "Малий",
        < 5000 => "Середній",
        >= 5000 => "Великий",
        _ => "Невідомий"
    };

    public string GetDeliveryMethod() => (Status, Total) switch
    {
        
        (OrderStatus.Paid, >= 2000) => "Безкоштовна доставка",
        _ => "Невідомий метод доставки"
    };
}