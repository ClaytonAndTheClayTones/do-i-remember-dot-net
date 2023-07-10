namespace WebApi.Services;
 
using WebApi.Models.Labels;
using WebApi.Accessors;
using WebApi.Models.Common;

public interface ILabelService
{
    Task<PagedList<LabelModel>> Search(LabelSearchModel? searchModel, PagingInfo? pagingModel);
    Task<LabelModel?> GetById(Guid id);
    Task<LabelModel> Create(LabelCreateRequest model);
    Task<LabelModel?> Update(Guid id, LabelUpdateRequest model);
    Task<LabelModel?> Delete(Guid id);
}

public class LabelService : ILabelService
{
    private ILabelAccessor _labelAccessor;

    public LabelService(ILabelAccessor labelAccessor)
    {
        _labelAccessor = labelAccessor;
    }

    public async Task<PagedList<LabelModel>> Search(LabelSearchModel? searchModel, PagingInfo? pagingModel)
    {
        return await _labelAccessor.Search(searchModel, pagingModel);
    }

    public async Task<LabelModel?> GetById(Guid id)
    {
        var label = await this._labelAccessor.GetById(id);

        if (label == null)
        {
            throw new KeyNotFoundException("Label not found");
        }

        return label;
    }

    public async Task<LabelModel> Create(LabelCreateRequest model)
    {
        // save label
        return await _labelAccessor.Create(model);
    }

    public async Task<LabelModel?> Update(Guid id, LabelUpdateRequest model)
    {
        // save label
        return await this._labelAccessor.Update(id, model);
    }

    public async Task<LabelModel?> Delete(Guid id)
    {
        return await this._labelAccessor.Delete(id);
    }
}