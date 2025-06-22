namespace EShopDomain.Models
{
    public class BaseModel
    {
        public bool Deleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int UpdatedBy { get; set; }
    }
}
