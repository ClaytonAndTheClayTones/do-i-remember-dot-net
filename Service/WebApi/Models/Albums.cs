namespace WebApi.Models.Albums;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class AlbumCreateRequest
{
    [Required]
    public string? Name { get; set; }

    public Guid? LabelId { get; set; }

    [Required]
    public DateOnly? DateReleased { get; set; } 
}

public class AlbumUpdateRequest
{
    public string? Name { get; set; } 

    public Guid? LabelId { get; set; }

    public DateOnly? DateReleased { get; set; } 
}

[BindProperties]
public class AlbumSearchRequest
{
    [FromQuery]
    public string? Ids { get; set; }

    [FromQuery]
    public string? LabelIds { get; set; }
       
    [FromQuery]
    public string? NameLike { get; set; }

    [FromQuery]
    public DateOnly? DateReleasedMin { get; set; }

    [FromQuery]
    public DateOnly? DateReleasedMax { get; set; } 
}

public class AlbumSearchModel
{
    public List<Guid>? Ids { get; set; }
       
    public List<Guid>? LabelIds { get; set; }

    public string? NameLike { get; set; }

    public DateOnly? DateReleasedMin { get; set; }
     
    public DateOnly? DateReleasedMax { get; set; }
}

public class AlbumModel
{
    public Guid Id { get; set; }
    public Guid? LabelId { get; set; } 
    public string Name { get; set; }
    public DateOnly DateReleased { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public AlbumModel(Guid id, Guid? labelId, string name, DateOnly dateReleased, DateTime createdAt, DateTime? updatedAt)
    {
        this.Id = id;
        this.LabelId = labelId; 

        this.Name = name;
        this.DateReleased = dateReleased; 

        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class AlbumDatabaseModel
{
    public Guid id { get; set; }
    public Guid? label_id { get; set; } 
    public string name { get; set; }
    public DateTime date_released { get; set; } 
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public long? full_count { get; set; }

    public AlbumDatabaseModel(Guid id, Guid? label_id, string name, DateTime date_released, DateTime created_at, DateTime? updated_at)
    {
        this.id = id;
        this.label_id = label_id; 
        this.name = name;
        this.date_released = date_released; 
        this.created_at = created_at;
        this.updated_at = updated_at;
    }

    public AlbumDatabaseModel(Guid id, Guid? label_id, string name, DateTime date_released, DateTime created_at, DateTime? updated_at, long full_count)
        : this(id, label_id, name, date_released, created_at, updated_at)
    { 
        this.full_count = full_count;
    }

}

public class AlbumResponseModel
{
    public Guid Id { get; set; }
    public Guid? LabelId { get; set; }
    public Guid? CurrentLocationId { get; set; }
    public string Name { get; set; }
    public string DateReleased { get; set; }
    public string? DateDisbanded { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public AlbumResponseModel(Guid id, Guid? labelId, string name, string dateReleased, string createdAt, string? updatedAt)
    {
        this.Id = id;
        this.LabelId = labelId; 

        this.Name = name;
        this.DateReleased = dateReleased; 
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}