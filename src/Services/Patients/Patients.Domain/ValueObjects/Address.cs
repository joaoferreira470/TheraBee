namespace Patients.Domain.ValueObjects;

public record Address
{
    public string AddressLine { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    protected Address()
    {

    }

    private Address(string addressLine, string district, string location, string zipcode)
    {
        AddressLine = addressLine;
        District = district;
        Location = location;
        ZipCode = zipcode;
    }

    public static Address Of(string addressLine, string district, string location, string zipcode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        ArgumentException.ThrowIfNullOrWhiteSpace(district);
        ArgumentException.ThrowIfNullOrWhiteSpace(location);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipcode);

        return new Address(addressLine, district, location, zipcode);
    }
}
