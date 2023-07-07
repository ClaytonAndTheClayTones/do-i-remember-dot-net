namespace WebApi.Accessors;

using Dapper;
using WebApi.Helpers;
using WebApi.Models.Locations; 
using System.Linq;
using System.Collections.Generic;
using WebApi.Adapters.LocationAdapter;
using WebApi.Models.Common;

public interface ILocationAccessor
{
    Task<PagedList<LocationModel>> Search(SearchLocationModel? searchModel, PagingInfo? paging);
    Task<LocationModel?> GetById(Guid id);
    Task<LocationModel> Create(CreateLocationRequest Location);
    Task<LocationModel?> Update(Guid id, UpdateLocationRequest Location);
    Task<LocationModel?> Delete(Guid id);
}

public class LocationAccessor : ILocationAccessor
{
    private DataContext _context;
    private IDbUtils _dbUtils;
    private ILocationAdapter _locationAdapter;

    public LocationAccessor(DataContext context, IDbUtils dbUtils, ILocationAdapter LocationAdapter)
    {
        _context = context;
        _dbUtils = dbUtils;
        _locationAdapter = LocationAdapter;
    }

    public async Task<PagedList<LocationModel>> Search(SearchLocationModel? searchModel, PagingInfo? paging)
    {
        using var connection = _context.CreateConnection();

        List<ISearchTerm> searchTerms = this._locationAdapter.convertFromSearchModelToSearchTerms(searchModel);

        var queryPackage = this._dbUtils.BuildSelectQuery("Locations", searchTerms, paging);

        List<LocationDatabaseModel> results = (await connection.QueryAsync<LocationDatabaseModel>(queryPackage.sql, queryPackage.parameters)).ToList();

        PagedList<LocationModel> pagedList = new PagedList<LocationModel>();

        foreach (LocationDatabaseModel dbModel in results)
        {
            pagedList.Items.Add(_locationAdapter.convertFromDatabaseModelToModel(dbModel));
        }

        if (queryPackage.pagingInfoUsed != null)
        {
            long totalCount = (results.Count > 0 && results[0].full_count != null) ? (long)(results[0].full_count) : (long)0;

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

    public async Task<LocationModel?> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Locations 
            WHERE id = @id
        """;

        LocationDatabaseModel result = await connection.QuerySingleOrDefaultAsync<LocationDatabaseModel>(sql, new { id = id });
        if (result != null)
        {
            var model = this._locationAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<LocationModel> Create(CreateLocationRequest Location)
    {

        try
        {
            using var connection = _context.CreateConnection();
            var sql = """
            INSERT INTO Locations (city, state)
            VALUES (@city, @state)
            RETURNING *
        """;


            var result = await connection.QuerySingleAsync<LocationDatabaseModel>(sql, new
            { 
                city = Location.City,
                state = Location.State
            });

            var model = this._locationAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<LocationModel?> Update(Guid id, UpdateLocationRequest Location)
    {
        using var connection = _context.CreateConnection();

        UpdateQueryPackage? updateQuery = this._dbUtils.BuildUpdateQuery("Locations", id, new
        { 
            city = Location.City,
            state = Location.State
        });

        if (updateQuery == null)
        {
            return await GetById(id);
        }

        var result = await connection.QuerySingleOrDefaultAsync<LocationDatabaseModel>(updateQuery.sql, updateQuery.parameters);

        if (result != null)
        {
            var model = this._locationAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<LocationModel?> Delete(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            DELETE FROM Locations 
            WHERE Id = @id
            RETURNING *
        """;

        var result = await connection.QuerySingleOrDefaultAsync<LocationDatabaseModel>(sql, new { id });

        if (result != null)
        {
            var model = this._locationAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }
}