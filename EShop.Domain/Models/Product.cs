namespace EShopDomain.Models
{
    public class Product : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

       

        public decimal Price { get; set; }

        

       

        public ProductCategory Category { get; set; } = default!;
    }
}
