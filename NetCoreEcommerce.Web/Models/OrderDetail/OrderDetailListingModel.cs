using NetCoreEcommerce.Web.Views.Product;

namespace NetCoreEcommerce.Web.Models.OrderDetail
{
    public class OrderDetailListingModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
        public ProductSummaryModel Product { get; set; }
        public int Amount { get; set; }
		public decimal Price { get; set; }	
	}
}