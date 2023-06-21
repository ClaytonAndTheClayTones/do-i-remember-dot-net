namespace WebApi.Models.Artists;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Common;

public class CreateArtistRequest
{
    [Required]
    public string? Name { get; set; }

    public string? CurrentCity { get; set; }

    public string? CurrentState { get; set; }

    [Required]
    public string? DateFounded { get; set; }

    [Required]
    public string? DateFounded { get; set; }
}

public class UpdateArtistRequest
{
    public string? Name { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

[BindProperties]
public class SearchArtistRequest
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

public class SearchArtistModel
{
    public List<Guid>? Ids { get; set; }

    public string? NameLike { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

public class ArtistModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ArtistModel(Guid id, string name, string? city, string? state, DateTime createdAt, DateTime? updatedAt)
    {
        this.Name = name;
        this.Id = id;
        this.City = city;
        this.State = state;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class ArtistDatabaseModel
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public long? full_count { get; set; }

    ArtistDatabaseModel(Guid id, string name, string? city, string? state, DateTime created_at, DateTime? updated_at)
    {
        this.name = name;
        this.id = id;
        this.city = city;
        this.state = state;
        this.created_at = created_at;
        this.updated_at = updated_at;
    }


    ArtistDatabaseModel(Guid id, string name, string? city, string? state, DateTime created_at, DateTime? updated_at, long? full_count)
    {
        this.name = name;
        this.id = id;
        this.city = city;
        this.state = state;
        this.created_at = created_at;
        this.updated_at = updated_at;
        this.full_count = full_count;
    }
}

public class ArtistResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public ArtistResponseModel(Guid id, string name, string? city, string? state, string createdAt, string? updatedAt)
    {
        this.Name = name;
        this.Id = id;
        this.City = city;
        this.State = state;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}