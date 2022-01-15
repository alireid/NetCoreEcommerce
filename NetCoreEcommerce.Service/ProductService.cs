using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Data.Models;

namespace NetCoreEcommerce.Service
{
    public class ProductService : IProduct
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

		public void DeleteProduct(int id)
		{
            var product = GetById(id);
            if(product == null)
            {
                throw new ArgumentException();
            }
            _context.Remove(product);
            _context.SaveChanges();
		}

		public void EditProduct(Product product)
        {
            var model = _context.Products.First(f => f.Id == product.Id);
            _context.Entry<Product>(model).State = EntityState.Detached;
            _context.Update(product);
            _context.SaveChanges();
        }

		public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category );
        }

        public Product GetById(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetFilteredProducts(int id, string searchQuery)
        {
            
            if(string.IsNullOrEmpty(searchQuery) || string.IsNullOrWhiteSpace(searchQuery))
            {
                return GetProductsByCategoryId(id);
            }

            return GetFilteredProducts(searchQuery).Where(p => p.Category.Id == id);
        }

        public IEnumerable<Product> GetFilteredProducts(string searchQuery)
        {
            var queries = string.IsNullOrEmpty(searchQuery) ? null : Regex.Replace(searchQuery, @"\s+", " ").Trim().ToLower().Split(" ");
            if(queries == null)
            {
                return GetPreferred(10);
            }

            return GetAll().Where(item => queries.Any(query => (item.Name.ToLower().Contains(query))));
        }

        public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
        {
            return GetAll().Where(p => p.Category.Id == categoryId);
        }

        public IEnumerable<Product> GetPreferred(int count)
        {
            return GetAll().OrderByDescending(p => p.Id).Where(p => p.IsPreferedProduct && p.InStock != 0).Take(count);
        }

        public void NewProduct(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
        }
    }
}
