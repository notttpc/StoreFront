using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public int CategoryId { get; set; }
        public int ProductStatusId { get; set; }
        public int CompanyId { get; set; }
        public int? SwordId { get; set; }
        public int? GenreId { get; set; }
        public decimal ProductPrice { get; set; }
        public bool IsFeatured { get; set; }

        
        public virtual Category? Category { get; set; } 
        public virtual Company? Company { get; set; } 
        public virtual Genre? Genre { get; set; }
        public virtual ProductStatus? ProductStatus { get; set; }
        public virtual SwordType? Sword { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
