namespace WebApi.Adapters;

using System.Linq;
using WebApi.Models.Labels;
using WebApi.Helpers;

public interface ILabelAdapter
{
    LabelResponseModel convertFromModelToResponseModel(LabelModel model);
    SearchLabelRequest convertFromRequestToSearchModel(string? ids, string? nameLike, string? city, string? state);
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
            state: model.State
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
}