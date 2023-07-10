import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createLabel } from '../../../QDK/operators/labels'
import { initConfig } from '../../../QDK/config'
import { qapost } from '../../../QDK/qaxios'

describe('Post Label Tests', () => {
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

  it('Posts a valid label', async () => {
    //just call with default values
    await createLabel(context, {}, entityMap)
  })

  it('Gets errors for empty label', async () => {
    //just call with default values
    const result = await qapost(context.url + "/labels", {});
    
    expect(result.status).toEqual(400);
    expect(result.data.errors["Name"]).toBeTruthy();
    expect(result.data.errors["Name"].length).toEqual(1);
    expect(result.data.errors["Name"][0]).toEqual('The Name field is required.'); 
  })

  it('Gets errors for invalid label', async () => {
    //just call with default values
    const result = await createLabel(context, { CurrentLocationId: "notAnId"}, entityMap, undefined, true)
    expect(result.status).toEqual(400);
    expect(result.data.errors["$.CurrentLocationId"]).toBeTruthy();
    expect(result.data.errors["$.CurrentLocationId"].length).toEqual(1);
    expect(result.data.errors["$.CurrentLocationId"][0]).toEqual("The JSON value could not be converted to System.Nullable`1[System.Guid]. Path: $.CurrentLocationId | LineNumber: 0 | BytePositionInLine: 60."); 
  })
})


