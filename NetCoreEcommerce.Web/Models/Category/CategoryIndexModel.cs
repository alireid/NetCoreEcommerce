using System.Collections.Generic;

namespace NetCoreEcommerce.Web.Models.Category
{
    public class CategoryIndexModel
    {
        public IEnumerable<CategoryListingModel> CategoryList { get; set; }
    }
}
