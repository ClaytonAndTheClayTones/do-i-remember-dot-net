namespace WebApi.Adapters.AlbumArtistLinkAdapter;

using WebApi.Models.AlbumArtistLinks;
using WebApi.Helpers;
using WebApi.Adapters.Common;

public interface IAlbumArtistLinkAdapter : IModelAdapter<AlbumArtistLinkCreateRequest, AlbumArtistLinkUpdateRequest, AlbumArtistLinkSearchRequest, AlbumArtistLinkDatabaseModel, AlbumArtistLinkModel, AlbumArtistLinkSearchModel, AlbumArtistLinkResponseModel> { }
  
public class AlbumArtistLinkAdapter : IAlbumArtistLinkAdapter
{
    ICommonUtils _commonUtils;

    public AlbumArtistLinkAdapter(ICommonUtils commonUtils)
    {
        _commonUtils = commonUtils;
    }

    public AlbumArtistLinkResponseModel convertFromModelToResponseModel(AlbumArtistLinkModel model)
    {  
        AlbumArtistLinkResponseModel responseModel = new AlbumArtistLinkResponseModel(
            id: model.Id,
            albumId: model.AlbumId,
            artistId: model.ArtistId,

            createdAt: model.CreatedAt.ToString("s"),
            updatedAt: model.UpdatedAt != null ? $"{model.UpdatedAt:s}" : null
        );

        return responseModel;
    }

    public AlbumArtistLinkModel convertFromDatabaseModelToModel(AlbumArtistLinkDatabaseModel model)
    {
        AlbumArtistLinkModel responseModel = new AlbumArtistLinkModel(
            id: model.id,
            albumId: model.album_id,
            artistId: model.artist_id,

            createdAt: model.created_at,
            updatedAt: model.updated_at
        );

        return responseModel;
    }

    public AlbumArtistLinkSearchModel convertFromRequestToSearchModel(AlbumArtistLinkSearchRequest request)
    {
        AlbumArtistLinkSearchModel result = new AlbumArtistLinkSearchModel();

        if (request.Ids != null)
        {
            result.Ids = this._commonUtils.ConvertDelimitedStringToGuidList(request.Ids);
        }

        if (request.AlbumIds != null)
        {
            result.AlbumIds = this._commonUtils.ConvertDelimitedStringToGuidList(request.AlbumIds);
        }

        if (request.ArtistIds != null)
        {
            result.ArtistIds = this._commonUtils.ConvertDelimitedStringToGuidList(request.ArtistIds);
        }
  
        return result;
    }

    public List<ISearchTerm> convertFromSearchModelToSearchTerms(AlbumArtistLinkSearchModel? model)
    {
        List<ISearchTerm> searchTerms = new List<ISearchTerm>();

        if (model != null)
        {
            if (model.Ids != null && model.Ids.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("id", model.Ids));
            }

            if (model.AlbumIds != null && model.AlbumIds.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("album_id", model.AlbumIds));
            }

            if (model.ArtistIds != null && model.ArtistIds.Count > 0)
            {
                searchTerms.Add(new InArraySearchTerm<Guid>("artist_id", model.ArtistIds));
            } 
         }

        return searchTerms;
    }

    public object convertFromCreateRequestToDatabaseModel(AlbumArtistLinkCreateRequest model)
    {
        return new
        {
            album_id = model.AlbumId,
            artist_id = model.ArtistId 
        };
    }

    public object convertFromUpdateRequestToDatabaseModel(AlbumArtistLinkUpdateRequest model)
    {
        return new
        {
            album_id = model.AlbumId,
            artist_id = model.ArtistId
        };
    }
}