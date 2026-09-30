namespace OnlineStore.Domain;

public interface IPaymentMethod
{
    string Name { get; }

   
    string Pay(decimal amount);
}