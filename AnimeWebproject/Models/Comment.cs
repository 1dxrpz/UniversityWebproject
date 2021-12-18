using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Models
{
	public class Comment
	{
		public Guid Id { get; set; }
		public Guid Author { get; set; }
		public Guid VideoId { get; set; }
		public String Text { get; set; }
	}
}
