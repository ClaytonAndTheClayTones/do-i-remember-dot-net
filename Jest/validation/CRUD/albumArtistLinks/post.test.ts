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
 
    expect(Object.keys(result.data.errors).length).toEqual(2);

    expect(result.data.errors["AlbumId"]).toBeTruthy();
    expect(result.data.errors["AlbumId"].length).toEqual(1);
    expect(result.data.errors["AlbumId"][0]).toEqual('The AlbumId field is required.'); 

    expect(result.data.errors["ArtistId"]).toBeTruthy();
    expect(result.data.errors["ArtistId"].length).toEqual(1);
    expect(result.data.errors["ArtistId"][0]).toEqual('The ArtistId field is required.'); 
  })

  it('Gets errors for an invalid albumArtistLink', async () => {
    //just call with default values
    const result = await qapost(context.url + "/albumArtistLinks", { ArtistId: "not an id", AlbumId: "also not an id"});
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.errors).length).toEqual(3);

    expect(result.data.errors["$.AlbumId"]).toBeTruthy();
    expect(result.data.errors["$.AlbumId"].length).toEqual(1);
    expect(result.data.errors["$.AlbumId"][0]).toEqual('The Name field is required.'); 

    expect(result.data.errors["$.ArtistId"]).toBeTruthy();
    expect(result.data.errors["$.ArtistId"].length).toEqual(1);
    expect(result.data.errors["$.ArtistId"][0]).toEqual('The DateFounded field is required.'); 
  })
})


