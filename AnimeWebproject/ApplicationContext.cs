using AnimeWebproject.Models;
using DxORM;
using DxORM.Services;

namespace AnimeWebproject
{
	public class ApplicationContext : DbContext
	{
		public DbSet<User> Users;
		public DbSet<Comment> Comments;
		public DbSet<VideoComment> VideoComments;
		public DbSet<Video> Videos;

		public ApplicationContext() : base()
		{
			Users = new DbSet<User>("Users", this);
			Comments = new DbSet<Comment>("Comments", this);
			VideoComments = new DbSet<VideoComment>("VideoComments", this);
			Videos = new DbSet<Video>("Videos", this);
		}
		public override void OnCreate()
		{
			AddTable<User>(Users);
			AddTable<Comment>(Comments);
			AddTable<VideoComment>(VideoComments);
			AddTable<Video>(Videos);
		}
	}
}