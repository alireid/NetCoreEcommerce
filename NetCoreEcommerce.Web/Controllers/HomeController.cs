using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Web.DataMapper;
using NetCoreEcommerce.Web.Models;

namespace NetCoreEcommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _productService;
        private readonly Mapper _mapper;

        public HomeController(IProduct productService)
        {
            _productService = productService;
            _mapper = new Mapper();
        }

        [Route("/")]
        public IActionResult Index()
        {
            var preferedProducts = _productService.GetPreferred(10);
            var model = _mapper.ProductsToHomeIndexModel(preferedProducts);
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery) || string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index");
            }

            var searchedProducts = _productService.GetFilteredProducts(searchQuery);
            var model = _mapper.ProductsToHomeIndexModel(searchedProducts);

            return View(model);
        }
    }
}
