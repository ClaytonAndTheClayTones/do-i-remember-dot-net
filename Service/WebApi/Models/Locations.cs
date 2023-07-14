namespace WebApi.Models.Locations;
  
using Microsoft.AspNetCore.Mvc;

public class LocationCreateRequest
{ 
    public string? City { get; set; }
     
    public string? State { get; set; }
}

public class LocationUpdateRequest
{
    public string? City { get; set; }

    public string? State { get; set; }
}

public class LocationCreateModel
{ 
    public string? City { get; set; }
     
    public string? State { get; set; }
}

public class LocationUpdateModel
{
    public string? City { get; set; }

    public string? State { get; set; }
}

[BindProperties]
public class LocationSearchRequest
{
    [FromQuery]
    public string? Ids { get; set; }

    [FromQuery]
    public string? CityOrStateLike { get; set; }
}

public class LocationSearchModel
{
    public List<Guid>? Ids { get; set; } 

    public string? CityOrStateLike { get; set; }
}

public class LocationModel
{
    public Guid Id { get; set; } 
    public string? City { get; set; }
    public string? State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public LocationModel(Guid id, string? city, string? state, DateTime createdAt, DateTime? updatedAt)
    { 
        this.Id = id;
        this.City = city;
        this.State = state;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class LocationDatabaseModel
{
    public Guid id { get; set; } 
    public string? city { get; set; }
    public string? state { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public long? full_count { get; set; }

    public LocationDatabaseModel(Guid id, string? city, string? state, DateTime created_at, DateTime? updated_at)
    { 
        this.id = id;
        this.city = city;
        this.state = state;
        this.created_at = created_at;
        this.updated_at = updated_at;
    }
     
    public LocationDatabaseModel(Guid id, string? city, string? state, DateTime created_at, DateTime? updated_at, long? full_count)
        : this(id, city, state, created_at, updated_at)
    {  
        this.full_count = full_count;
    }
}

public class LocationResponseModel
{
    public Guid Id { get; set; } 
    public string? City { get; set; }
    public string? State { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public LocationResponseModel(Guid id, string? city, string? state, string createdAt, string? updatedAt)
    { 
        this.Id = id;
        this.City = city;
        this.State = state;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}