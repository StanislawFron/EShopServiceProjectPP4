namespace User.Domain.Models.Entities
{
    public class Client
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string NIP { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public int AddressId { get; set; }
        public Address Address { get; set; } = default!;
        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}