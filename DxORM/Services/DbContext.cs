using DxORM;
using DxORM.Interfaces;
using DxORM.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MyConfigServiceCollectionExtensions
	{
		public static IServiceCollection AddPostgreSql<T>(
			 this IServiceCollection services, string connectionString) where T : DbContext, new()
		{
			T service = new T();
			service.connection.ConnectionString = connectionString;
			services.AddSingleton(service);
			service.OnCreate();
			return services;
		}
		public static IServiceCollection AddEncryption(
			 this IServiceCollection services, string key)
		{
			services.AddSingleton(new Encryption(key));
			return services;
		}
	}
}
namespace DxORM.Services
{
	public class Options
	{
		public string ConnectionString { get; set; }
	}
	public class DbContext : IPostgreDatabase
	{
		private TypeParser TypeParser = new TypeParser();
		public NpgsqlCommand command;
		public NpgsqlConnection connection = new NpgsqlConnection();

		internal string ConnectionString = "";
		public DbContext()
		{
			ConnectionString = connection.ConnectionString;
		}
		public void AddTable<T>(DbSet<T> table) where T : class, new()
		{
			connection.Open();
			var _sql = $"CREATE TABLE IF NOT EXISTS public.\"{table.Name}\"" +
				"(" +
				PostgreTypeNotation(typeof(T)) +
				$"CONSTRAINT \"id_{table.Name}_pkey\" PRIMARY KEY (\"id\")" +
				")" +
				"TABLESPACE pg_default;" +
				$"ALTER TABLE public.\"{table.Name}\"" +
				" OWNER to postgres;";
			command = new NpgsqlCommand(_sql, connection);
			command.ExecuteNonQuery();
			connection.Close();
		}
		private string PostgreTypeNotation(Type type)
		{
			string res = "";
			foreach (var item in type.GetProperties())
			{
				var propName = item.Name.ToString();
				var propType = TypeParser.Parse(item.PropertyType);
				res += $"{propName} {propType},";
			}
			return res;
		}
		public virtual void OnCreate()
		{

		}
	}
}
