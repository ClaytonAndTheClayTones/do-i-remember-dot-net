namespace WebApi.Adapters.LabelAdapter;

using WebApi.Models.Labels;
using WebApi.Helpers;
using WebApi.Adapters.Common; 

public interface ILabelAdapter : IModelAdapter<LabelCreateRequest, LabelSearchRequest, LabelDatabaseModel, LabelModel, LabelSearchModel, LabelResponseModel>, IUpdateModelAdapter<LabelUpdateRequest> { }
 
public class LabelAdapter : ILabelAdapter
{
    ICommonUtils _commonUtils;

    public LabelAdapter(ICommonUtils commonUtils)
    {
        _commonUtils = commonUtils;
    }

    public LabelResponseModel convertFromModelToResponseModel(LabelModel model)
    {
        LabelResponseModel responseModel = new LabelResponseModel(
            id: model.Id,
            name: model.Name,
            currentLocationId: model.CurrentLocationId,
            createdAt: model.CreatedAt.ToString("s"),
            updatedAt: model.UpdatedAt != null ? $"{model.UpdatedAt:s}" : null
        );

        return responseModel;
    }

    public LabelModel convertFromDatabaseModelToModel(LabelDatabaseModel model)
    {
        LabelModel responseModel = new LabelModel(
            id: model.id,
            name: model.name,
            currentLocationId: model.current_location_id,
            createdAt: model.created_at,
            updatedAt: model.updated_at
        );

        return responseModel;
    }

    public LabelSearchModel convertFromRequestToSearchModel(LabelSearchRequest request)
    {
        LabelSearchModel result = new LabelSearchModel();

        if (request.Ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(request.Ids);
        }

        if (request.CurrentLocationIds != null)
        {
            result.CurrentLocationIds = this._commonUtils.ConvertDelimitedStringToGuidList(request.CurrentLocationIds);
        }

        result.NameLike = request.NameLike; 

        return result;
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(LabelSearchModel? model)
    {
        List<ISearchTerm> searchTerms = new List<ISearchTerm>();

        if (model != null)
        {
            if (model.Ids != null && model.Ids.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("id", model.Ids));
            }

            if (model.CurrentLocationIds != null && model.CurrentLocationIds.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("current_location_id", model.CurrentLocationIds));
            }

            if (model.NameLike != null)
            {
                searchTerms.Add(new LikeSearchTerm("name", model.NameLike, LikeTypes.Like));
            } 
        }

        return searchTerms;
    } 

    public object convertFromCreateRequestToDatabaseModel(LabelCreateRequest model)
    {
        return new
        {
            name = model.Name,
            current_location_id = model.CurrentLocationId
        };
    }

    public object convertFromUpdateRequestToDatabaseModel(LabelUpdateRequest model)
    {
        return new
        {
            name = model.Name,
            current_location_id = model.CurrentLocationId
        };
    }
}