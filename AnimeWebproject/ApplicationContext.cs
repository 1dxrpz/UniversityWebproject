using DAL.Models;
using DxORM;
using DxORM.Services;

namespace AnimeWebproject
{
	public class ApplicationContext : DbContext
	{
		public DbSet<User> Users;
		public ApplicationContext() : base()
		{
			Users = new DbSet<User>("Users", this);
		}
		public override void OnCreate()
		{
			//AddTable<User>(Users);
		}
	}
}