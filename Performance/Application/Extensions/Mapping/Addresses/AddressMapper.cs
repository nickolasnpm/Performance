using Performance.Application.DTOs.Addresses;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.Addresses
{
    public static class AddressMapper
    {
        public static AddressDTO ToDTO(this Address address) => new()
        {
            Id = address.Id,
            AddressLine = address.AddressLine,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country,
            UserId = address.UserId
        };
    }
}