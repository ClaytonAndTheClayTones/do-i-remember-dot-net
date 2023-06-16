namespace WebApi.Adapters;

using System.Linq;
using WebApi.Models.Labels;
using WebApi.Helpers;

public interface ILabelAdapter
{
    LabelResponseModel convertFromModelToResponseModel(LabelModel model);
    SearchLabelModel convertFromRequestToSearchModel(SearchLabelRequest request);
    List<ISearchTerm> convertFromSearchModelToSearchTerms(SearchLabelModel? model);
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
            updatedAt: model.UpdatedAt != null ? $"{model.UpdatedAt:s}" : null
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

    public SearchLabelModel convertFromRequestToSearchModel(SearchLabelRequest request)
    {
        SearchLabelModel result = new SearchLabelModel();

        if (request.Ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(request.Ids);
        }

        result.NameLike = request.NameLike;
        result.City = request.City;
        result.State = request.State;

        return result;
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(SearchLabelModel? model)
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
                searchTerms.Add(new ExactMatchSearchTerm<string>("city", model.City, true));
            }

            if (model.State != null)
            {
                searchTerms.Add(new ExactMatchSearchTerm<string>("state", model.State, true));
            }
        }

        return searchTerms;
    }
}