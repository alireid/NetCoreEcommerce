namespace NetCoreEcommerce.Web.Models.ShoppingCart
{
    public class ShoppingCartIndexModel
    {
        public NetCoreEcommerce.Data.Models.ShoppingCart ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public string ReturnUrl { get; set; }
    }
}
