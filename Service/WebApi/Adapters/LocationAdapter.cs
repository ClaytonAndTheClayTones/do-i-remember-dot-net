namespace WebApi.Adapters.LocationAdapter;

using WebApi.Models.Locations;
using WebApi.Helpers;
using WebApi.Adapters.Common;

public interface ILocationAdapter : IModelAdapter<LocationCreateRequest, LocationUpdateRequest, LocationSearchRequest, LocationDatabaseModel, LocationModel, LocationSearchModel, LocationResponseModel> { }

public class LocationAdapter : ILocationAdapter
{
    ICommonUtils _commonUtils;

    public LocationAdapter(ICommonUtils commonUtils)
    {
        _commonUtils = commonUtils;
    }

    public LocationResponseModel convertFromModelToResponseModel(LocationModel model)
    {
        LocationResponseModel responseModel = new LocationResponseModel(
            id: model.Id, 
            city: model.City,
            state: model.State,
            createdAt: model.CreatedAt.ToString("s"),
            updatedAt: model.UpdatedAt != null ? $"{model.UpdatedAt:s}" : null
        );

        return responseModel;
    }

    public LocationModel convertFromDatabaseModelToModel(LocationDatabaseModel model)
    {
        LocationModel responseModel = new LocationModel(
            id: model.id, 
            city: model.city,
            state: model.state,
            createdAt: model.created_at,
            updatedAt: model.updated_at
        );

        return responseModel;
    }


    public object convertFromCreateRequestToDatabaseModel(LocationCreateRequest model)
    {
        return new
        {
            city = model.City,
            state = model.State
        };
    }

    public object convertFromUpdateRequestToDatabaseModel(LocationUpdateRequest model)
    {
        return new
        {
            city = model.City,
            state = model.State
        };
    }

    public LocationSearchModel convertFromRequestToSearchModel(LocationSearchRequest request)
    {
        LocationSearchModel result = new LocationSearchModel();

        if (request.Ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(request.Ids);
        }
         
        result.CityOrStateLike = request.CityOrStateLike;
  
        return result;
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(LocationSearchModel? model)
    {
        List<ISearchTerm> searchTerms = new List<ISearchTerm>();

        if (model != null)
        {
            if (model.Ids != null && model.Ids.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("id", model.Ids));
            }

            if(model.CityOrStateLike != null)
            {
                var list = new List<ComparisonSearchTermInput>()
                {
                    new ComparisonSearchTermInput("city", model.CityOrStateLike, LikeTypes.Like, true),
                    new ComparisonSearchTermInput("state", model.CityOrStateLike, LikeTypes.Like, true)
                };

                searchTerms.Add(new ComparisonSearchTerm(list));
            } 
        }

        return searchTerms;
    }
}