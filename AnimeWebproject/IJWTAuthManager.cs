using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject
{
	public interface IJWTAuthManager
	{
		string Authenticate(User user);
	}
}
