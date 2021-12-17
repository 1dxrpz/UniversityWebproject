using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Models
{
	public class VideoComment
	{
		public Guid Id { get; set; }
		public Guid VideoId { get; set; }
		public Guid CommentId { get; set; }
	}
}
