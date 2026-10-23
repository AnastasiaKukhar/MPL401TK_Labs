namespace OnlineStore.Domain;

public class BankTransferPayment : IPaymentMethod //підключення інтерфейсу IPaymentMethod для реалізації методу оплати банківським переказом
{
    private readonly string _iban;

    public string Name => "Банківський переказ";

    public BankTransferPayment(string iban)
    {
        if (string.IsNullOrWhiteSpace(iban) || iban.Length < 15)
            throw new ArgumentException("Некоректний IBAN.", nameof(iban));

        _iban = iban;
    }

    public string Pay(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сума оплати має бути більшою за 0.", nameof(amount));

        return $"Переказ на рахунок {_iban}: {amount:F2} грн";
    }
}