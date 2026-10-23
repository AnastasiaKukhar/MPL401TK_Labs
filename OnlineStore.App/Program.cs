using OnlineStore.Domain;

Console.WriteLine("Modern Programming Course");
Console.WriteLine();
Console.WriteLine("Student: Кухар Анастасія Романівна ");
Console.WriteLine("Group: 401TK");
Console.WriteLine();
Console.WriteLine("Variant: 13(1)");
Console.WriteLine("Domain: Online Store");
Console.WriteLine();
Console.WriteLine(".NET: 10");
Console.WriteLine();

var customer = new Customer("Анастасія", "anastasia@example.com");
var laptop = new Product("Ноутбук", 25000m);
var mouse = new Product("Мишка", 500m);

var order = new Order(customer);
order.AddItem(laptop, 1);
order.AddItem(mouse, 2);
order.AddItem(mouse, 1);       
order.RemoveItem(mouse);
order.AddItem(mouse, 2);

Console.WriteLine($"Замовлення для {order.Customer.Name}:");
foreach (var item in order.Items)
    Console.WriteLine($" - {item.Product.Name} x{item.Quantity} = {item.Total:F2} грн");
Console.WriteLine($"Разом: {order.Total:F2} грн, статус: {order.Status}");
Console.WriteLine();


IPaymentMethod[] methods =
{
    new CardPayment("4111 1111 1111 1111"),
    new BankTransferPayment("UA213223130000026007233566001")
};

foreach (IPaymentMethod method in methods)
{
    var demoOrder = new Order(customer);
    demoOrder.AddItem(laptop, 1);
    Console.WriteLine($"[{method.Name}] {demoOrder.Pay(method)}");
}
Console.WriteLine();

order.Pay(methods[0]);
Console.WriteLine($"Статус після оплати: {order.Status}");

try { order.Pay(methods[1]); }
catch (InvalidOperationException ex) { Console.WriteLine($"Помилка: {ex.Message}"); }

var cancelled = new Order(customer);
cancelled.AddItem(mouse, 1);
cancelled.Cancel();
try { cancelled.Pay(methods[0]); }
catch (InvalidOperationException ex) { Console.WriteLine($"Помилка: {ex.Message}"); }

try { var bad = new Product("Брак", 0); }
catch (ArgumentException ex) { Console.WriteLine($"Помилка: {ex.Message}"); }

