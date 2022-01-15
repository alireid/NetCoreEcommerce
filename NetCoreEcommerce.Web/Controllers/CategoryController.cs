using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreEcommerce.Data;
using NetCoreEcommerce.Web.DataMapper;
using NetCoreEcommerce.Web.Models.Category;
using NetCoreEcommerce.Web.Models.Product;

namespace NetCoreEcommerce.Web.Controllers
{
    public class CategoryController : Controller
	{
		private readonly ICategory _categoryService;
		private readonly IProduct _productService;
        private readonly Mapper _mapper;

		public CategoryController(ICategory categoryService, IProduct productService)
		{
			_categoryService = categoryService;
			_productService = productService;
            _mapper = new Mapper();
		}

		public IActionResult Index()
		{
			var categories = _categoryService.GetAll().
				Select(category => new CategoryListingModel
				{
					Name = category.Name,
					Description = category.Description,
					Id = category.Id,
					ImageUrl = category.ImageUrl
				});

			var model = new CategoryIndexModel
			{
				CategoryList = categories
			};

			return View(model);
		}

		public IActionResult Topic(int id, string searchQuery)
		{
			var category = _categoryService.GetById(id);
			var allCategories = _categoryService.GetAll();
			var products = _productService.GetFilteredProducts(id, searchQuery);

			var productistings = products.Select(product => new ProductListingModel
			{
				Id = product.Id,
				Name = product.Name,
				InStock = product.InStock,
				Price = product.Price,
				ShortDescription = product.ShortDescription,
				Category = _mapper.ProductToCategoryListing(product),
				ImageUrl = product.ImageUrl
			});

			var model = new CategoryTopicModel
			{
				Category = _mapper.CategoryToCategoryListing(category),
				CategoryList = allCategories,
				Products = productistings
			};

			return View(model);
		}

		public IActionResult Search(int id, string searchQuery)
		{
			return RedirectToAction("Topic", new { id, searchQuery });
		}

		[Authorize(Roles = "Admin")]
		public IActionResult New()
		{
			ViewBag.ActionText = "create";
			ViewBag.Action = "New";
			ViewBag.CancelAction = "Index";
			ViewBag.SubmitText = "Create Category";
			return View("CreateEdit");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult New(CategoryListingModel model)
		{
			if (ModelState.IsValid)
			{
				var category = _mapper.CategoryListingToModel(model);
				_categoryService.NewCategory(category);
				return RedirectToAction("Topic", new { id = category.Id, searchQuery = "" });
			}

			ViewBag.ActionText = "create";
			ViewBag.Action = "New";
			ViewBag.CancelAction = "Index";
			ViewBag.SubmitText = "Create Category";

			return View("CreateEdit", model);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Edit(int id)
		{
            ViewBag.ActionText = "change";
            ViewBag.Action = "Edit";
            ViewBag.CancelAction = "Topic";
            ViewBag.SubmitText = "Save Changes";
            ViewBag.RouteId=id;

			var category = _categoryService.GetById(id);
			if (category != null)
			{
				var model = _mapper.CategoryToCategoryListing(category);
				return View("CreateEdit" ,model);
			}

			return View("CreateEdit");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult Edit(CategoryListingModel model)
		{
			if (ModelState.IsValid)
			{
				var category = _mapper.CategoryListingToModel(model);
				_categoryService.EditCategory(category);
				return RedirectToAction("Topic", new { id = category.Id, searchQuery = "" });
			}

            ViewBag.ActionText = "change";
            ViewBag.Action = "Edit";
            ViewBag.CancelAction = "Topic";
            ViewBag.SubmitText = "Save Changes";
            ViewBag.RouteId=model.Id;

			return View("CreateEdit",model);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Delete(int id)
		{
			_categoryService.DeleteCategory(id);

			return RedirectToAction("Index");
		}
	}
}
