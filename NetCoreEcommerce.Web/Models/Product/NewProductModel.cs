using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreEcommerce.Web.Models.Product
{
    public class NewProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name of the Product")]
        [Display(Name = "Product name*")]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter url of the Product image")]
        [Display(Name = "Image url*")]
        public string ImageUrl { get; set; }

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }


        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [Required(ErrorMessage = "Please enter price of the Product")]
        [Range(0.1,double.MaxValue)]
        [Display(Name = "Price*")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Select if the Product is prefered or not")]
        [Display(Name = "Is prefered?*")]
        public bool? IsPreferedProduct { get; set; } = false;

        [Range(0, double.MaxValue)]
        [Required(ErrorMessage = "Please enter how many is left in stock")]
        [Display(Name = "In stock*")]
        public int? InStock { get; set; }

        [Required(ErrorMessage = "Please select category")]
        [Range(1,double.MaxValue)]
        public int? CategoryId { get; set; }
    }
}
