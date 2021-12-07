using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public AccountController(ApplicationContext context)
		{
			db = context;
		}
		[HttpPost("reg")]
		public string Post([FromForm] User model)
		{
			//db.Users.Add(model);
			var users = db.Users.FirstOrDefault(x => x.Username == model.Username);
			
			return users.ToString();
		}
	}
}
