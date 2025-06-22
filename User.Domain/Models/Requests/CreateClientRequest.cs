namespace User.Domain.Models.Requests
{
    public class CreateClientRequest
    {
        public string Name { get; set; } = default!;
        public int UserId { get; set; }
    }
} 