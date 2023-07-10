namespace WebApi.Accessors;

using Dapper;
using WebApi.Helpers;
using WebApi.Models.AlbumArtistLinks;  
using System.Linq;
using System.Collections.Generic;
using WebApi.Adapters.AlbumArtistLinkAdapter;
using WebApi.Models.Common;

public interface IAlbumArtistLinkAccessor
{
    Task<PagedList<AlbumArtistLinkModel>> Search(AlbumArtistLinkSearchModel? searchModel, PagingInfo? paging);
    Task<AlbumArtistLinkModel?> GetById(Guid id);
    Task<AlbumArtistLinkModel> Create(AlbumArtistLinkCreateRequest albumAlbumArtistLinkLink);
    Task<AlbumArtistLinkModel?> Update(Guid id, AlbumArtistLinkUpdateRequest albumAlbumArtistLinkLink);
    Task<AlbumArtistLinkModel?> Delete(Guid id);
}

public class AlbumArtistLinkAccessor : IAlbumArtistLinkAccessor
{
    private DataContext _context;
    private IDbUtils _dbUtils;
    private IAlbumArtistLinkAdapter _albumAlbumArtistLinkLinkAdapter;

    public AlbumArtistLinkAccessor(DataContext context, IDbUtils dbUtils, IAlbumArtistLinkAdapter albumAlbumArtistLinkLinkAdapter)
    {
        _context = context;
        _dbUtils = dbUtils;
        _albumAlbumArtistLinkLinkAdapter = albumAlbumArtistLinkLinkAdapter;
    }

    public async Task<PagedList<AlbumArtistLinkModel>> Search(AlbumArtistLinkSearchModel? searchModel, PagingInfo? paging)
    {
        using var connection = _context.CreateConnection();

        List<ISearchTerm> searchTerms = this._albumAlbumArtistLinkLinkAdapter.convertFromSearchModelToSearchTerms(searchModel);

        var queryPackage = this._dbUtils.BuildSelectQuery("album_artist_links", searchTerms, paging);

        List<AlbumArtistLinkDatabaseModel> results = (await connection.QueryAsync<AlbumArtistLinkDatabaseModel>(queryPackage.sql, queryPackage.parameters)).ToList();

        PagedList<AlbumArtistLinkModel> pagedList = new PagedList<AlbumArtistLinkModel>();

        foreach (AlbumArtistLinkDatabaseModel dbModel in results)
        {
            pagedList.Items.Add(_albumAlbumArtistLinkLinkAdapter.convertFromDatabaseModelToModel(dbModel));
        }

        if (queryPackage.pagingInfoUsed != null)
        {
            long totalCount = (results.Count > 0 && results[0].full_count != null) ? (long)results[0].full_count : 0;

            PagingResultInfo pagingResultInfo = new PagingResultInfo()
            {
                Page = queryPackage.pagingInfoUsed.Page ?? 0,
                PageLength = queryPackage.pagingInfoUsed.PageLength ?? 0,
                SortBy = queryPackage.pagingInfoUsed.SortBy,
                IsDescending = queryPackage.pagingInfoUsed.IsDescending,
                TotalCount = totalCount
            };

            pagedList.PagingInfo = pagingResultInfo;
        }

        return pagedList;
    }

    public async Task<AlbumArtistLinkModel?> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM album_artist_links 
            WHERE id = @id
        """;

        AlbumArtistLinkDatabaseModel result = await connection.QuerySingleOrDefaultAsync<AlbumArtistLinkDatabaseModel>(sql, new { id = id });
        if (result != null)
        {
            var model = this._albumAlbumArtistLinkLinkAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<AlbumArtistLinkModel> Create(AlbumArtistLinkCreateRequest albumAlbumArtistLinkLink)
    {

        try
        {
            using var connection = _context.CreateConnection();

            var insertPackage = this._dbUtils.BuildInsertQuery("album_artist_links", this._albumAlbumArtistLinkLinkAdapter.convertFromCreateRequestToDatabaseModel(albumAlbumArtistLinkLink));

            var result = await connection.QuerySingleAsync<AlbumArtistLinkDatabaseModel>(insertPackage.sql, insertPackage.parameters);

            var model = this._albumAlbumArtistLinkLinkAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<AlbumArtistLinkModel?> Update(Guid id, AlbumArtistLinkUpdateRequest albumAlbumArtistLinkLink)
    {
        using var connection = _context.CreateConnection();

        UpdateQueryPackage? updateQuery = this._dbUtils.BuildUpdateQuery("album_artist_links", id, this._albumAlbumArtistLinkLinkAdapter.convertFromUpdateRequestToDatabaseModel(albumAlbumArtistLinkLink));

        if (updateQuery == null)
        {
            return await GetById(id);
        }

        var result = await connection.QuerySingleOrDefaultAsync<AlbumArtistLinkDatabaseModel>(updateQuery.sql, updateQuery.parameters);

        if (result != null)
        {
            var model = this._albumAlbumArtistLinkLinkAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<AlbumArtistLinkModel?> Delete(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            DELETE FROM album_artist_links 
            WHERE Id = @id
            RETURNING *
        """;

        var result = await connection.QuerySingleOrDefaultAsync<AlbumArtistLinkDatabaseModel>(sql, new { id });

        if (result != null)
        {
            var model = this._albumAlbumArtistLinkLinkAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }
}