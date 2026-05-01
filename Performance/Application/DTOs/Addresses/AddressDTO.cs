namespace Performance.Application.DTOs.Addresses
{
    public record AddressDTO(
        string Id,
        string AddressLine,
        string City,
        string State,
        string PostalCode,
        string Country);
}