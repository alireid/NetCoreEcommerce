using System;
using System.Collections.Generic;
using NetCoreEcommerce.Data.Enums;
using NetCoreEcommerce.Data.Models;

namespace NetCoreEcommerce.Data
{
    public interface IOrder
	{
		void CreateOrder(Order order);
        Order GetById(int orderId);
		IEnumerable<Order> GetByUserId(string userId);
		IEnumerable<Order> GetAll();
		IEnumerable<Order> GetUserLatestOrders(int count, string userId);
		IEnumerable<Product> GetUserMostPopularProducts(string id);
		IEnumerable<Order> GetFilteredOrders(
			string userId=null,
			OrderBy orderBy = OrderBy.None,
			int offset=0,
			int limit=10,
			decimal? minimalPrice = null,
			decimal? maximalPrice = null,
			DateTime? minDate = null,
			DateTime? maxDate = null,
			string zipCode = null
			);
	}
}