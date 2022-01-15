using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Data.Models;
using NetCoreEcommerce.Web.Models.ShoppingCart;

namespace NetCoreEcommerce.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProduct _productService;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IProduct productService, ShoppingCart shoppingCart)
        {
            _productService = productService;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index(bool isValidAmount = true, string returnUrl = "/")
        {
            _shoppingCart.GetShoppingCartItems();

            var model = new ShoppingCartIndexModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
                ReturnUrl = returnUrl
            };

            if (!isValidAmount)
            {
                ViewBag.InvalidAmountText = "*There were not enough items in stock to add*";
            }

            return View("Index", model);
        }

        [HttpGet]
        [Route("/ShoppingCart/Add/{id}/{returnUrl?}")]
        public IActionResult Add(int id, int? amount = 1, string returnUrl=null )
        {
            var product = _productService.GetById(id);
            returnUrl = returnUrl.Replace("%2F", "/");
            bool isValidAmount = false;
            if (product != null)
            {
                isValidAmount = _shoppingCart.AddToCart(product, amount.Value);
            }

            return Index(isValidAmount, returnUrl);
        }

        public IActionResult Remove(int productId)
        {
            var product = _productService.GetById(productId);
            if (product != null)
            {
                _shoppingCart.RemoveFromCart(product);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Back(string returnUrl="/")
        {
            return Redirect(returnUrl);
        }
    }
}