namespace WebApi.Models.Labels;
 
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc; 

public class LabelCreateRequest
{
    public Guid? CurrentLocationId { get; set; }

    [Required]
    public string? Name { get; set; } 
}

public class LabelUpdateRequest
{
    public string? Name { get; set; }
     
    public Guid? CurrentLocationId { get; set; }
}

[BindProperties]
public class LabelSearchRequest
{
    [FromQuery]
    public string? Ids { get; set; }
     
    [FromQuery]
    public string? CurrentLocationIds { get; set; }

    [FromQuery]
    public string? NameLike { get; set; }
     
}

public class LabelSearchModel
{
    public List<Guid>? Ids { get; set; }

    public List<Guid>? CurrentLocationIds { get; set; }

    public string? NameLike { get; set; }
}

public class LabelModel
{
    public Guid Id { get; set; }

    public Guid? CurrentLocationId { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public LabelModel(Guid id, Guid? currentLocationId, string name, DateTime createdAt, DateTime? updatedAt)
    {
        this.Id = id;

        this.CurrentLocationId = currentLocationId;

        this.Name = name;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class LabelDatabaseModel
{
    public Guid id { get; set; }

    public Guid? current_location_id { get; set; }

    public string name { get; set; }

    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public long? full_count { get; set; }

    public LabelDatabaseModel(Guid id, Guid? current_location_id, string name, DateTime created_at, DateTime? updated_at)
    {
        this.name = name;
        this.id = id;
        this.current_location_id = current_location_id;
        this.created_at = created_at;
        this.updated_at = updated_at;
    }


    public LabelDatabaseModel(Guid id, Guid? current_location_id, string name, DateTime created_at, DateTime? updated_at, long? full_count) :
          this(id, current_location_id,  name, created_at,  updated_at)
    { 
        this.full_count = full_count;
    }
}

public class LabelResponseModel
{
    public Guid Id { get; set; }
    public Guid? CurrentLocationId { get; set; }
    public string Name { get; set; } 
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public LabelResponseModel(Guid id, Guid? currentLocationId, string name,  string createdAt, string? updatedAt)
    {
        this.Id = id;
        this.CurrentLocationId = currentLocationId;
        this.Name = name;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}