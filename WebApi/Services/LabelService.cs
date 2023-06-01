namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net; 
using WebApi.Helpers;
using WebApi.Models.Labels;
using WebApi.Repositories;

public interface ILabelService
{
    Task<IEnumerable<LabelModel>> Search(SearchLabelRequest searchModel);
    Task<LabelModel> GetById(string id);
    Task Create(CreateLabelRequest model);
    Task Update(string id, UpdateLabelRequest model);
    Task Delete(string id);
}

public class LabelService : ILabelService
{
    private ILabelAccessor _labelAccessor; 

    public LabelService(ILabelAccessor labelAccessor)
    {
        _labelAccessor = labelAccessor; 
    }

    public async Task<IEnumerable<LabelModel>> Search(SearchLabelRequest searchModel)
    {
        return await _labelAccessor.Search(searchModel);
    }

    public async Task<LabelModel> GetById(string id)
    {
        var label = await this._labelAccessor.GetById(id);

        if (label == null)
            throw new KeyNotFoundException("Label not found");

        return label;
    }

    public async Task Create(CreateLabelRequest model)
    {  
        // save label
        await this._labelAccessor.Create(model);
    }

    public async Task Update(string id, UpdateLabelRequest model)
    {
        var label = await this._labelAccessor.GetById(id);

        if (label == null)
        {
            throw new KeyNotFoundException("Label not found");
        }
         
        // save label
        await this._labelAccessor.Update(id, model);
    }

    public async Task Delete(string id)
    {
        await this._labelAccessor.Delete(id);
    }
}