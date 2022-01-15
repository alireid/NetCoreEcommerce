﻿using System.Collections.Generic;

namespace NetCoreEcommerce.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
