using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Models
{
	public class Video
	{
		public Guid Id { get; set; }
		public String Link { get; set; }
		public String Name { get; set; }
		public String Description { get; set; }
	}
}
