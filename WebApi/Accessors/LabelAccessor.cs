namespace WebApi.Accessors;

using Dapper; 
using WebApi.Helpers;
using WebApi.Models.Labels;

public interface ILabelAccessor
{
    Task<IEnumerable<LabelModel>> Search(SearchLabelRequest? searchModel);
    Task<LabelModel> GetById(Guid id); 
    Task<LabelModel> Create(CreateLabelRequest label);
    Task<LabelModel> Update(Guid id, UpdateLabelRequest label);
    Task<LabelModel> Delete(Guid id);
}

public class LabelAccessor : ILabelAccessor
{
    private DataContext _context;
    private IDbUtils _dbUtils;

    public LabelAccessor(DataContext context, IDbUtils dbUtils)
    { 
        _context = context;
        _dbUtils = dbUtils; 
    }

    public async Task<IEnumerable<LabelModel>> Search(SearchLabelRequest? searchModel)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM labels;
        """;
        return await connection.QueryAsync<LabelModel>(sql);
    }

    public async Task<LabelModel> GetById(Guid id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM labels 
            WHERE id = @id
        """;

        LabelModel result =  await connection.QuerySingleOrDefaultAsync<LabelModel>(sql, new { id = id });

        return result;
    }
  
    public async Task<LabelModel> Create(CreateLabelRequest label)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO labels (name, city, state)
            VALUES (@name, @city, @state)
            RETURNING *
        """;
        
        var result = await connection.QuerySingleAsync<LabelModel>(sql, new {
            name = label.Name, city = label.City, state = label.State
        });

        return result;
    }

    public async Task<LabelModel> Update(Guid id, UpdateLabelRequest label)
    {
        using var connection = _context.CreateConnection();
         
        UpdateQueryPackage? updateQuery = this._dbUtils.BuildUpdateQuery("labels", id, new
        {
            name = label.Name,
            city = label.City,
            state = label.State
        });

        if(updateQuery == null)
        {
            return await GetById(id);
        }
          
        var result = await connection.QuerySingleOrDefaultAsync<LabelModel>(updateQuery.sql, updateQuery.parameters);

        return result;
    }

    public async Task<LabelModel> Delete(Guid id)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            DELETE FROM labels 
            WHERE Id = @id
            RETURNING *
        """;

        var result = await connection.QuerySingleOrDefaultAsync<LabelModel>(sql, new { id });

        return result;
    }
}