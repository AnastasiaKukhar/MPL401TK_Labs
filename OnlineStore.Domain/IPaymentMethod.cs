namespace OnlineStore.Domain;

public interface IPaymentMethod //інтерфейс для методів оплати, набір методів, які потім треба реалізувати в клаі
{
    string Name { get; }

   
    string Pay(decimal amount);
}