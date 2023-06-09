import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createArtist } from '../../../QDK/operators/artists'
import { initConfig } from '../../../QDK/config'
import { qapost } from '../../../QDK/qaxios'

describe('Post Artist Tests', () => {
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

  it('Posts a valid artist', async () => {
    //just call with default values
    await createArtist(context, {}, entityMap)
  })

  it('Gets errors for an empty artist', async () => {
    //just call with default values
    const result = await qapost(context.url + "/artists", {});
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.errors).length).toEqual(2);

    expect(result.data.errors["Name"]).toBeTruthy();
    expect(result.data.errors["Name"].length).toEqual(1);
    expect(result.data.errors["Name"][0]).toEqual('The Name field is required.'); 

    expect(result.data.errors["DateFounded"]).toBeTruthy();
    expect(result.data.errors["DateFounded"].length).toEqual(1);
    expect(result.data.errors["DateFounded"][0]).toEqual('The DateFounded field is required.'); 
  })

  it('Gets errors for an invalid artist', async () => {
    //just call with default values
    const result = await qapost(context.url + "/artists", { DateFounded: "notadate" });
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.errors).length).toEqual(2);

    expect(result.data.errors["$.DateFounded"]).toBeTruthy();
    expect(result.data.errors["$.DateFounded"].length).toEqual(1);
    expect(result.data.errors["$.DateFounded"][0]).toEqual('The JSON value could not be converted to System.Nullable`1[System.DateOnly]. Path: $.DateFounded | LineNumber: 0 | BytePositionInLine: 25.'); 
  })
})


