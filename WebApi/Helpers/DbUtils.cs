using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Dapper;
using WebApi.Helpers;

namespace WebApi.Helpers
{

    public enum LikeTypes
    {
        StartsWith,
        EndsWith,
        Like
    }

    public class DbParameter<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }

        public DbParameter(string key, T value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public class ClauseAndParameters
    {
        public string Clause { get; set; } = "";
        public DynamicParameters Parameters { get; set; } = new DynamicParameters();
    }


    public interface ISearchTerm
    {
        public string ColumnName { get; set; }

        public ClauseAndParameters GenerateClauseAndParameters();
    }

    public class ExactMatchSearchTerm<T> : ISearchTerm
    {
        public string ColumnName { get; set; }
        public T Value { get; set; }

        public ExactMatchSearchTerm(string columnName, T value)
        {
            this.ColumnName = columnName;
            this.Value = value;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            result.Clause = $"{this.ColumnName} = @{this.ColumnName}";

            result.Parameters.Add(this.ColumnName, this.Value);

            return result;
        }
    }

    public class LikeSearchTerm : ISearchTerm
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }
        public LikeTypes LikeComparisonType { get; set; }

        public LikeSearchTerm(string columnName, string value, LikeTypes likeComparisonType)
        {
            this.ColumnName = columnName;
            this.Value = value;
            this.LikeComparisonType = likeComparisonType;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            result.Clause = $"{this.ColumnName} LIKE @{this.ColumnName}";

            string value = "";


            switch (this.LikeComparisonType)
            {
                case LikeTypes.StartsWith:
                    {
                        value = $"{this.Value}%";
                        break;
                    }
                case LikeTypes.EndsWith:
                    {
                        value = $"%{this.Value}";
                        break;
                    }
                case LikeTypes.Like:
                    {
                        value = $"%{this.Value}%";
                        break;
                    }
            }

            result.Parameters.Add(this.ColumnName, value);

            return result;
        }
    }

    public class InArraySearchTerm<T> : ISearchTerm
    {
        public string ColumnName { get; set; }
        public List<T> Values { get; set; }

        public InArraySearchTerm(string columnName, List<T> values)
        {
            this.ColumnName = columnName;
            this.Values = values;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            result.Clause = $"{this.ColumnName} IN (\n";

            int index = 0;

            foreach (T item in this.Values)
            {
                result.Clause += $"{(index > 0 ? ",\n" : "")}@{this.ColumnName}_{index}";


                result.Parameters.Add($"{this.ColumnName}_{index++}", item);
            }

            result.Clause += "\n)";
            return result;
        }
    }


    public interface IDbUtils
    {
        UpdateQueryPackage? BuildUpdateQuery(string tableName, Guid id, object udpateObject);
        SelectQueryPackage? BuildSelectQuery(string tableName, List<ISearchTerm> searchTerms);
    }

    public class UpdateQueryPackage
    {
        public string sql { get; set; }

        public DynamicParameters parameters { get; set; }

        public UpdateQueryPackage(string sql, DynamicParameters parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }
    }

    public class SelectQueryPackage
    {
        public string sql { get; set; }

        public DynamicParameters parameters { get; set; }

        public SelectQueryPackage(string sql, DynamicParameters parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }
    }

    public class DbUtils : IDbUtils
    {
        public DbUtils()
        {
        }

        public SelectQueryPackage? BuildSelectQuery(string tableName, List<ISearchTerm> searchTerms)
        {
            string query = $"SELECT * FROM {tableName}";

            DynamicParameters parameters = new DynamicParameters();

            if (searchTerms.Count > 0)
            {

                query += "\nWHERE";

                int paramCount = 0;

                searchTerms.ForEach(x =>
                {
                    ClauseAndParameters clauseAndParameters = x.GenerateClauseAndParameters();

                    query += $"{(paramCount++ > 0 ? "," : "")}{"\n"}{clauseAndParameters.Clause}";

                    foreach (string parameterName in clauseAndParameters.Parameters.ParameterNames)
                    {
                        parameters.Add(parameterName, clauseAndParameters.Parameters.Get<object>(parameterName));
                    }
                });
            }

            SelectQueryPackage returnPackage = new SelectQueryPackage(query, parameters);

            return returnPackage;
        }

        public UpdateQueryPackage? BuildUpdateQuery(string tableName, Guid id, object updateObject)
        {
            Dictionary<string, object> updateDictionary = new Dictionary<string, object>();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(updateObject))
            {
                object? value = property.GetValue(updateObject);

                if (value != null)
                {
                    updateDictionary.Add(property.Name, value);
                }
            }


            if (updateDictionary.Count == 0)
            {
                return null;
            }

            string query = $"UPDATE {tableName} SET";

            int paramCount = 0;

            foreach (KeyValuePair<string, object> kvp in updateDictionary)
            {

                query += $"{(paramCount++ > 0 ? "," : "")} {kvp.Key} = @{kvp.Key}";

            }

            query += " WHERE id = @id RETURNING *;";

            var dbArgs = new DynamicParameters();

            dbArgs.Add("id", id);

            foreach (var pair in updateDictionary)
            {
                dbArgs.Add(pair.Key, pair.Value);
            }

            UpdateQueryPackage result = new UpdateQueryPackage(query, dbArgs);

            return result;
        }
    }
}

