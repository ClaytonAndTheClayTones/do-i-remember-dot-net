namespace WebApi.Services;
 
using WebApi.Models.Locations;
using WebApi.Accessors;
using WebApi.Models.Common;

public interface ILocationService
{
    Task<PagedList<LocationModel>> Search(SearchLocationModel? searchModel, PagingInfo? pagingModel);
    Task<LocationModel?> GetById(Guid id);
    Task<LocationModel> Create(CreateLocationRequest model);
    Task<LocationModel?> Update(Guid id, UpdateLocationRequest model);
    Task<LocationModel?> Delete(Guid id);
}

public class LocationService : ILocationService
{
    private ILocationAccessor _locationAccessor;

    public LocationService(ILocationAccessor locationAccessor)
    {
        _locationAccessor = locationAccessor;
    }

    public async Task<PagedList<LocationModel>> Search(SearchLocationModel? searchModel, PagingInfo? pagingModel)
    {
        return await _locationAccessor.Search(searchModel, pagingModel);
    }

    public async Task<LocationModel?> GetById(Guid id)
    {
        var location = await this._locationAccessor.GetById(id);

        if (location == null)
        {
            throw new KeyNotFoundException("Location not found");
        }

        return location;
    }

    public async Task<LocationModel> Create(CreateLocationRequest model)
    {
        // save location
        return await _locationAccessor.Create(model);
    }

    public async Task<LocationModel?> Update(Guid id, UpdateLocationRequest model)
    {
        // save location
        return await this._locationAccessor.Update(id, model);
    }

    public async Task<LocationModel?> Delete(Guid id)
    {
        return await this._locationAccessor.Delete(id);
    }
}