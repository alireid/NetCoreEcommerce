using System.Collections.Generic;
using NetCoreEcommerce.Data.Models;

namespace NetCoreEcommerce.Data
{
    public interface IProduct
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetPreferred(int count);
        IEnumerable<Product> GetProductsByCategoryId(int categoryId);
        IEnumerable<Product> GetFilteredProducts(int id, string searchQuery);
        IEnumerable<Product> GetFilteredProducts(string searchQuery);
        Product GetById(int id);
        void NewProduct(Product product);
        void EditProduct(Product product);
        void DeleteProduct(int id);
    }
}