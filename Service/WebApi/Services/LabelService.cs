namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Helpers;
using WebApi.Models.Labels;
using WebApi.Accessors;

public interface ILabelService
{
    Task<IEnumerable<LabelModel>> Search(SearchLabelModel? searchModel);
    Task<LabelModel?> GetById(Guid id);
    Task<LabelModel> Create(CreateLabelRequest model);
    Task<LabelModel?> Update(Guid id, UpdateLabelRequest model);
    Task<LabelModel?> Delete(Guid id);
}

public class LabelService : ILabelService
{
    private ILabelAccessor _labelAccessor;

    public LabelService(ILabelAccessor labelAccessor)
    {
        _labelAccessor = labelAccessor;
    }

    public async Task<IEnumerable<LabelModel>> Search(SearchLabelModel? searchModel)
    {
        return await _labelAccessor.Search(searchModel);
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

    public async Task<LabelModel> Create(CreateLabelRequest model)
    {
        // save label
        return await _labelAccessor.Create(model);
    }

    public async Task<LabelModel?> Update(Guid id, UpdateLabelRequest model)
    {
        // save label
        return await this._labelAccessor.Update(id, model);
    }

    public async Task<LabelModel?> Delete(Guid id)
    {
        return await this._labelAccessor.Delete(id);
    }
}