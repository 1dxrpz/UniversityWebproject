using DxORM.Services;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace DxORM
{
	/// <summary>
	/// Database table
	/// </summary>
	/// <typeparam name="T">Table model</typeparam>
	public class DbSet<T> where T : class, new()
	{
		DbContext db;
		private string _name = "";
		internal string Name {
			get => _name;
			private set { }
		}
		public DbSet(string name, DbContext context)
		{
			_name = name;
			db = context;
		}
		public void Add(T model)
		{
			int i = 0;
			string cols = "";
			string vals = "";
			foreach (var item in typeof(T).GetProperties())
			{
				var propName = item.Name.ToString().ToLower();
				cols += $"{propName}";
				i++;
				if (propName.ToString() == "id")
					item.SetValue(model, (Guid)Convert.ChangeType(Guid.NewGuid(), item.PropertyType));
				vals += item.PropertyType == typeof(int) ?
					$"{item.GetValue(model)}" :
					item.PropertyType != typeof(byte[]) ? $"'{item.GetValue(model)}'" : $"@image";
				if (i != typeof(T).GetProperties().Length)
				{
					cols += ',';
					vals += ',';
				}
			}

			string result = $"({cols}) VALUES({vals});";
			db.connection.Open();
			var _sql = $"INSERT INTO \"{_name}\" {result};";
			db.command = new NpgsqlCommand(_sql, db.connection);
			foreach (var item in typeof(T).GetProperties())
			{
				if (item.PropertyType == typeof(byte[]))
				{
					NpgsqlParameter param = db.command.CreateParameter();
					param.ParameterName = "@image";
					param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
					param.Value = item.GetValue(model);
					db.command.Parameters.Add(param);
				}
			}
			db.command.ExecuteScalar();
			db.connection.Close();
		}
		public void Update(T model)
		{
			var id = typeof(T).GetProperties()
				.First(x => x.Name.ToString().ToLower() == "id")
				.GetValue(model).ToString();
			var vals = "";
			int i = 0;
			foreach (var item in typeof(T).GetProperties())
			{
				vals += $"{item.Name.ToLower()}=";
				vals += item.PropertyType == typeof(int) ?
					$"{item.GetValue(model)}" :
					item.PropertyType != typeof(byte[]) ? $"'{item.GetValue(model)}'" : $"@image";
				i++;
				if (i != typeof(T).GetProperties().Length)
					vals += ',';
			}

			var result = $"SET {vals} WHERE \"id\"='{id}'";
			db.connection.Open();
			var _sql = $"UPDATE \"{_name}\" {result};";
			db.command = new NpgsqlCommand(_sql, db.connection);
			foreach (var item in typeof(T).GetProperties())
			{
				if (item.PropertyType == typeof(byte[]))
				{
					NpgsqlParameter param = db.command.CreateParameter();
					param.ParameterName = "@image";
					param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
					param.Value = item.GetValue(model);
					db.command.Parameters.Add(param);
				}
			}
			db.command.ExecuteScalar();
			db.connection.Close();
		}
		public List<T> Select()
		{
			List<T> result = new List<T>();
			db.connection.Open();
			var _sql = $"SELECT * FROM \"{_name}\";";
			db.command = new NpgsqlCommand(_sql, db.connection);
			using (var reader = db.command.ExecuteReader())
			{
				while (reader.Read())
				{
					T model = new T();
					int i = 0;
					foreach (var item in typeof(T).GetProperties())
					{
						var modelPropName = item.Name;
						var modelPropValue = reader.GetValue(i);
						item.SetValue(model, Convert.ChangeType(reader.GetValue(i), item.PropertyType));
						i++;
					}
					result.Add(model);
				}
			}
			db.connection.Close();
			return result;
		}
		public List<T> Select(Predicate<T> match)
		{
			return Select().FindAll(match);
		}
		public T FirstOrDefault(Func<T, bool> predicate)
		{
			try
			{
				return Select().FirstOrDefault(predicate);
			}
			catch
			{
				return null;
			}
		}
		public void Remove(T model)
		{
			try
			{
				var id = typeof(T).GetProperties()
					.First(x => x.Name.ToString().ToLower() == "id")
					.GetValue(model).ToString();

				db.connection.Open();
				var _sql = $"DELETE FROM \"{_name}\" WHERE \"id\"='{id}'";
				db.command = new NpgsqlCommand(_sql, db.connection);
				db.command.ExecuteScalar();
				db.connection.Close();
			} catch { }
		}
		public bool Exists(Predicate<T> match)
		{
			return Select(match).Count != 0;
		}
	}
}