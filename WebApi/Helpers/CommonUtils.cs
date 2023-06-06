using System;
using System.ComponentModel;
using System.Reflection;
using Dapper;
using WebApi.Helpers;

namespace WebApi.Helpers
{

    public interface ICommonUtils
    {
        List<Guid> ConvertDelimitedStringToGuidList(string input, string? delimiter = ",");
    }


    public class CommonUtils : ICommonUtils
    {
        public CommonUtils()
        {
        }

        public List<Guid> ConvertDelimitedStringToGuidList(string input, string? delimiter = ",")
        {
            List<string> idTokens = input.Split(delimiter).ToList();

            List<Guid> guids = idTokens.Where<string>(x =>
            {
                Guid guid;
                bool isGuid = Guid.TryParse(x, out guid);
                return isGuid;
            }).Select(x =>
            {
                Guid guid = Guid.Parse(x);
                return guid;
            }).ToList();

            return guids;
        }
    }

    public static class CommonExtensions
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

