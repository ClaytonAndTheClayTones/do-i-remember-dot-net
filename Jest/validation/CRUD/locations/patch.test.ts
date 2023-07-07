import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { LocationUpdateModel, createLocation, updateLocation } from '../../../QDK/operators/locations'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring'

describe('Patch Location Tests', () => {
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

  it('Patches a valid location', async () => {
    //just call with default values
    const createdLocation = await createLocation(context, {}, entityMap)

    const updateModel : LocationUpdateModel = { 
      City: "updateCity:" + generate(12),
      State: "updateState"
    }

    const updatedLocation = await updateLocation(context, createdLocation.data.Id, updateModel, undefined, false)

    expect(updatedLocation.data).toHaveSamePropertiesAs(updateModel, ["Id", "CreatedAt", "UpdatedAt"]);
    expect(updatedLocation.data.Id).toEqual(createdLocation.data.Id);
    expect(updatedLocation.data.CreatedAt).toEqual(createdLocation.data.CreatedAt);
    expect(updatedLocation.data.UpdatedAt).toBeTruthy();
  }) 
})


