namespace WebApi.Models.AlbumArtistLinks;
 
using Microsoft.AspNetCore.Mvc; 

public class AlbumArtistLinkCreateRequest
{
    public string? AlbumId { get; set; }

    public string? ArtistId { get; set; }
}

public class AlbumArtistLinkCreateModel
{
    public Guid? AlbumId { get; set; }

    public Guid? ArtistId { get; set; }
}

[BindProperties]
public class AlbumArtistLinkSearchRequest
{
    [FromQuery]
    public string? Ids { get; set; }

    [FromQuery]
    public string? AlbumIds { get; set; }

    [FromQuery]
    public string? ArtistIds { get; set; }
}

public class AlbumArtistLinkSearchModel
{
    public List<Guid>? Ids { get; set; }
     
    public List<Guid>? AlbumIds { get; set; }

    public List<Guid>? ArtistIds { get; set; } 
}

public class AlbumArtistLinkModel
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public Guid ArtistId { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public AlbumArtistLinkModel(Guid id, Guid albumId, Guid artistId, DateTime createdAt, DateTime? updatedAt)
    {
        this.Id = id;
        this.AlbumId = albumId;
        this.ArtistId = artistId;
         
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}

public class AlbumArtistLinkDatabaseModel
{
    public Guid id { get; set; }
    public Guid album_id { get; set; }
    public Guid artist_id { get; set; }

    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public long? full_count { get; set; }

    public AlbumArtistLinkDatabaseModel(Guid id, Guid album_id, Guid artist_id, DateTime created_at, DateTime? updated_at)
    {
        this.id = id;
        this.album_id = album_id;
        this.artist_id = artist_id;

        this.created_at = created_at;
        this.updated_at = updated_at;
    }

    public AlbumArtistLinkDatabaseModel(Guid id, Guid album_id, Guid artist_id, DateTime created_at, DateTime? updated_at, long full_count)
        : this(id, album_id, artist_id, created_at, updated_at)
    { 
        this.full_count = full_count;
    } 
}

public class AlbumArtistLinkResponseModel
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public Guid ArtistId { get; set; }

    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public AlbumArtistLinkResponseModel(Guid id, Guid albumId, Guid artistId, string createdAt, string? updatedAt)
    {
        this.Id = id;
        this.AlbumId = albumId;
        this.ArtistId = artistId;
         
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }
}