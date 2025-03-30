namespace Patients.Domain.ValueObjects;

public record Address
{
    public string AddressLine { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
}
