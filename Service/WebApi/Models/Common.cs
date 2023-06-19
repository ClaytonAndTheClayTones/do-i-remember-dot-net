namespace WebApi.Models.Common;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

[BindProperties]
public class PagingRequestInfo
{
    [FromQuery]
    public int? Page { get; set; }

    [FromQuery]
    public int? PageLength { get; set; }

    [FromQuery]
    public string? SortBy { get; set; }

    [FromQuery]
    public bool? IsDescending { get; set; }
}

public class PagingInfo
{
    public int? Page { get; set; }

    public int? PageLength { get; set; }

    public string? SortBy { get; set; }

    public bool? IsDescending { get; set; }
}

public class PagingResultInfo
{
    public int Page { get; set; }

    public int PageLength { get; set; }

    public string? SortBy { get; set; }

    public bool? IsDescending { get; set; }

    public long TotalCount { get; set; }
}

public class PagingResponseInfo
{
    public int Page { get; set; }

    public int PageLength { get; set; }

    public string? SortBy { get; set; }

    public bool? IsDescending { get; set; }

    public int TotalCount { get; set; }
}

public class PagedList<T>
{
    public PagingResultInfo? PagingInfo { get; set; }
    public List<T> Items { get; set; }

    public PagedList()
    {
        Items = new List<T>();
    }
}

public class PagedListResponse<T>
{
    public PagingResponseInfo? PagingInfo { get; set; }
    public List<T> Items { get; set; }

    public PagedListResponse()
    {
        Items = new List<T>();
    }
}