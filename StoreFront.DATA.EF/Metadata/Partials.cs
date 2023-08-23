using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreFront.DATA.EF.Models//.Metadata
{
    internal class Partials
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
        public partial class Product { }
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
}
