using AnimeWebproject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Pages
{
	public class IndexModel : PageModel
	{
		public string test = "Hello world";

		HomeController controller;
		public IndexModel(ApplicationContext context)
		{
			controller = new HomeController(context);
		}
		public PageResult Get()
		{
			return Page();
		}
	}
}
