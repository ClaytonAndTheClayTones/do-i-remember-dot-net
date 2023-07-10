namespace WebApi.Models.Artists;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class ArtistCreateRequest
{
    [Required]
    public string? Name { get; set; }

    public Guid? CurrentLocationId { get; set; }

    public Guid? CurrentLabelId { get; set; }

    [Required]
    public DateOnly? DateFounded { get; set; }
     
    public DateOnly? DateDisbanded { get; set; }
}

public class ArtistUpdateRequest
{
    public string? Name { get; set; }

    public Guid? CurrentLocationId { get; set; }

    public Guid? CurrentLabelId { get; set; }

    public DateOnly? DateFounded { get; set; }

    public DateOnly? DateDisbanded { get; set; }
}

[BindProperties]
public class ArtistSearchRequest
{
    [FromQuery]
    public string? Ids { get; set; }

    [FromQuery]
    public string? CurrentLocationIds { get; set; }
     
    [FromQuery]
    public string? CurrentLabelIds { get; set; }

    [FromQuery]
    public string? NameLike { get; set; }

    [FromQuery]
    public DateOnly? DateFoundedMin { get; set; }

    [FromQuery]
    public DateOnly? DateFoundedMax { get; set; } 
}

public class ArtistSearchModel
{
    public List<Guid>? Ids { get; set; }
     
    public List<Guid>? CurrentLocationIds { get; set; }

    public List<Guid>? CurrentLabelIds { get; set; }

    public string? NameLike { get; set; }

    public DateOnly? DateFoundedMin { get; set; }
     
    public DateOnly? DateFoundedMax { get; set; }
}

public class ArtistModel
{
    public Guid Id { get; set; }
    public Guid? CurrentLabelId { get; set; }
    public Guid? CurrentLocationId { get; set; }
    public string Name { get; set; }
    public DateOnly DateFounded { get; set; }
    public DateOnly? DateDisbanded { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ArtistModel(Guid id, Guid? currentLabelId, Guid? currentLocationId, string name, DateOnly dateFounded, DateOnly? dateDisbanded, DateTime createdAt, DateTime? updatedAt)
    {
        this.Id = id;
        this.CurrentLabelId = currentLabelId;
        this.CurrentLocationId = currentLocationId;

        this.Name = name;
        this.DateFounded = dateFounded;
        this.DateDisbanded = dateDisbanded;

        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class ArtistDatabaseModel
{
    public Guid id { get; set; }
    public Guid? current_label_id { get; set; }
    public Guid? current_location_id { get; set; }
    public string name { get; set; }
    public DateTime date_founded { get; set; }
    public DateTime? date_disbanded { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public long? full_count { get; set; }

    public ArtistDatabaseModel(Guid id, Guid? current_label_id, Guid? current_location_id, string name, DateTime date_founded, DateTime? date_disbanded, DateTime created_at, DateTime? updated_at)
    {
        this.id = id;
        this.current_label_id = current_label_id;
        this.current_location_id = current_location_id;
        this.name = name;
        this.date_founded = date_founded;
        this.date_disbanded = date_disbanded;
        this.created_at = created_at;
        this.updated_at = updated_at;
    }

    public ArtistDatabaseModel(Guid id, Guid? current_label_id, Guid? current_location_id, string name, DateTime date_founded, DateTime? date_disbanded, DateTime created_at, DateTime? updated_at, long full_count)
        : this(id, current_label_id, current_location_id, name, date_founded, date_disbanded, created_at, updated_at)
    { 
        this.full_count = full_count;
    }

}

public class ArtistResponseModel
{
    public Guid Id { get; set; }
    public Guid? CurrentLabelId { get; set; }
    public Guid? CurrentLocationId { get; set; }
    public string Name { get; set; }
    public string DateFounded { get; set; }
    public string? DateDisbanded { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public ArtistResponseModel(Guid id, Guid? currentLabelId, Guid? currentLocationId, string name, string dateFounded, string? dateDisbanded, string createdAt, string? updatedAt)
    {
        this.Id = id;
        this.CurrentLabelId = currentLabelId;
        this.CurrentLocationId = currentLocationId;

        this.Name = name;
        this.DateFounded = dateFounded;
        this.DateDisbanded = dateDisbanded;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}