namespace WebApi.Accessors;

using Dapper;
using WebApi.Helpers;
using WebApi.Models.Labels;
using WebApi.Adapters;
using System.Linq;
using System.Collections.Generic;
using WebApi.Adapters.LabelAdapter;
using WebApi.Models.Common;

public interface ILabelAccessor
{
    Task<PagedList<LabelModel>> Search(SearchLabelModel? searchModel, PagingInfo? paging);
    Task<LabelModel?> GetById(Guid id);
    Task<LabelModel> Create(CreateLabelRequest label);
    Task<LabelModel?> Update(Guid id, UpdateLabelRequest label);
    Task<LabelModel?> Delete(Guid id);
}

public class LabelAccessor : ILabelAccessor
{
    private DataContext _context;
    private IDbUtils _dbUtils;
    private ILabelAdapter _labelAdapter;

    public LabelAccessor(DataContext context, IDbUtils dbUtils, ILabelAdapter labelAdapter)
    {
        _context = context;
        _dbUtils = dbUtils;
        _labelAdapter = labelAdapter;
    }

    public async Task<PagedList<LabelModel>> Search(SearchLabelModel? searchModel, PagingInfo? paging)
    {
        using var connection = _context.CreateConnection();

        List<ISearchTerm> searchTerms = this._labelAdapter.convertFromSearchModelToSearchTerms(searchModel);

        var queryPackage = this._dbUtils.BuildSelectQuery("labels", searchTerms, paging);

        List<LabelDatabaseModel> results = (await connection.QueryAsync<LabelDatabaseModel>(queryPackage.sql, queryPackage.parameters)).ToList();

        PagedList<LabelModel> pagedList = new PagedList<LabelModel>();

        foreach (LabelDatabaseModel dbModel in results)
        {
            pagedList.Items.Add(_labelAdapter.convertFromDatabaseModelToModel(dbModel));
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

    public async Task<LabelModel?> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM labels 
            WHERE id = @id
        """;

        LabelDatabaseModel result = await connection.QuerySingleOrDefaultAsync<LabelDatabaseModel>(sql, new { id = id });
        if (result != null)
        {
            var model = this._labelAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<LabelModel> Create(CreateLabelRequest label)
    {

        try
        {
            using var connection = _context.CreateConnection();
            var sql = """
            INSERT INTO labels (name, city, state)
            VALUES (@name, @city, @state)
            RETURNING *
        """;


            var result = await connection.QuerySingleAsync<LabelDatabaseModel>(sql, new
            {
                name = label.Name,
                city = label.City,
                state = label.State
            });

            var model = this._labelAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<LabelModel?> Update(Guid id, UpdateLabelRequest label)
    {
        using var connection = _context.CreateConnection();

        UpdateQueryPackage? updateQuery = this._dbUtils.BuildUpdateQuery("labels", id, new
        {
            name = label.Name,
            city = label.City,
            state = label.State
        });

        if (updateQuery == null)
        {
            return await GetById(id);
        }

        var result = await connection.QuerySingleOrDefaultAsync<LabelDatabaseModel>(updateQuery.sql, updateQuery.parameters);

        if (result != null)
        {
            var model = this._labelAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }

    public async Task<LabelModel?> Delete(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            DELETE FROM labels 
            WHERE Id = @id
            RETURNING *
        """;

        var result = await connection.QuerySingleOrDefaultAsync<LabelDatabaseModel>(sql, new { id });

        if (result != null)
        {
            var model = this._labelAdapter.convertFromDatabaseModelToModel(result);

            return model;
        }
        else
        {
            return null;
        }
    }
}