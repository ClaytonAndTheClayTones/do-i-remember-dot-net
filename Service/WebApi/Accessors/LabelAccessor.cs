namespace WebApi.Accessors;

using Dapper;
using WebApi.Helpers;
using WebApi.Models.Labels;
using WebApi.Adapters;
using System.Linq;
using System.Collections.Generic;

public interface ILabelAccessor
{
    Task<IEnumerable<LabelModel>> Search(SearchLabelRequest? searchModel);
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

    public async Task<IEnumerable<LabelModel>> Search(SearchLabelRequest? searchModel)
    {
        using var connection = _context.CreateConnection();

        List<ISearchTerm> searchTerms = this._labelAdapter.convertFromSearchModelToSearchTerms(searchModel);

        var queryPackage = this._dbUtils.BuildSelectQuery("labels", searchTerms);

        var results = await connection.QueryAsync<LabelDatabaseModel>(queryPackage.sql, queryPackage.parameters);

        List<LabelModel> labelModels = new List<LabelModel>();

        foreach (LabelDatabaseModel dbModel in results)
        {
            labelModels.Add(_labelAdapter.convertFromDatabaseModelToModel(dbModel));
        }

        return labelModels;
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