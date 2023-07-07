import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createLocation, deleteLocation, getLocationById } from '../../../QDK/operators/locations'
import { initConfig } from '../../../QDK/config'

describe('Delete Location Tests', () => {
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

  it('deletes a posted location by id', async () => {
    //just call with default values
    const locationResult = await createLocation(context, {}, entityMap)
    const locationDeleted = await deleteLocation(context, locationResult.data.Id);
    const deletedLocationRetrieved = await getLocationById(context, locationResult.data.Id, undefined, true);

    expect(locationDeleted.status).toEqual(200)
    expect(locationDeleted.data).toHaveSamePropertiesAs(locationResult.data);    
    expect(deletedLocationRetrieved.status).toEqual(404);
  }) 
})


