using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Models
{
	public class Video
	{
		public Guid Id { get; set; }
		public string Link { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Season { get; set; }
	}
}
