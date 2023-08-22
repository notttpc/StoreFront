using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Company
    {
        public Company()
        {
            Products = new HashSet<Product>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
