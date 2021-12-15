using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeWebproject.Models.ViewModels
{
	public class SettingsViewModel
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public IFormFile Avatar { get; set; }
	}
}
