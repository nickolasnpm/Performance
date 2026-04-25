using Performance.Application.DTOs.Addresses;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.Addresses
{
    public static class AddressMapper
    {
        public static AddressDTO ToDTO(Address address, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncryptId(address.Id),
            AddressLine = address.AddressLine,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country,
        };
    }
}