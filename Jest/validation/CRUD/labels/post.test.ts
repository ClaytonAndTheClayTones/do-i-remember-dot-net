import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createLabel } from '../../../QDK/operators/labels'
import { initConfig } from '../../../QDK/config'

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

  it('Gets errors for invalid label', async () => {
    //just call with default values
    const result = await createLabel(context, { name: undefined }, entityMap, undefined, true)
    expect(result.status).toEqual(400);
    expect(result.data.errors["Name"]).toBeTruthy();
    expect(result.data.errors["Name"].length).toEqual(1);
    expect(result.data.errors["Name"][0]).toEqual('The Name field is required.'); 
  })
})


