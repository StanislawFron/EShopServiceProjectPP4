namespace User.Domain.Models.Entities
{
    public class Address
    {
        public int Id { get; set; } = default!;
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
