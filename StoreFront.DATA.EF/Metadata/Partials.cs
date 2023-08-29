using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreFront.DATA.EF.Models//.Metadata
{
   
        #region Category Metadata
        [ModelMetadataType(typeof(CategoryMetadata))]
        public partial class Category { }
        #endregion

        #region Company Metadata
        [ModelMetadataType(typeof(CompanyMetadata))]
        public partial class Company { }
        #endregion

        #region Genre Metadata
        [ModelMetadataType(typeof(GenreMetadata))]
        public partial class Genre { }
        #endregion

        #region Order Metadata
        [ModelMetadataType(typeof(OrderMetadata))]
        public partial class Order { }
        #endregion

        #region Product Metadata
        [ModelMetadataType(typeof(ProductMetadata))]
        public partial class Product 
        {
        [NotMapped]
        public IFormFile? Image { get; set; }
        }
        #endregion

        #region Product Status Metadata
        [ModelMetadataType(typeof(ProductStatusMetadata))]
        public partial class ProductStatus { }
        #endregion

        #region Sword Type Metadata
        [ModelMetadataType(typeof(SwordTypeMetadata))]
        public partial class SwordType { }
        #endregion

        #region User Detail Metadata
        [ModelMetadataType(typeof(UserDetailMetadata))]
        public partial class UserDetail { }
        #endregion
    
}
