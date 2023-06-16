using System;
namespace WebApi.Common.Types
{
    public class PagingRequestModel
    {
        public int? Page { get; set; }
        public int? PageLength { get; set; }
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }

        public PagingRequestModel()
        {

        }
    }

    public class PagingResponseModel
    {
        public int? Page { get; set; }
        public int? PageLength { get; set; }
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
        public int? TotalCount { get; set; }

        public PagingResponseModel()
        {

        }
    }
}

