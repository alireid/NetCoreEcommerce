using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreEcommerce.Data.Models
{
    public class ShoppingCart
	{
		private readonly ApplicationDbContext _context;

		public ShoppingCart(ApplicationDbContext context)
		{
			_context = context;
		}

		public string Id { get; set; }
		public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }

		public static ShoppingCart GetCart(IServiceProvider services)
		{
			ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
			var context = services.GetService<ApplicationDbContext>();
			string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

			session.SetString("CartId", cartId);
			return new ShoppingCart(context) { Id = cartId };
		}

		public bool AddToCart(Product product, int amount)
		{
			if(product.InStock == 0 || amount == 0)
			{
				return false;
			}
			
			var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
				s => s.Product.Id == product.Id && s.ShoppingCartId == Id);
            var isValidAmount = true;
			if (shoppingCartItem == null)
			{
                if (amount > product.InStock)
                {
                    isValidAmount = false;
                }
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = Id,
                    Product = product,
                    Amount = Math.Min(product.InStock, amount)
				};
				_context.ShoppingCartItems.Add(shoppingCartItem);
			}
			else
			{
                if(product.InStock - shoppingCartItem.Amount - amount >= 0)
                {
                    shoppingCartItem.Amount +=  amount;
                }
                else
                {
					shoppingCartItem.Amount += (product.InStock - shoppingCartItem.Amount);
                    isValidAmount = false;
                }
            }

			_context.SaveChanges();
            return isValidAmount;
		}

		public int RemoveFromCart(Product product)
		{
			var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
				s => s.Product.Id == product.Id && s.ShoppingCartId == Id);
			int localAmount = 0;
			if (shoppingCartItem != null)
			{
				if (shoppingCartItem.Amount > 1)
				{
					shoppingCartItem.Amount--;
					localAmount = shoppingCartItem.Amount;
				}
				else
				{
					_context.ShoppingCartItems.Remove(shoppingCartItem);
				}
			}

			_context.SaveChanges();
			return localAmount;
		}

		public IEnumerable<ShoppingCartItem> GetShoppingCartItems()
		{
			return ShoppingCartItems ??
				   (ShoppingCartItems = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == Id)
					   .Include(s => s.Product));
		}

		public void ClearCart()
		{
			var cartItems = _context
				.ShoppingCartItems
				.Where(cart => cart.ShoppingCartId == Id);

			_context.ShoppingCartItems.RemoveRange(cartItems);
			_context.SaveChanges();
		}

		public decimal GetShoppingCartTotal()
		{
			Decimal total = 0;

			List<ShoppingCartItem> orderItems = _context.ShoppingCartItems.Include(x => x.Product).Where(c => c.ShoppingCartId == Id).ToList();
				
			foreach(ShoppingCartItem i in orderItems)
            {
				total += i.Product.Price * i.Amount;
			}

			return total;
		}

	}
}
