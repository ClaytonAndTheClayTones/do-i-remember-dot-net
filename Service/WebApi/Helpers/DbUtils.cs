using System.Collections.Generic;
using System.ComponentModel; 
using Dapper; 
using WebApi.Models.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi.Helpers
{

    public enum LikeTypes
    {
        StartsWith,
        EndsWith,
        Like,
        Exact
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
        public ClauseAndParameters GenerateClauseAndParameters();
    }

    public class ExactMatchSearchTerm<T> : ISearchTerm
    {
        public string ColumnName { get; set; }
        public bool IgnoreCase { get; set; }
        public T Value { get; set; }

        public ExactMatchSearchTerm(string columnName, T value, bool ignoreCase = false)
        {
            this.ColumnName = columnName;
            this.Value = value;
            this.IgnoreCase = ignoreCase;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            result.Clause = $"{(this.IgnoreCase == true ? "LOWER(" : "")}{this.ColumnName}{(this.IgnoreCase == true ? ")" : "")} = {(this.IgnoreCase == true ? "LOWER(" : "")}@{this.ColumnName}{(this.IgnoreCase == true ? ")" : "")}";

            result.Parameters.Add(this.ColumnName, this.Value);

            return result;
        }
    }

    public class LikeSearchTerm : ISearchTerm
    { 
        public string ColumnName { get; set; }
        public string Value { get; set; }
        public bool IgnoreCase { get; set; }
        public LikeTypes LikeComparisonType { get; set; }


        public LikeSearchTerm(string columnName, string value, LikeTypes likeComparisonType, bool ignoreCase = true)
        {
            this.ColumnName = columnName;
            this.Value = value;
            this.LikeComparisonType = likeComparisonType;
            this.IgnoreCase = ignoreCase;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            result.Clause = $"{this.ColumnName} {(this.IgnoreCase == true ? "ILIKE" : "LIKE")} @{this.ColumnName}";

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
        public bool IgnoreCase { get; set; }

        public InArraySearchTerm(string columnName, List<T> values, bool ignoreCase = false)
        {
            this.ColumnName = columnName;
            this.Values = values;
            this.IgnoreCase = ignoreCase;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            result.Clause = $"{(this.IgnoreCase == true ? "LOWER(" : "")}{this.ColumnName}{(this.IgnoreCase == true ? ")" : "")} IN (\n";

            int index = 0;

            foreach (T item in this.Values)
            {
                result.Clause += $"{(index > 0 ? ",\n" : "")}{(this.IgnoreCase == true ? "LOWER(" : "")}@{this.ColumnName}_{index}{(this.IgnoreCase == true ? ")" : "")}";


                result.Parameters.Add($"{this.ColumnName}_{index++}", item);
            }

            result.Clause += "\n)";
            return result;
        }
    } 

    public class ComparisonSearchTermInput
    {

        public string ColumnName { get; set; }
        public string Value { get; set; }
        public bool IgnoreCase { get; set; }
        public LikeTypes LikeComparisonType { get; set; }

        public ComparisonSearchTermInput(string columnName, string value, LikeTypes likeComparisonType = LikeTypes.Exact, bool ignoreCase = true)
        {
            this.ColumnName = columnName;
            this.Value = value;
            this.LikeComparisonType = likeComparisonType;
            this.IgnoreCase = ignoreCase;
        } 
    }

    public class ComparisonSearchTerm : ISearchTerm
    {
        public List<ComparisonSearchTermInput> Comparisons = new List<ComparisonSearchTermInput>();

        public ComparisonSearchTerm()
        {
        }

        public ComparisonSearchTerm(List<ComparisonSearchTermInput> comparisons)
        {
            this.Comparisons = comparisons;
        }

        private ClauseAndParameters GenerateIndividualComparisonClauseAndParameters(ComparisonSearchTermInput input)
        {
            ClauseAndParameters result = new ClauseAndParameters();

            if (input.LikeComparisonType == LikeTypes.Exact)
            {
                result.Clause = $"{(input.IgnoreCase == true ? "LOWER(" : "")}{input.ColumnName}{(input.IgnoreCase == true ? ")" : "")} = {(input.IgnoreCase == true ? "LOWER(" : "")}@{input.ColumnName}{(input.IgnoreCase == true ? ")" : "")}";
                result.Parameters.Add(input.ColumnName, input.Value); 
            }

            else
            {
                result.Clause = $"{input.ColumnName} {(input.IgnoreCase == true ? "ILIKE" : "LIKE")} @{input.ColumnName}";

                string value = "";


                switch (input.LikeComparisonType)
                {
                    case LikeTypes.StartsWith:
                        {
                            value = $"{input.Value}%";
                            break;
                        }
                    case LikeTypes.EndsWith:
                        {
                            value = $"%{input.Value}";
                            break;
                        }
                    case LikeTypes.Like:
                        {
                            value = $"%{input.Value}%";
                            break;
                        }
                }

                result.Parameters.Add(input.ColumnName, value);
            }

            return result;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();

            int comparisonIndex = 0;

            this.Comparisons.ForEach(comparison =>
            {
                ClauseAndParameters clauseAndParameters = this.GenerateIndividualComparisonClauseAndParameters(comparison);

                if(comparisonIndex++ != 0)
                {
                    result.Clause += "\nOR ";
                }

                result.Clause += clauseAndParameters.Clause;

                clauseAndParameters.Parameters.ParameterNames.ToList().ForEach(paramName =>
                {
                    object toMove = clauseAndParameters.Parameters.Get<object>(paramName);
                    result.Parameters.Add(paramName, toMove);
                });
            });

            return result;
        }
    }

    public class DateRangeSearchTerm : ISearchTerm
    {
        public string ColumnName { get; set; }
        public DateTime? Min { get; set; }
        public DateTime? Max { get; set; }

        public DateRangeSearchTerm(string columnName, DateTime? min, DateTime? max)
        {
            this.ColumnName = columnName;
            this.Min = min;
            this.Max = max;
        }

        public ClauseAndParameters GenerateClauseAndParameters()
        {
            ClauseAndParameters result = new ClauseAndParameters();
             
            result.Clause = $"\n{this.ColumnName} BETWEEN COALESCE(@{this.ColumnName}_min, {this.ColumnName}) AND COALESCE(@{this.ColumnName}_max, {this.ColumnName})";
             
            result.Parameters.Add($"{this.ColumnName}_min", Min);
            result.Parameters.Add($"{this.ColumnName}_max", Max);

            return result;
        }
    }
     
    public class InsertQueryPackage
    {
        public string sql { get; set; }

        public DynamicParameters parameters { get; set; }

        public InsertQueryPackage(string sql, DynamicParameters parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }
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

    public interface IDbUtils
    {
        InsertQueryPackage BuildInsertQuery(string tableName, object insertObject);
        UpdateQueryPackage? BuildUpdateQuery(string tableName, Guid id, object updateObject);
        SelectQueryPackage BuildSelectQuery(string tableName, List<ISearchTerm> searchTerms, PagingInfo? pagingInfo, bool skipPaging = false);
        void BuildPagingInfo(ref SelectQueryPackage package, PagingInfo? pagingInfo);
    }


    public class SelectQueryPackage
    {
        public string sql { get; set; }

        public DynamicParameters parameters { get; set; }
        public PagingInfo? pagingInfoUsed { get; set; }


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

        public void BuildPagingInfo(ref SelectQueryPackage package, PagingInfo? pagingInfo)
        {
            if (pagingInfo == null)
            {
                pagingInfo = new PagingInfo()
                {
                    Page = 1,
                    PageLength = 25
                };
            }

            if (pagingInfo.SortBy != null)
            {
                package.sql += $"\nORDER BY @sort_by {(pagingInfo.IsDescending == true ? "DESC" : "ASC")}";
                package.parameters.Add("sort_by", pagingInfo.SortBy);
            }

            int page = pagingInfo.Page == null ? 1 : Math.Max(1, (int)pagingInfo.Page);
            int limit = pagingInfo.PageLength == null ? 25 : Math.Max(1, (int)pagingInfo.PageLength);

            int offset = (page - 1) * limit;

            package.sql += $"\nOFFSET @offset LIMIT @limit";

            package.parameters.Add("limit", limit);
            package.parameters.Add("offset", offset);

            package.pagingInfoUsed = pagingInfo;
        }

        public SelectQueryPackage BuildSelectQuery(string tableName, List<ISearchTerm> searchTerms, PagingInfo? pagingInfo, bool skipPaging = false)
        {
            SelectQueryPackage package = new SelectQueryPackage($"SELECT *{(skipPaging ? "" : ", count(*) over() as full_count")} FROM {tableName}", new DynamicParameters());

            if (searchTerms.Count > 0)
            {
                package.sql += "\nWHERE";

                int paramCount = 0;

                searchTerms.ForEach(x =>
                {
                    ClauseAndParameters clauseAndParameters = x.GenerateClauseAndParameters();

                    package.sql += $"{(paramCount++ > 0 ? " AND" : "")}{"\n"}{clauseAndParameters.Clause}";

                    foreach (string parameterName in clauseAndParameters.Parameters.ParameterNames)
                    {
                        package.parameters.Add(parameterName, clauseAndParameters.Parameters.Get<object>(parameterName));
                    }
                });

                if (!skipPaging)
                {
                    BuildPagingInfo(ref package, pagingInfo);
                }
            }

            return package;
        }

        public InsertQueryPackage BuildInsertQuery(string tableName, object insertObject)
        {
            Dictionary<string, object> insertDictionary = new Dictionary<string, object>();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(insertObject))
            {
                object? value = property.GetValue(insertObject);

                if (value != null)
                {
                    insertDictionary.Add(property.Name, value);
                }
            }

            if (insertDictionary.Count == 0)
            {
                return null;
            }

  

            string query = $"INSERT INTO {tableName}\n(";

            int paramCount = 0;
            string columnsString = "";
            string valuesString = "";
            DynamicParameters dbArgs = new DynamicParameters();

            for (int i = 0; i < insertDictionary.Count; i++)
            {

                columnsString += $"{(paramCount++ > 0 ? "," : "")}\n\t{insertDictionary.Keys.ToList()[i]}";
                valuesString += $"{(i > 0 ? "," : "")}\n\t@{insertDictionary.Keys.ToList()[i]}";
                dbArgs.Add(insertDictionary.Keys.ToList()[i], insertDictionary.Values.ToList()[i]);
            }

            query += $"{columnsString}\n)\nVALUES\n({valuesString}\n)\nRETURNING *;";
             
            InsertQueryPackage result = new InsertQueryPackage(query, dbArgs);

            return result;
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

            updateDictionary.Add("updated_at", DateTime.UtcNow);

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

