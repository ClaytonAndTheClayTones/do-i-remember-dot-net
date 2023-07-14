namespace WebApi.Adapters.AlbumAdapter;

using WebApi.Models.Albums;
using WebApi.Helpers;
using WebApi.Adapters.Common;

public interface IAlbumAdapter : IModelAdapter<AlbumCreateRequest, AlbumCreateModel, AlbumSearchRequest, AlbumDatabaseModel, AlbumModel, AlbumSearchModel, AlbumResponseModel>, IUpdateModelAdapter<AlbumUpdateRequest, AlbumUpdateModel> { }
  
public class AlbumAdapter : IAlbumAdapter
{
    ICommonUtils _commonUtils;

    public AlbumAdapter(ICommonUtils commonUtils)
    {
        _commonUtils = commonUtils;
    }

    public AlbumCreateModel convertFromCreateRequestToCreateModel(AlbumCreateRequest model)
    {
        return new AlbumCreateModel()
        {
            LabelId = model.LabelId != null ? Guid.Parse(model.LabelId) : null,

            Name = model.Name,
            DateReleased = model.DateReleased != null ? DateOnly.Parse(model.DateReleased) : null
        };
    }

    public AlbumUpdateModel convertFromUpdateRequestToUpdateModel(AlbumUpdateRequest model)
    {
        return new AlbumUpdateModel()
        {
            LabelId = model.LabelId != null ? Guid.Parse(model.LabelId) : null,

            Name = model.Name,
            DateReleased = model.DateReleased != null ? DateOnly.Parse(model.DateReleased) : null
        };
    } 

    public AlbumSearchModel convertFromSearchRequestToSearchModel(AlbumSearchRequest request)
    {
        AlbumSearchModel result = new AlbumSearchModel();

        if (request.Ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(request.Ids);
        }

        if (request.LabelIds != null)
        {
            result.LabelIds = this._commonUtils.ConvertDelimitedStringToGuidList(request.LabelIds);
        }

        result.NameLike = request.NameLike;

        result.DateReleasedMin = request.DateReleasedMin;
        result.DateReleasedMax = request.DateReleasedMax;

        return result;
    }
     
    public object convertFromCreateModelToDatabaseModel(AlbumCreateModel model)
    {
        return new
        {
            label_id = model.LabelId,
            name = model.Name,
            date_released = model.DateReleased != null ? ((DateOnly)model.DateReleased).ToDateTime(TimeOnly.MinValue) : new DateTime(),
        };
    }

    public object convertFromUpdateModelToDatabaseModel(AlbumUpdateModel model)
    {
        return new
        {
            label_id = model.LabelId,
            name = model.Name,
            date_released = model.DateReleased != null ? ((DateOnly)model.DateReleased).ToDateTime(TimeOnly.MinValue) : new DateTime(),
        };
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(AlbumSearchModel? model)
    {
        List<ISearchTerm> searchTerms = new List<ISearchTerm>();

        if (model != null)
        {
            if (model.Ids != null && model.Ids.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("id", model.Ids));
            }

            if (model.LabelIds != null && model.LabelIds.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("label_id", model.LabelIds));
            }
 
            if (model.NameLike != null)
            {
                searchTerms.Add(new LikeSearchTerm("name", model.NameLike, LikeTypes.Like));
            }
             
            if (model.DateReleasedMin != null || model.DateReleasedMax != null)
            {
                searchTerms.Add(new DateRangeSearchTerm(
                    "date_released",
                    model.DateReleasedMin == null ? null : ((DateOnly)model.DateReleasedMin).ToDateTime(TimeOnly.MinValue),
                    model.DateReleasedMax == null ? null : ((DateOnly)model.DateReleasedMax).ToDateTime(TimeOnly.MinValue))); 
            }
        }

        return searchTerms;
    }

    public AlbumModel convertFromDatabaseModelToModel(AlbumDatabaseModel model)
    {
        AlbumModel responseModel = new AlbumModel(
            id: model.id,
            labelId: model.label_id,
            name: model.name,
            dateReleased: new DateOnly(model.date_released.Year, model.date_released.Month, model.date_released.Day),
            createdAt: model.created_at,
            updatedAt: model.updated_at
        );

        return responseModel;
    }

    public AlbumResponseModel convertFromModelToResponseModel(AlbumModel model)
    {
        AlbumResponseModel responseModel = new AlbumResponseModel(
            id: model.Id,
            labelId: model.LabelId,
            name: model.Name,
            dateReleased: model.DateReleased.ToString("O"),
            createdAt: model.CreatedAt.ToString("s"),
            updatedAt: model.UpdatedAt != null ? $"{model.UpdatedAt:s}" : null
        );

        return responseModel;
    }
 
}