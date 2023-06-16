namespace WebApi.Models.Labels;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class CreateLabelRequest
{
    [Required]
    public string? Name { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

public class UpdateLabelRequest
{
    public string? Name { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

[BindProperties]
public class SearchLabelRequest
{
    [FromQuery]
    public string? Ids { get; set; }

    [FromQuery]
    public string? NameLike { get; set; }

    [FromQuery]
    public string? City { get; set; }

    [FromQuery]
    public string? State { get; set; }
}

public class SearchLabelModel
{
    public List<Guid>? Ids { get; set; }

    public string? NameLike { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

public class LabelModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public LabelModel(Guid id, string name, string? city, string? state, DateTime createdAt, DateTime? updatedAt)
    {
        this.Name = name;
        this.Id = id;
        this.City = city;
        this.State = state;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class LabelDatabaseModel
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    LabelDatabaseModel(Guid id, string name, string? city, string? state, DateTime created_at, DateTime? updated_at)
    {
        this.name = name;
        this.id = id;
        this.city = city;
        this.state = state;
        this.created_at = created_at;
        this.updated_at = updated_at;
    }
}

public class LabelResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public LabelResponseModel(Guid id, string name, string? city, string? state, string createdAt, string? updatedAt)
    {
        this.Name = name;
        this.Id = id;
        this.City = city;
        this.State = state;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}