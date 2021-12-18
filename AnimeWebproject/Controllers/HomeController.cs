using AnimeWebproject.Models;
using AnimeWebproject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		public ApplicationContext db;
		public HomeController(ApplicationContext context)
		{
			db = context;
		}
		public void OnGet()
		{
			
		}
		[HttpPost("settings")]
		public byte[] Post([FromForm] SettingsViewModel model)
		{
			byte[] imageData = null;
			var user = GetUser();
			user.Username = model.Username;
			user.Email = model.Email;
			user.Password = model.Password;
			user.Description = model.Description;
			if (model.Avatar != null)
			{
				using (var binaryReader = new BinaryReader(model.Avatar.OpenReadStream()))
				{
					imageData = binaryReader.ReadBytes((int)model.Avatar.Length);
				}
				user.Avatar = imageData;
			}
			db.Users.Update(user);
			
			return imageData;
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
