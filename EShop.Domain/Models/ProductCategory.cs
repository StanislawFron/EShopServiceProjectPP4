﻿namespace EShopDomain.Models
{
    public class ProductCategory : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
