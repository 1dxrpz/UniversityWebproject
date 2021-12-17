using AnimeWebproject.Controllers;
using AnimeWebproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Pages
{
	public class IndexModel : PageModel
	{
		ApplicationContext db;
		IJWTAuthManager _jWTAuthManager;

		public IndexModel(ApplicationContext context, IJWTAuthManager jWTAuthManager)
		{
			db = context;
			_jWTAuthManager = jWTAuthManager;
		}
		
		public bool isAuthentificated = false;
		public PageResult Get()
		{
			isAuthentificated = GetUser() != null;
			
			return Page();
		}
		public User GetUser()
		{
			if (Request.Cookies["token"] == null || Request.Cookies["token"] == "")
				return null;

			var stream = Request.Cookies["token"];
			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(stream);
			var _token = jsonToken as JwtSecurityToken;

			var CurrentId = _token.Claims.First(claim => claim.Type == "nameid").Value;

			var user = db.Users.FirstOrDefault(x => x.Id.ToString() == CurrentId);
			if (user == null)
			{
				return null;
			}
			return user;
		}
	}
}
