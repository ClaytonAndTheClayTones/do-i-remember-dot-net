import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createAlbum } from '../../../QDK/operators/albums'
import { initConfig } from '../../../QDK/config'
import { qapost } from '../../../QDK/qaxios'

describe('Post Album Tests', () => {
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

  it('Posts a valid album', async () => {
    //just call with default values
    await createAlbum(context, {}, entityMap)
  })

  it('Gets errors for an empty album', async () => {
    //just call with default values
    const result = await qapost(context.url + "/albums", {});
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.errors).length).toEqual(2);

    expect(result.data.errors["Name"]).toBeTruthy();
    expect(result.data.errors["Name"].length).toEqual(1);
    expect(result.data.errors["Name"][0]).toEqual('The Name field is required.'); 

    expect(result.data.errors["DateReleased"]).toBeTruthy();
    expect(result.data.errors["DateReleased"].length).toEqual(1);
    expect(result.data.errors["DateReleased"][0]).toEqual('The DateReleased field is required.'); 
  })

  it('Gets errors for an invalid album', async () => {
    //just call with default values
    const result = await qapost(context.url + "/albums", { DateReleased: "notADate"});
    expect(result.status).toEqual(400);
 
    expect(Object.keys(result.data.errors).length).toEqual(2);
  
    expect(result.data.errors["$.DateReleased"]).toBeTruthy();
    expect(result.data.errors["$.DateReleased"].length).toEqual(1);
    expect(result.data.errors["$.DateReleased"][0]).toEqual('The JSON value could not be converted to System.Nullable`1[System.DateOnly]. Path: $.DateReleased | LineNumber: 0 | BytePositionInLine: 26.'); 

  })
})


