namespace Performance.Application.DTOs.Addresses
{
    public record AddAddressRequestDTO(
        string AddressLine,
        string City,
        string State,
        string PostalCode,
        string Country
    );
}