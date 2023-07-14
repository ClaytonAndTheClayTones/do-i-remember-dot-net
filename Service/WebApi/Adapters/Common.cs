namespace WebApi.Adapters.Common;
 
using WebApi.Models.Common;
using WebApi.Helpers;

public interface IUpdateModelAdapter<TUpdateRequest, TUpdateModel>
{
    TUpdateModel convertFromUpdateRequestToUpdateModel(TUpdateRequest model);
    object convertFromUpdateModelToDatabaseModel(TUpdateModel model);
}

public interface IModelAdapter<TCreateRequest, TCreateModel, TSearchRequest, TDatabaseModel, TModel, TSearchModel, TResponse>
{
    TCreateModel convertFromCreateRequestToCreateModel(TCreateRequest request);
    TSearchModel convertFromSearchRequestToSearchModel(TSearchRequest request);

    object convertFromCreateModelToDatabaseModel(TCreateModel model); 
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

