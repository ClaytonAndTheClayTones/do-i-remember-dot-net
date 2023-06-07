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
    [FromQuery(Name = "ids")]
    public List<Guid>? Ids { get; set; }

    [FromQuery(Name = "nameLike")]
    public string? NameLike { get; set; }

    [FromQuery(Name = "city")]
    public string? City { get; set; }

    [FromQuery(Name = "state")]
    public string? State { get; set; }
}

public class LabelModel 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }

    LabelModel(Guid id, string name, string? city, string? state)
    {
        this.Name = name;
        this.Id = id;
        this.City = city;
        this.State = state;
    }
}

public class LabelResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }

    public LabelResponseModel(Guid id, string name, string? city, string? state)
    {
        this.Name = name;
        this.Id = id;
        this.City = city;
        this.State = state;
    }
}