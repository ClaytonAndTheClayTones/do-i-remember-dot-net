using System;
using System.Reflection;
using WebApi.Helpers;

namespace WebApi.Helpers
{
 
	public interface IDbUtils
	{
        string? BuildUpdateQuery(string tableName, Dictionary<string, object?> updates);
    }
	 
	public class DbUtils : IDbUtils
	{
		public DbUtils()
		{
		}

		public string? BuildUpdateQuery(string tableName, Dictionary<string, object?> updates)
		{
		 

            if (!updates.Any(x => x.Value != null))
			{
				return null;
            }

			string query = $"UPDATE {tableName} SET";

			int paramCount = 0;

			foreach(KeyValuePair<string, object?> kvp in updates)
			{
				if (kvp.Value != null)
				{  
					query += $"{(paramCount++ > 0 ? "," : "")} {kvp.Key} = @{kvp.Key}";
				}
			}

			query += " WHERE id = @id;";

			return query;
        }

    }

	public static class DbExtensions
	{ 
        public static bool IsNumber(this Object value)
        {
            return value is sbyte
         || value is byte
         || value is short
         || value is ushort
         || value is int
         || value is uint
         || value is long
         || value is ulong
         || value is float
         || value is double
         || value is decimal;
        }
    }
}

