namespace OnlineStore.Domain;
public record Address(string Street, string City, string Building, string PostalCode)
{
    public string? PhoneNumber { get; set; }
    public override string ToString()
    {
        return $"{Street}, {City}, {Building}, {PostalCode}";
    }
}