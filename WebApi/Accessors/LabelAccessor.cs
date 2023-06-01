namespace WebApi.Repositories;

using Dapper; 
using WebApi.Helpers;
using WebApi.Models.Labels;

public interface ILabelAccessor
{
    Task<IEnumerable<LabelModel>> Search(SearchLabelRequest searchModel);
    Task<LabelModel> GetById(string id); 
    Task Create(CreateLabelRequest label);
    Task Update(string id, UpdateLabelRequest label);
    Task Delete(string id);
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

    public async Task<IEnumerable<LabelModel>> Search(SearchLabelRequest searchModel)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM labels
        """;
        return await connection.QueryAsync<LabelModel>(sql);
    }

    public async Task<LabelModel> GetById(string id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM labels 
            WHERE id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<LabelModel>(sql, new { id = id });
    }
  
    public async Task Create(CreateLabelRequest label)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO labels (name, city, state)
            VALUES (@name, @city, @state)
        """;
        
        await connection.ExecuteAsync(sql, new {
            name = label.Name, city = label.City, state = label.State
        });
    }

    public async Task Update(string id, UpdateLabelRequest label)
    {
        using var connection = _context.CreateConnection();

        Dictionary<string, object?> queryDict = new Dictionary<string, object?>()
        {
            { "name", label.Name },
            { "city", label.City },
            { "state", label.State }
        };
         
        this._dbUtils.BuildUpdateQuery("labels", queryDict);

        var sql = $"""
            UPDATE labels 
            SET {(label.Name != null ? "name = @name" : "")},
                {(label.City != null ? "city = @city" : "")},
                {(label.State != null ? "state = @state" : "")} 
            WHERE id = @id
        """;
         
        await connection.ExecuteAsync(sql, new
        {
            id=id,
            name = label.Name 
        });
    }

    public async Task Delete(string id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM labels 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}