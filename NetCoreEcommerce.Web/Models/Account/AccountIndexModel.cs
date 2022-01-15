using System.Collections.Generic;

namespace NetCoreEcommerce.Web.Models.Account
{
    public class AccountIndexModel
	{
		public IEnumerable<AccountProfileModel> Accounts { get; set; }
		public string SearchQuery { get; set; }
	}
}