namespace WebApi.Accessors;

using Dapper;
using WebApi.Helpers;
using WebApi.Models.Artists;  
using System.Linq;
using System.Collections.Generic;
using WebApi.Adapters.ArtistAdapter;
using WebApi.Models.Common;

public interface IArtistAccessor
{
    Task<PagedList<ArtistModel>> Search(ArtistSearchModel? searchModel, PagingInfo? paging);
    Task<ArtistModel?> GetById(Guid id);
    Task<ArtistModel> Create(ArtistCreateRequest artist);
    Task<ArtistModel?> Update(Guid id, ArtistUpdateRequest artist);
    Task<ArtistModel?> Delete(Guid id);
}

public class ArtistAccessor : IArtistAccessor
{
    private DataContext _context;
    private IDbUtils _dbUtils;
    private IArtistAdapter _artistAdapter;

    public ArtistAccessor(DataContext context, IDbUtils dbUtils, IArtistAdapter artistAdapter)
    {
        _context = context;
        _dbUtils = dbUtils;
        _artistAdapter = artistAdapter;
    }

    public async Task<PagedList<ArtistModel>> Search(ArtistSearchModel? searchModel, PagingInfo? paging)
    {
        using var connection = _context.CreateConnection();

        List<ISearchTerm> searchTerms = this._artistAdapter.convertFromSearchModelToSearchTerms(searchModel);

        var queryPackage = this._dbUtils.BuildSelectQuery("artists", searchTerms, paging);

        List<ArtistDatabaseModel> results = (await connection.QueryAsync<ArtistDatabaseModel>(queryPackage.sql, queryPackage.parameters)).ToList();

        PagedList<ArtistModel> pagedList = new PagedList<ArtistModel>();

        foreach (ArtistDatabaseModel dbModel in results)
        {
            pagedList.Items.Add(_artistAdapter.convertFromDatabaseModelToModel(dbModel));
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

    public async Task<ArtistModel?> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM artists 
            WHERE id = @id
        """;

        ArtistDatabaseModel result = await connection.QuerySingleOrDefaultAsync<ArtistDatabaseModel>(sql, new { id = id });
        if (result != null)
        {
            var model = this._artistAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<ArtistModel> Create(ArtistCreateRequest artist)
    {

        try
        {
            using var connection = _context.CreateConnection();

            var insertPackage = this._dbUtils.BuildInsertQuery("artists", this._artistAdapter.convertFromCreateRequestToDatabaseModel(artist));

            var result = await connection.QuerySingleAsync<ArtistDatabaseModel>(insertPackage.sql, insertPackage.parameters);

            var model = this._artistAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<ArtistModel?> Update(Guid id, ArtistUpdateRequest artist)
    {
        using var connection = _context.CreateConnection();

        UpdateQueryPackage? updateQuery = this._dbUtils.BuildUpdateQuery("artists", id, this._artistAdapter.convertFromUpdateRequestToDatabaseModel(artist));

        if (updateQuery == null)
        {
            return await GetById(id);
        }

        var result = await connection.QuerySingleOrDefaultAsync<ArtistDatabaseModel>(updateQuery.sql, updateQuery.parameters);

        if (result != null)
        {
            var model = this._artistAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<ArtistModel?> Delete(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            DELETE FROM artists 
            WHERE Id = @id
            RETURNING *
        """;

        var result = await connection.QuerySingleOrDefaultAsync<ArtistDatabaseModel>(sql, new { id });

        if (result != null)
        {
            var model = this._artistAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }
}