namespace Performance.Application.DTOs.Addresses
{
    public class AddressDTO
    {
        public required string Id { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
        public required string UserId { get; set; }
    }
}