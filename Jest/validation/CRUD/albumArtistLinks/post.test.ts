import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createAlbumArtistLink } from '../../../QDK/operators/albumArtistLinks'
import { initConfig } from '../../../QDK/config'
import { qapost } from '../../../QDK/qaxios'

describe('Post AlbumArtistLink Tests', () => {
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

  it('Posts a valid albumArtistLink', async () => {
    //just call with default values
    await createAlbumArtistLink(context, {}, entityMap)
  })

  it('Gets errors for an empty albumArtistLink', async () => {
    //just call with default values
    const result = await qapost(context.url + "/albumArtistLinks", {});
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.Errors).length).toEqual(2);

    expect(result.data.Errors[0].PropertyName).toEqual("AlbumId");
    expect(result.data.Errors[0].ErrorMessage).toEqual("Required property AlbumId is missing."); 

    expect(result.data.Errors[1].PropertyName).toEqual("ArtistId");
    expect(result.data.Errors[1].ErrorMessage).toEqual("Required property ArtistId is missing."); 
  })

  it('Gets errors for an invalid albumArtistLink', async () => {
    //just call with default values
    const result = await qapost(context.url + "/albumArtistLinks", { ArtistId: "not an id", AlbumId: "also not an id"});
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.Errors).length).toEqual(2);

    expect(result.data.Errors[0].PropertyName).toEqual("AlbumId");
    expect(result.data.Errors[0].ErrorMessage).toEqual('Property AlbumId must be a valid GUID in the format xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.'); 

    expect(result.data.Errors[1].PropertyName).toEqual("ArtistId");
    expect(result.data.Errors[1].ErrorMessage).toEqual('Property ArtistId must be a valid GUID in the format xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.'); 
  })
})


