namespace WebApi.Services;

using WebApi.Models.Artists;
using WebApi.Accessors;
using WebApi.Models.Common;

public interface IArtistService
{
    Task<PagedList<ArtistModel>> Search(ArtistSearchModel? searchModel, PagingInfo? pagingModel);
    Task<ArtistModel?> GetById(Guid id);
    Task<ArtistModel> Create(ArtistCreateRequest model);
    Task<ArtistModel?> Update(Guid id, ArtistUpdateRequest model);
    Task<ArtistModel?> Delete(Guid id);
}

public class ArtistService : IArtistService
{
    private IArtistAccessor _artistAccessor;

    public ArtistService(IArtistAccessor artistAccessor)
    {
        _artistAccessor = artistAccessor;
    }

    public async Task<PagedList<ArtistModel>> Search(ArtistSearchModel? searchModel, PagingInfo? pagingModel)
    {
        return await _artistAccessor.Search(searchModel, pagingModel);
    }

    public async Task<ArtistModel?> GetById(Guid id)
    {
        var artist = await this._artistAccessor.GetById(id);

        if (artist == null)
        {
            throw new KeyNotFoundException("Artist not found");
        }

        return artist;
    }

    public async Task<ArtistModel> Create(ArtistCreateRequest model)
    {
        // save artist
        return await _artistAccessor.Create(model);
    }

    public async Task<ArtistModel?> Update(Guid id, ArtistUpdateRequest model)
    {
        // save artist
        return await this._artistAccessor.Update(id, model);
    }

    public async Task<ArtistModel?> Delete(Guid id)
    {
        return await this._artistAccessor.Delete(id);
    }
}