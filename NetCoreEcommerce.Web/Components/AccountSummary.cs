using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreEcommerce.Data.Models;
using NetCoreEcommerce.Web.Models.Account;

namespace NetCoreEcommerce.Web.Components
{
    public class AccountSummary : ViewComponent
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountSummary(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		private ApplicationUser _user;

		public IViewComponentResult Invoke()
		{
			GetUser().Wait();
			
			if(_user != null)
			{
                var model = new AccountSummaryModel
                {
                    ImageUrl = _user.ImageUrl,
                    Name = $"{_user.FirstName} {_user.LastName}"
                };

				return View(model);
			}
			return View();
		}

		private async Task GetUser()
		{
			_user = await _userManager.FindByNameAsync(User.Identity.Name);
		}
	}
}
