import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createAlbumArtistLink, deleteAlbumArtistLink, getAlbumArtistLinkById } from '../../../QDK/operators/albumArtistLinks'
import { initConfig } from '../../../QDK/config'

describe('Delete AlbumArtistLink Tests', () => {
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

  it('deletes a posted albumAlbumArtistLinkLink by id', async () => {
    //just call with default values
    const albumArtistLinkResult = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkDeleted = await deleteAlbumArtistLink(context, albumArtistLinkResult.data.Id);
    const albumArtistLinkRetrieved = await getAlbumArtistLinkById(context, albumArtistLinkResult.data.Id, undefined, true);

    expect(albumArtistLinkDeleted.status).toEqual(200)
    expect(albumArtistLinkDeleted.data).toHaveSamePropertiesAs(albumArtistLinkResult.data);    
    expect(albumArtistLinkRetrieved.status).toEqual(404);
  }) 
})


