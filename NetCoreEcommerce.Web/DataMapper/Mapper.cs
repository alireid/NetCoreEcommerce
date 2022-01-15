using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Data.Models;
using NetCoreEcommerce.Web.Models.Account;
using NetCoreEcommerce.Web.Models.Category;
using NetCoreEcommerce.Web.Models.Home;
using NetCoreEcommerce.Web.Models.Order;
using NetCoreEcommerce.Web.Models.OrderDetail;
using NetCoreEcommerce.Web.Models.Product;
using NetCoreEcommerce.Web.Views.Product;

namespace NetCoreEcommerce.Web.DataMapper
{
    public class Mapper
    {

        #region Category

        public Category CategoryListingToModel(CategoryListingModel model)
        {
            return new Category
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
            };
        }

        public CategoryListingModel ProductToCategoryListing(Product product)
        {
            var category = product.Category;
            return CategoryToCategoryListing(category);
        }

        public CategoryListingModel CategoryToCategoryListing(Category category)
        {
            return new CategoryListingModel
            {
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
                ImageUrl = category.ImageUrl
            };
        }

        #endregion

        #region Product

        public NewProductModel ProductToNewProductModel(Product product)
        {
            return new NewProductModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                InStock = product.InStock,
                IsPreferedProduct = product.IsPreferedProduct,
                LongDescription = product.LongDescription,
                Price = product.Price,
                ShortDescription = product.ShortDescription,
            };
        }


        public Product NewProductModelToProduct(NewProductModel model, bool newInstance, ICategory categoryService)
        {
            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Category = categoryService.GetById(model.CategoryId.Value),
                CategoryId = model.CategoryId.Value,
                ImageUrl = model.ImageUrl,
                InStock = model.InStock.Value,
                IsPreferedProduct = model.IsPreferedProduct.Value,
                LongDescription = model.LongDescription,
                Price = model.Price.Value,
                ShortDescription = model.ShortDescription,
            };

            if (!newInstance)
            {
                product.Id = model.Id;
            }

            return product;
        }

        private IEnumerable<ProductSummaryModel> ProductToProductSummaryModel(IEnumerable<Product> products)
        {
            return products.Select(product => new ProductSummaryModel
            {
                Name = product.Name,
                Id = product.Id
            });
        }

        #endregion

        #region Home

        public HomeIndexModel ProductsToHomeIndexModel(IEnumerable<Product> products)
        {

            var productsListing = products.Select(product => new ProductListingModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = CategoryToCategoryListing(product.Category),
                ImageUrl = product.ImageUrl,
                InStock = product.InStock,
                Price = product.Price,
                ShortDescription = product.ShortDescription
            });

            return new HomeIndexModel
            {
                ProductsList = productsListing
            };
        }

        #endregion

        #region Order

        public Order OrderIndexModelToOrder(OrderIndexModel model, ApplicationUser user)
        {
            return new Order
            {
                Id = model.Id,
                OrderPlaced = model.OrderPlaced,
                OrderTotal = model.OrderTotal,
                User = user,
                UserId = user.Id,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                ZipCode = model.ZipCode,
                //OrderLines = OrderDetailsListingModelToOrderDetails(model.OrderLines)
            };
        }

        private IEnumerable<OrderDetail> OrderDetailsListingModelToOrderDetails(IEnumerable<OrderDetailListingModel> orderLines)
        {
            return orderLines.Select(line => new OrderDetail
            {
                Amount = line.Amount,
                ProductId = line.Product.Id,
                Id = line.Id,
                OrderId = line.OrderId,
                Price = line.Price
            });
        }

        public IEnumerable<OrderIndexModel> OrdersToOrderIndexModels(IEnumerable<Order> orders)
        {
            return orders.Select(order => new OrderIndexModel
            {
                Id = order.Id,
                Address = order.Address,
                City = order.City,
                Country = order.Country,
                OrderPlaced = order.OrderPlaced,
                OrderTotal = order.OrderTotal,
                UserId = order.UserId,
                ZipCode = order.ZipCode,
                UserFullName = $"{order.User.FirstName} {order.User.LastName}",
                OrderLines = OrderDetailsToOrderDetailsListingModel(order.OrderLines)
            });
        }



        private IEnumerable<OrderDetailListingModel> OrderDetailsToOrderDetailsListingModel(IEnumerable<OrderDetail> orderLines)
        {
            if (orderLines == null)
            {
                orderLines = Enumerable.Empty<OrderDetail>();
            }
            return orderLines.Select(orderLine => new OrderDetailListingModel
            {
                Amount = orderLine.Amount,
                Id = orderLine.Id,
                OrderId = orderLine.OrderId,
                Price = orderLine.Price,
                Product = new Views.Product.ProductSummaryModel
                {
                    Id = orderLine.ProductId,
                    Name = orderLine.Product.Name
                },
                ProductId = orderLine.ProductId
            });
        }

        #endregion

        #region Account
        public AccountSettingsModel ApplicationUserToAccountSettingsModel(ApplicationUser user, string roleId)
        {
            return new AccountSettingsModel
            {
                Id = user.Id,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                ImageUrl = user.ImageUrl,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                RoleId = roleId
            };
        }

        public ApplicationUser AccountRegisterModelToApplicationUser(AccountRegisterModel login)
        {
            return new ApplicationUser
            {
                FirstName = login.FirstName,
                AddressLine1 = login.AddressLine1,
                AddressLine2 = login.AddressLine2,
                City = login.City,
                Country = login.Country,
                Email = login.Email,
                ImageUrl = login.ImageUrl,
                MemberSince = DateTime.Now,
                Balance = 0,
                LastName = login.LastName,
                UserName = login.Email,
                Orders = Enumerable.Empty<Order>(),
                PhoneNumber = login.PhoneNumber,
            };
        }

        public AccountProfileModel ApplicationUserToAccountProfileModel(ApplicationUser user, IOrder orderService,string role)
        {
            return new AccountProfileModel
            {
                Id = user.Id,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                Balance = user.Balance,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                ImageUrl = user.ImageUrl,
                LastName = user.LastName,
                MemberSince = user.MemberSince,
                PhoneNumber = user.PhoneNumber,
                MostPopularProducts = ProductToProductSummaryModel(orderService.GetUserMostPopularProducts(user.Id)),
                OrderCount = orderService.GetByUserId(user.Id).Count(),
                LatestOrders = OrdersToOrderIndexModels(orderService.GetUserLatestOrders(5, user.Id)),
                Role = role,
                TotalSpent = orderService.GetByUserId(user.Id).Sum(order => order.OrderTotal)
            };
        }

        public void AccountSettingsModelToApplicationUser(AccountSettingsModel model, ApplicationUser user)
        {
            user.City = model.City;
            user.AddressLine1 = model.AddressLine1;
            user.AddressLine2 = model.AddressLine2;
            user.Country = model.Country;
            user.FirstName = model.FirstName;
            user.ImageUrl = model.ImageUrl;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
        }

        public async Task<IEnumerable<AccountProfileModel>> ApplicationUsersToAccountProfileModelsAsync(IEnumerable<ApplicationUser> users, IOrder orderService, UserManager<ApplicationUser> userManager)
        {
            List<AccountProfileModel> models = new List<AccountProfileModel>(users.Count());

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                models.Add(ApplicationUserToAccountProfileModel(user, orderService, roles.FirstOrDefault()));
            }

            return models;
        }

        #endregion
    }
}
