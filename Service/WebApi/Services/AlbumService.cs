namespace WebApi.Services;

using WebApi.Models.Albums;
using WebApi.Accessors;
using WebApi.Models.Common;

public interface IAlbumService
{
    Task<PagedList<AlbumModel>> Search(AlbumSearchModel? searchModel, PagingInfo? pagingModel);
    Task<AlbumModel?> GetById(Guid id);
    Task<AlbumModel> Create(AlbumCreateRequest model);
    Task<AlbumModel?> Update(Guid id, AlbumUpdateRequest model);
    Task<AlbumModel?> Delete(Guid id);
}

public class AlbumService : IAlbumService
{
    private IAlbumAccessor _albumAccessor;

    public AlbumService(IAlbumAccessor albumAccessor)
    {
        _albumAccessor = albumAccessor;
    }

    public async Task<PagedList<AlbumModel>> Search(AlbumSearchModel? searchModel, PagingInfo? pagingModel)
    {
        return await _albumAccessor.Search(searchModel, pagingModel);
    }

    public async Task<AlbumModel?> GetById(Guid id)
    {
        var album = await this._albumAccessor.GetById(id);

        if (album == null)
        {
            throw new KeyNotFoundException("Album not found");
        }

        return album;
    }

    public async Task<AlbumModel> Create(AlbumCreateRequest model)
    {
        // save album
        return await _albumAccessor.Create(model);
    }

    public async Task<AlbumModel?> Update(Guid id, AlbumUpdateRequest model)
    {
        // save album
        return await this._albumAccessor.Update(id, model);
    }

    public async Task<AlbumModel?> Delete(Guid id)
    {
        return await this._albumAccessor.Delete(id);
    }
}