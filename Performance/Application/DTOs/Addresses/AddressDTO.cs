using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.Addresses
{
    public class AddressDTO
    {
        public long Id { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
        public required long UserId { get; set; }


        // only for mapping from Address entity to AddressDTO
        [SetsRequiredMembers]
        public AddressDTO(Address address)
        {
            Id = address.Id;
            AddressLine = address.AddressLine;
            City = address.City;
            State = address.State;
            PostalCode = address.PostalCode;
            Country = address.Country;
            UserId = address.UserId;
        }
    }
}