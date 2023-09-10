using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreFront.DATA.EF.Models//.Metadata
{
    #region Category Metadata
    public class CategoryMetadata
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "*Category is required")]
        [StringLength(50, ErrorMessage = "*Must be 50 chracters or less")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(150, ErrorMessage = "*Must be 150 characters or less")]
        [Display(Name = "Description")]
        public string? CategoryDescription { get; set; }
    }
    #endregion

    #region Company Metadata
    public class CompanyMetadata
    {

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "*Company Name is required")]
        [StringLength(40, ErrorMessage = "*Must be 40 chracters or less")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = null!;

        [StringLength(60, ErrorMessage = "*Must be 60 chracters or less")]
        public string? State { get; set; }

        [StringLength(15, ErrorMessage = "*Must be 15 chracters or less")]
        public string? City { get; set; }

        [StringLength(60, ErrorMessage = "*Must be 60 chracters or less")]
        [Display(Name = "Street Address")]
        public string? Address { get; set; }

        [StringLength(24, ErrorMessage = "*Must be 24 chracters or less")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone #")]
        public string? Phone { get; set; }
    }
    #endregion

    #region Genre Metadata
    public class GenreMetadata
    {
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "Genre Type")]
        public string GenreName { get; set; } = null!;
    }
    #endregion

    #region Order Metadata
    public class OrderMetadata
    {
        public int OrderId { get; set; }

        //none
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Order Date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Ship To Customer is required")]
        [StringLength(100, ErrorMessage = "*Must be 100 characters or less")]
        [Display(Name = "Customer")]
        public string ShipToName { get; set; } = null!;

        [Required(ErrorMessage = "Ship To City is required")]
        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "City")]
        public string ShipCity { get; set; } = null!;

        [StringLength(2, ErrorMessage = "*Must be 2 characters or less")]
        [Display(Name = "State")]
        public string? ShipState { get; set; }

        [Required(ErrorMessage = "Ship To Zip is required")]
        [StringLength(5, ErrorMessage = "*Must be 5 characters or less")]
        [Display(Name = "Zip")]
        [DataType(DataType.PostalCode)]
        public string ShipZip { get; set; } = null!;
    }
    #endregion

    #region Product Metadata
    public class ProductMetadata
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "*Name is required")]
        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "Name")]
        public string ProductName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "*Must be 500 characters or less")]
        [Display(Name = "Description")]
        public string? ProductDescription { get; set; }

        [StringLength(75, ErrorMessage = "*Must be 75 characters or less")]
        [Display(Name = "Image")]
        public string? ProductImage { get; set; }

        //None
        public int CategoryId { get; set; }
        public int ProductStatusId { get; set; }
        public int CompanyId { get; set; }
        public int? SwordId { get; set; }
        public int? GenreId { get; set; }
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Range(0, (double)decimal.MaxValue)]
        public decimal ProductPrice { get; set; }
        [Display(Name = "Featured?")]
        public bool IsFeatured { get; set; }

    }
    #endregion

    #region Product Status Metadata
    public class ProductStatusMetadata
    {
        public int ProductStatusId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(20, ErrorMessage = "*Must be 20 characters or less")]
        [Display(Name = "Description")]
        public string StatusDescription { get; set; } = null!;
    }
    #endregion

    #region Sword Type Metadata
    public class SwordTypeMetadata
    {

        public int SwordId { get; set; }

        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "Sword Type")]
        public string? SwordType1 { get; set; }
    }
    #endregion

    #region User Detail Metadata
    public class UserDetailMetadata
    {
        public string UserId { get; set; } = null!;

        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "Street Address")]
        public string? Address { get; set; }

        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "City")]
        public string? City { get; set; }

        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "State")]
        public string? State { get; set; }

        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "Zip")]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [StringLength(128, ErrorMessage = "*Must be 128 characters or less")]
        [Display(Name = "Phone #")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
    }
    #endregion

}
