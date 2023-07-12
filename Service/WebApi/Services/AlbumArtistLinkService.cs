namespace WebApi.Services;

using WebApi.Models.AlbumArtistLinks;
using WebApi.Accessors;
using WebApi.Models.Common;

public interface IAlbumArtistLinkService
{
    Task<PagedList<AlbumArtistLinkModel>> Search(AlbumArtistLinkSearchModel? searchModel, PagingInfo? pagingModel);
    Task<AlbumArtistLinkModel?> GetById(Guid id);
    Task<AlbumArtistLinkModel> Create(AlbumArtistLinkCreateRequest model); 
    Task<AlbumArtistLinkModel?> Delete(Guid id);
}

public class AlbumArtistLinkService : IAlbumArtistLinkService
{
    private IAlbumArtistLinkAccessor _albumArtistLinkAccessor;

    public AlbumArtistLinkService(IAlbumArtistLinkAccessor albumArtistLinkAccessor)
    {
        _albumArtistLinkAccessor = albumArtistLinkAccessor;
    }

    public async Task<PagedList<AlbumArtistLinkModel>> Search(AlbumArtistLinkSearchModel? searchModel, PagingInfo? pagingModel)
    {
        return await _albumArtistLinkAccessor.Search(searchModel, pagingModel);
    }

    public async Task<AlbumArtistLinkModel?> GetById(Guid id)
    {
        var albumArtistLink = await this._albumArtistLinkAccessor.GetById(id);

        if (albumArtistLink == null)
        {
            throw new KeyNotFoundException("AlbumArtistLink not found");
        }

        return albumArtistLink;
    }

    public async Task<AlbumArtistLinkModel> Create(AlbumArtistLinkCreateRequest model)
    {
        // save albumArtistLink
        return await _albumArtistLinkAccessor.Create(model);
    }
  
    public async Task<AlbumArtistLinkModel?> Delete(Guid id)
    {
        return await this._albumArtistLinkAccessor.Delete(id);
    }
}