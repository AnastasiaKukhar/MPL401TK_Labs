namespace OnlineStore.Domain;

public class CardPayment : IPaymentMethod //підключення інтерфейу
{
    private readonly string _cardNumber;

    public string Name => "Банківська картка";

    public CardPayment(string cardNumber)
    {
        var digits = new string((cardNumber ?? "").Where(char.IsDigit).ToArray());
        if (digits.Length != 16)
            throw new ArgumentException("Номер картки має містити 16 цифр.", nameof(cardNumber));

        _cardNumber = digits;
    }

    public string Pay(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сума оплати має бути більшою за 0.", nameof(amount));

        return $"Оплата карткою **** {_cardNumber[^4..]}: {amount:F2} грн";
    }
}