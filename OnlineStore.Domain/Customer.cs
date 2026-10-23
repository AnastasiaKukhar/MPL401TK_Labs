namespace OnlineStore.Domain;

public class Customer
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Name { get; }
    public required string Email { get; }

    public Customer(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я покупця не може бути порожнім.", nameof(name));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Некоректний email.", nameof(email));

        Name = name;
        Email = email;
    }
}