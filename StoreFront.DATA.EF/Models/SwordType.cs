using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class SwordType
    {
        public SwordType()
        {
            Products = new HashSet<Product>();
        }

        public int SwordId { get; set; }
        public string? SwordType1 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
