namespace WebApi.Accessors;

using Dapper;
using WebApi.Helpers;
using WebApi.Models.Albums;  
using System.Linq;
using System.Collections.Generic;
using WebApi.Adapters.AlbumAdapter;
using WebApi.Models.Common;

public interface IAlbumAccessor
{
    Task<PagedList<AlbumModel>> Search(AlbumSearchModel? searchModel, PagingInfo? paging);
    Task<AlbumModel?> GetById(Guid id);
    Task<AlbumModel> Create(AlbumCreateRequest album);
    Task<AlbumModel?> Update(Guid id, AlbumUpdateRequest album);
    Task<AlbumModel?> Delete(Guid id);
}

public class AlbumAccessor : IAlbumAccessor
{
    private DataContext _context;
    private IDbUtils _dbUtils;
    private IAlbumAdapter _albumAdapter;

    public AlbumAccessor(DataContext context, IDbUtils dbUtils, IAlbumAdapter albumAdapter)
    {
        _context = context;
        _dbUtils = dbUtils;
        _albumAdapter = albumAdapter;
    }

    public async Task<PagedList<AlbumModel>> Search(AlbumSearchModel? searchModel, PagingInfo? paging)
    {
        using var connection = _context.CreateConnection();

        List<ISearchTerm> searchTerms = this._albumAdapter.convertFromSearchModelToSearchTerms(searchModel);

        var queryPackage = this._dbUtils.BuildSelectQuery("albums", searchTerms, paging);

        List<AlbumDatabaseModel> results = (await connection.QueryAsync<AlbumDatabaseModel>(queryPackage.sql, queryPackage.parameters)).ToList();

        PagedList<AlbumModel> pagedList = new PagedList<AlbumModel>();

        foreach (AlbumDatabaseModel dbModel in results)
        {
            pagedList.Items.Add(_albumAdapter.convertFromDatabaseModelToModel(dbModel));
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

    public async Task<AlbumModel?> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM albums 
            WHERE id = @id
        """;

        AlbumDatabaseModel result = await connection.QuerySingleOrDefaultAsync<AlbumDatabaseModel>(sql, new { id = id });
        if (result != null)
        {
            var model = this._albumAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<AlbumModel> Create(AlbumCreateRequest album)
    {

        try
        {
            using var connection = _context.CreateConnection();

            var insertPackage = this._dbUtils.BuildInsertQuery("albums", this._albumAdapter.convertFromCreateRequestToDatabaseModel(album));

            var result = await connection.QuerySingleAsync<AlbumDatabaseModel>(insertPackage.sql, insertPackage.parameters);

            var model = this._albumAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<AlbumModel?> Update(Guid id, AlbumUpdateRequest album)
    {
        using var connection = _context.CreateConnection();

        UpdateQueryPackage? updateQuery = this._dbUtils.BuildUpdateQuery("albums", id, this._albumAdapter.convertFromUpdateRequestToDatabaseModel(album));

        if (updateQuery == null)
        {
            return await GetById(id);
        }

        var result = await connection.QuerySingleOrDefaultAsync<AlbumDatabaseModel>(updateQuery.sql, updateQuery.parameters);

        if (result != null)
        {
            var model = this._albumAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<AlbumModel?> Delete(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            DELETE FROM albums 
            WHERE Id = @id
            RETURNING *
        """;

        var result = await connection.QuerySingleOrDefaultAsync<AlbumDatabaseModel>(sql, new { id });

        if (result != null)
        {
            var model = this._albumAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }
}