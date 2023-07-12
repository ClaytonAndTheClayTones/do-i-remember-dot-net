import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { AlbumArtistLinkUpdateModel, createAlbumArtistLink, updateAlbumArtistLink } from '../../../QDK/operators/albumArtistLinks'
import { initConfig } from '../../../QDK/config' 
import { createAlbum } from 'QDK/operators/albums'
import { createArtist } from 'QDK/operators/artists'

describe('Patch AlbumArtistLink Tests', () => {
  let entityMap: TestEntityMap = new TestEntityMap()

  beforeEach(async () => {
    entityMap = new TestEntityMap()
    entityMap.entities = []
  })

  afterEach(async () => {
    await entityMap.cleanup()
  })

  let context: MusixApiContext;

  beforeAll(async () => { 
 
    initConfig();

    context = {
      url: process.env.TEST_MUSIX_URL || ''
    } 
  })

  it('Patches a valid albumArtistLink', async () => {
    //just call with default values
    const createdAlbumArtistLink = await createAlbumArtistLink(context, {}, entityMap);

    const albumToUpdateTo = await createAlbum(context, undefined, entityMap);
    const artistToUpdateTo = await createArtist(context, undefined, entityMap);

    const updateModel : AlbumArtistLinkUpdateModel = { 
      AlbumId: albumToUpdateTo.data.Id,
      ArtistId: artistToUpdateTo.data.Id
    }

    const updatedAlbumArtistLink = await updateAlbumArtistLink(context, createdAlbumArtistLink.data.Id, updateModel, undefined, false)

    expect(updatedAlbumArtistLink.data).toHaveSamePropertiesAs(updateModel, ["Id", "CreatedAt", "UpdatedAt"]);
    expect(updatedAlbumArtistLink.data.Id).toEqual(createdAlbumArtistLink.data.Id);
    expect(updatedAlbumArtistLink.data.CreatedAt).toEqual(createdAlbumArtistLink.data.CreatedAt);
    expect(updatedAlbumArtistLink.data.UpdatedAt).toBeTruthy();
  }) 
})


