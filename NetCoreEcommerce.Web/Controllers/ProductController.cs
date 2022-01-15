using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Web.DataMapper;
using NetCoreEcommerce.Web.Models.Category;
using NetCoreEcommerce.Web.Models.Product;

namespace NetCoreEcommerce.Web.Controllers
{
    public class ProductController : Controller
	{
		private readonly ICategory _categoryService;
		private readonly IProduct _productService;
        private readonly Mapper _mapper;

		public ProductController(ICategory categoryService, IProduct productService)
		{
			_categoryService = categoryService;
			_productService = productService;
            _mapper = new Mapper();
		}
        
        [Route("Products/{id}")]
		public IActionResult Index(int id)
		{
			var product = _productService.GetById(id);

			var model = new ProductIndexModel
			{
				Id = product.Id,
				Name = product.Name,
				ImageUrl = product.ImageUrl,
				InStock = product.InStock,
				Price = product.Price,
				Description = product.ShortDescription + "\n" + product.LongDescription,
				CategoryId = product.Category.Id,
				CategoryName = product.Category.Name
			};

			return View(model);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult New(int categoryId = 0)
		{
			GetCategoriesForDropDownList();
			NewProductModel model = new NewProductModel
			{
				CategoryId = categoryId
			};

			ViewBag.ActionText = "create";
			ViewBag.Action = "New";
			ViewBag.CancelAction = "Topic";
			ViewBag.SubmitText = "Create Product";
            ViewBag.RouteId = categoryId;
            ViewBag.ControllerName = "Category";

			if(categoryId == 0)
			{
				ViewBag.CancelAction = "Index";
				ViewBag.ControllerName = "Home";
			}

			return View("CreateEdit",model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult New(NewProductModel model)
		{
			if (ModelState.IsValid && _categoryService.GetById(model.CategoryId.Value) != null)
			{
				var product = _mapper.NewProductModelToProduct(model, true, _categoryService);
				_productService.NewProduct(product);
				return RedirectToAction("Index", new { id = product.Id });
			}
			GetCategoriesForDropDownList();

			ViewBag.ActionText = "create";
			ViewBag.Action = "New";
            ViewBag.ControllerName = "Category";
			ViewBag.CancelAction = "Topic";
			ViewBag.SubmitText = "Create product";
            ViewBag.RouteId = model.CategoryId;

			return View("CreateEdit",model);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Edit(int id)
		{
			ViewBag.ActionText = "change";
			ViewBag.Action = "Edit";
			ViewBag.CancelAction = "Index";
			ViewBag.SubmitText = "Save Changes";
            ViewBag.ControllerName = "Product";
			ViewBag.RouteId = id;

			GetCategoriesForDropDownList();
			
            var product = _productService.GetById(id);
			if (product != null)
			{
				var model = _mapper.ProductToNewProductModel(product);
				return View("CreateEdit", model);
			}

			return View("CreateEdit");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult Edit(NewProductModel model)
		{
			if (ModelState.IsValid)
			{
				var product = _mapper.NewProductModelToProduct(model, false, _categoryService);
				_productService.EditProduct(product);
				return RedirectToAction("Index", new { id = model.Id });
			}
            
			ViewBag.ActionText = "change";
			ViewBag.Action = "Edit";
			ViewBag.CancelAction = "Index";
			ViewBag.SubmitText = "Save Changes";
            ViewBag.ControllerName = "Product";
			ViewBag.RouteId = model.Id;
			GetCategoriesForDropDownList();

			return View("CreateEdit", model);
		}

		[Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var categoryId = _productService.GetById(id).CategoryId;
            _productService.DeleteProduct(id);

            return RedirectToAction("Topic", "Category", new { id = categoryId, searchQuery = "" });
        }

		private void GetCategoriesForDropDownList()
		{
			var categories = _categoryService.GetAll().Select(category => new CategoryDropdownModel
			{
				Id = category.Id,
				Name = category.Name
			});
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
		}
	}
}
