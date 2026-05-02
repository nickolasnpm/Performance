using Performance.Application.DTOs.Addresses;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class AddressMapper
    {
        extension(Address address)
        {
            public AddressDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(address.Id),
                AddressLine: address.AddressLine,
                City: address.City,
                State: address.State,
                PostalCode: address.PostalCode,
                Country: address.Country);
        }
    }
}