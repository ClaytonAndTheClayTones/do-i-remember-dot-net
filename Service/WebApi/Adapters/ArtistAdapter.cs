namespace WebApi.Adapters.ArtistAdapter;

using WebApi.Models.Artists;
using WebApi.Helpers;
using WebApi.Adapters.Common;

public interface IArtistAdapter : IModelAdapter<ArtistCreateRequest, ArtistSearchRequest, ArtistDatabaseModel, ArtistModel, ArtistSearchModel, ArtistResponseModel>, IUpdateModelAdapter<ArtistUpdateRequest> { }
  
public class ArtistAdapter : IArtistAdapter
{
    ICommonUtils _commonUtils;

    public ArtistAdapter(ICommonUtils commonUtils)
    {
        _commonUtils = commonUtils;
    }

    public ArtistResponseModel convertFromModelToResponseModel(ArtistModel model)
    {  
        ArtistResponseModel responseModel = new ArtistResponseModel(
            id: model.Id,
            currentLabelId: model.CurrentLabelId,
            currentLocationId: model.CurrentLocationId,
            name: model.Name,
            dateFounded: model.DateFounded.ToString("O"),
            dateDisbanded: model.DateDisbanded != null ? $"{model.DateDisbanded:O}" : null,
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

    public ArtistSearchModel convertFromRequestToSearchModel(ArtistSearchRequest request)
    {
        ArtistSearchModel result = new ArtistSearchModel();

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

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(ArtistSearchModel? model)
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

            if (model.DateFoundedMin != null || model.DateFoundedMax != null)
            {
                searchTerms.Add(new DateRangeSearchTerm(
                    "date_founded",
                    model.DateFoundedMin == null ? null : ((DateOnly)model.DateFoundedMin).ToDateTime(TimeOnly.MinValue),
                    model.DateFoundedMax == null ? null : ((DateOnly)model.DateFoundedMax).ToDateTime(TimeOnly.MinValue)));

            }
         }

        return searchTerms;
    }

    public object convertFromCreateRequestToDatabaseModel(ArtistCreateRequest model)
    {
        return new
        {
            current_label_id = model.CurrentLabelId,
            current_location_id = model.CurrentLocationId,
            name = model.Name,
            date_founded = model.DateFounded != null ? ((DateOnly)model.DateFounded).ToDateTime(TimeOnly.MinValue) : new DateTime(),
            date_disbanded = model.DateDisbanded != null ? (DateTime?)((DateOnly)model.DateDisbanded).ToDateTime(TimeOnly.MinValue) : null
        };
    }

    public object convertFromUpdateRequestToDatabaseModel(ArtistUpdateRequest model)
    {
        return new
        {
            current_label_id = model.CurrentLabelId,
            current_location_id = model.CurrentLocationId,
            name = model.Name,
            date_founded = model.DateFounded != null ? ((DateOnly)model.DateFounded).ToDateTime(TimeOnly.MinValue) : new DateTime(),
            date_disbanded = model.DateDisbanded != null ? (DateTime?)((DateOnly)model.DateDisbanded).ToDateTime(TimeOnly.MinValue) : null
        };
    }
}