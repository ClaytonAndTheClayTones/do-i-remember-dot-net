namespace WebApi.Adapters;

using System.Linq;
using WebApi.Models.Labels;
using WebApi.Helpers;

public interface ILabelAdapter
{
    LabelResponseModel convertFromModelToResponseModel(LabelModel model);
    SearchLabelRequest convertFromRequestToSearchModel(string? ids, string? nameLike, string? city, string? state);
    List<ISearchTerm> convertFromSearchModelToSearchTerms(SearchLabelRequest? model);
    LabelModel convertFromDatabaseModelToModel(LabelDatabaseModel model);
}

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
            city: model.City,
            state: model.State,
            createdAt: model.CreatedAt.ToString("s"),
            updatedAt: $"{model.UpdatedAt:s}"
        );

        return responseModel;
    }


    public LabelModel convertFromDatabaseModelToModel(LabelDatabaseModel model)
    {
        LabelModel responseModel = new LabelModel(
            id: model.id,
            name: model.name,
            city: model.city,
            state: model.state,
            createdAt: model.created_at,
            updatedAt: model.updated_at
        );

        return responseModel;
    }

    public SearchLabelRequest convertFromRequestToSearchModel(string? ids, string? nameLike, string? city, string? state)
    {
        SearchLabelRequest result = new SearchLabelRequest();

        if (ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(ids);
        }

        result.NameLike = nameLike;
        result.City = city;
        result.State = state;

        return result;
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(SearchLabelRequest? model)
    {
        List<ISearchTerm> searchTerms = new List<ISearchTerm>();

        if (model != null)
        {
            if (model.Ids != null && model.Ids.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("id", model.Ids));
            }

            if (model.NameLike != null)
            {
                searchTerms.Add(new LikeSearchTerm("name", model.NameLike, LikeTypes.Like));
            }

            if (model.City != null)
            {
                searchTerms.Add(new ExactMatchSearchTerm<string>("city", model.City));
            }

            if (model.State != null)
            {
                searchTerms.Add(new ExactMatchSearchTerm<string>("state", model.State));
            }
        }

        return searchTerms;
    }


}