namespace WebApi.Adapters.ArtistAdapter;

using WebApi.Models.Artists;
using WebApi.Helpers;

public interface IArtistAdapter
{
    ArtistResponseModel convertFromModelToResponseModel(ArtistModel model);
    SearchArtistModel convertFromRequestToSearchModel(SearchArtistRequest request);
    List<ISearchTerm> convertFromSearchModelToSearchTerms(SearchArtistModel? model);
    ArtistModel convertFromDatabaseModelToModel(ArtistDatabaseModel model);
}

public class ArtistAdapter : IArtistAdapter
{
    ICommonUtils _commonUtils;

    public ArtistAdapter(ICommonUtils commonUtils)
    {
        _commonUtils = commonUtils;
    }

    public ArtistResponseModel convertFromModelToResponseModel(ArtistModel model)
    {

        string dateFounded = model.DateFounded.ToString("s");
        ArtistResponseModel responseModel = new ArtistResponseModel(
            id: model.Id,
            currentLabelId: model.CurrentLabelId,
            currentLocationId: model.CurrentLocationId,
            name: model.Name,
            dateFounded: dateFounded,
            dateDisbanded: model.DateDisbanded != null ? $"{model.DateDisbanded:s}" : null,
            createdAt: model.CreatedAt.ToString("s"),
            updatedAt: model.UpdatedAt != null ? $"{model.UpdatedAt:s}" : null
        );

        return responseModel;
    }

    public ArtistModel convertFromDatabaseModelToModel(ArtistDatabaseModel model)
    { 
        ArtistModel responseModel = new ArtistModel(
            id: model.id,
            currentLabelId: model.current_label_id,
            currentLocationId: model.current_location_id,
            name: model.name,
            dateFounded: new DateOnly(model.date_founded.Year, model.date_founded.Month, model.date_founded.Day),
            dateDisbanded: model.date_disbanded != null ? new DateOnly(((DateTime)model.date_disbanded).Year, ((DateTime)model.date_disbanded).Month, ((DateTime)model.date_disbanded).Day) : null,
            createdAt: model.created_at,
            updatedAt: model.updated_at
        );

        return responseModel;
    }

    public SearchArtistModel convertFromRequestToSearchModel(SearchArtistRequest request)
    {
        SearchArtistModel result = new SearchArtistModel();

        if (request.Ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(request.Ids);
        }

        if (request.CurrentLabelIds != null)
        {
            result.CurrentLabelIds = this._commonUtils.ConvertDelimitedStringToGuidList(request.CurrentLabelIds);
        }

        if (request.CurrentLocationIds != null)
        {
            result.CurrentLocationIds = this._commonUtils.ConvertDelimitedStringToGuidList(request.CurrentLocationIds);
        }

        result.NameLike = request.NameLike;

        result.DateFoundedMin = request.DateFoundedMin;
        result.DateFoundedMax = request.DateFoundedMax;

        return result;
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(SearchArtistModel? model)
    {
        List<ISearchTerm> searchTerms = new List<ISearchTerm>();

        if (model != null)
        {
            if (model.Ids != null && model.Ids.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("id", model.Ids));
            }

            if (model.CurrentLabelIds != null && model.CurrentLabelIds.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("current_label_id", model.CurrentLabelIds));
            }

            if (model.CurrentLocationIds != null && model.CurrentLocationIds.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("current_location_id", model.CurrentLocationIds));
            }

            if (model.NameLike != null)
            {
                searchTerms.Add(new LikeSearchTerm("name", model.NameLike, LikeTypes.Like));
            }

            if (model.DateFoundedMin != null)
            {
                searchTerms.Add(new ExactMatchSearchTerm<DateOnly?>("city", model.DateFoundedMin, true));
            }

            if (model.DateFoundedMax != null)
            {
                searchTerms.Add(new ExactMatchSearchTerm<DateOnly?>("city", model.DateFoundedMax, true));
            }  
        }

        return searchTerms;
    }
}