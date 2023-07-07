import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createLocation } from '../../../QDK/operators/locations'
import { initConfig } from '../../../QDK/config'

describe('Post Location Tests', () => {
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

  it('Posts a valid location', async () => {
    //just call with default values
    await createLocation(context, {}, entityMap)
  })

  it('Gets errors for invalid location', async () => {
    //just call with default values
    const result = await createLocation(context, { City: undefined, State: undefined }, entityMap, undefined, true)
    expect(result.status).toEqual(400);

    expect(Object.keys(result.data.errors).length).toEqual(2)

    expect(result.data.errors["City"]).toBeTruthy();
    expect(result.data.errors["City"].length).toEqual(1);
    expect(result.data.errors["City"][0]).toEqual('The City field is required.'); 

    expect(result.data.errors["State"]).toBeTruthy();
    expect(result.data.errors["State"].length).toEqual(1);
    expect(result.data.errors["State"][0]).toEqual('The State field is required.'); 
  })
})


