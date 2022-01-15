using System.Collections.Generic;
using NetCoreEcommerce.Web.Models.Product;

namespace NetCoreEcommerce.Web.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<ProductListingModel> ProductsList { get; set; }
    }
}
