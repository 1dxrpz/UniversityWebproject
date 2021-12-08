using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnimeWebproject.Controllers
{
	public enum RequestStatus
	{
		Succsess,
		Fail
	}
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		public ApplicationContext db;
		IJWTAuthManager _jWTAuthManager;
		public AccountController(ApplicationContext context, IJWTAuthManager jWTAuthManager)
		{
			db = context;
			_jWTAuthManager = jWTAuthManager;
		}
		[HttpPost("reg")]
		public RequestStatus Post([FromForm] User model)
		{
			var users = db.Users.FirstOrDefault(x => x.Username == model.Username);
			if (users == null)
			{
				db.Users.Add(model);
				var token = _jWTAuthManager.Authenticate(model);
				Response.Cookies.Append("token", token);
				return RequestStatus.Succsess;
			}
			return RequestStatus.Fail;
		}
		[HttpGet("logout")]
		public void Logout()
		{
			var token = Request.Cookies["token"];
			if (token != null)
				Response.Cookies.Delete("token");
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
