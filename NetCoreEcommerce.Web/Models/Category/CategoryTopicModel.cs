using System.Collections.Generic;
using NetCoreEcommerce.Web.Models.Product;

namespace NetCoreEcommerce.Web.Models.Category
{
    public class CategoryTopicModel
    {
        public CategoryListingModel Category { get; set; }
        public IEnumerable<ProductListingModel> Products { get; set; }
        public string SearchQuery { get; set; }
        public IEnumerable<NetCoreEcommerce.Data.Models.Category> CategoryList { get; set; }
        
    }
}
