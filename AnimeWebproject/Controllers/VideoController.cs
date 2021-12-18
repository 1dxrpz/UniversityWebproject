using AnimeWebproject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeWebproject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VideoController : Controller
	{
		public ApplicationContext db;
		public VideoController(ApplicationContext context)
		{
			db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost("addVideo")]
		public RequestStatus Post([FromForm] Video model)
		{
			var video = db.Videos.FirstOrDefault(x => x.Link == model.Link);
			if (video == null)
			{
				model.Id = Guid.NewGuid();
				db.Videos.Add(model);
				return RequestStatus.Succsess;
			}
			else
				return RequestStatus.Fail;
		}
		[HttpGet("getVideo")]
		public Video Get(string id)
		{
			var video = db.Videos.FirstOrDefault(x => x.Id.ToString() == id);
			if (video != null)
			{
				return video;
			}
			else
				return new Video();
		}
		[HttpGet("getVideos")]
		public List<Video> GetVideos()
		{
			return db.Videos.Select();
		}

		[HttpPost("addComment")]
		public void PostComment(string video_id, string text, string user_id)
		{
			Comment comment = new Comment()
			{
				Author = Guid.Parse(user_id),
				Id = Guid.NewGuid(),
				Text = text,
				VideoId = Guid.Parse(video_id),
			};
			db.Comments.Add(comment);
		}

		[HttpGet("getComments")]
		public List<Comment> GetComment(string video_id)
		{
			var a = db.Comments.Select(x => x.VideoId.ToString() == video_id);
			return a;
		}
	}
}