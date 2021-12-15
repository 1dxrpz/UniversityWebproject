using System;
using System.Collections.Generic;
using System.Text;

namespace DxORM
{
	public class TypeParser
	{
		Dictionary<Type, string> types = new Dictionary<Type, string>();
		public TypeParser()
		{
			types.Add(typeof(int), "integer");
			types.Add(typeof(string), "character varying");
			types.Add(typeof(Guid), "uuid");
			types.Add(typeof(byte[]), "bytea");
		}

		public string Parse(Type type)
		{
			return types[type];
		}
	}
}
