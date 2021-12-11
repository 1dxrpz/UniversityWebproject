using System;
using System.Collections.Generic;
using System.Text;

namespace DxORM.Interfaces
{
	public interface IPostgreDatabase
	{
		public void AddTable<T>(DbSet<T> table) where T : class, new();
		public void OnCreate();
	}
}