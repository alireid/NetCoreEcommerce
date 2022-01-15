using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Data.Models;
using NetCoreEcommerce.Web.DataMapper;
using NetCoreEcommerce.Web.Models.Order;

namespace NetCoreEcommerce.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrder _orderService;
        private readonly IProduct _productService;
        private readonly ShoppingCart _shoppingCart;
        private readonly Mapper _mapper;
        private static UserManager<ApplicationUser> _userManager;

        public OrderController(IOrder orderService, IProduct productService, ShoppingCart shoppingCart, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _shoppingCart = shoppingCart;
            _userManager = userManager;
            _productService = productService;
            _mapper = new Mapper();
        }

        public IActionResult Checkout()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            if (items.Count() == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some items first");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderIndexModel model)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (items.Count() == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some items first");
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);

                model.OrderTotal = items.Sum(item => item.Amount * item.Product.Price);
                var order = _mapper.OrderIndexModelToOrder(model, user);

                _orderService.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(model);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order";
            return View();
        }
    }
}