namespace WebApi.Adapters.Common;

using System.Linq;
using WebApi.Models.Common;
using WebApi.Helpers;


public interface IModelAdapter<TCreateRequest, TUpdateRequest, TSearchRequest, TDatabaseModel, TModel, TSearchModel, TResponse>
{
    TSearchModel convertFromRequestToSearchModel(TSearchRequest request);

    object convertFromCreateRequestToDatabaseModel(TCreateRequest model);
    object convertFromUpdateRequestToDatabaseModel(TUpdateRequest model);
    List<ISearchTerm> convertFromSearchModelToSearchTerms(TSearchModel? model);

    TModel convertFromDatabaseModelToModel(TDatabaseModel model);

    TResponse convertFromModelToResponseModel(TModel model);
}


public interface IPagingAdapter
{
    PagingInfo convertFromPagingRequestInfoToPagingInfo(PagingRequestInfo model, int defaultPageLength = 25, int maxPageLength = 200);
    PagingResponseInfo convertFromPagingResultInfoToPagingResponseInfo(PagingResultInfo model);
}

public class PagingAdapter : IPagingAdapter
{
    public PagingInfo convertFromPagingRequestInfoToPagingInfo(PagingRequestInfo model, int defaultPageLength = 25, int maxPageLength = 200)
    {
        PagingInfo info = new PagingInfo()
        {
            Page = model.Page == null ? 1 : Math.Max(1, (int)model.Page),
            PageLength = model.PageLength == null ? defaultPageLength : Math.Max(1, Math.Min(maxPageLength, (int)model.PageLength)),
            SortBy = model.SortBy,
            IsDescending = model.IsDescending == null ? false : model.IsDescending
        };

        return info;
    }

    public PagingResponseInfo convertFromPagingResultInfoToPagingResponseInfo(PagingResultInfo model)
    {
        PagingResponseInfo response = new PagingResponseInfo()
        {
            Page = model.Page,
            PageLength = model.PageLength,
            SortBy = model.SortBy,
            IsDescending = model.IsDescending,
            TotalCount = (int)Math.Min((long)Int32.MaxValue, model.TotalCount)
        };

        return response;
    }
}

