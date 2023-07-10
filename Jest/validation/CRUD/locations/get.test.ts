import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { LocationSearchModel, createLocation, getLocationById, searchLocations } from '../../../QDK/operators/locations'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring'

describe('Get Location Tests', () => {
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

  it('Gets a posted location by id', async () => {
    //just call with default values
    const locationResult = await createLocation(context, {}, entityMap)
    const locationRetrieved = await getLocationById(context, locationResult.data.Id);

    expect(locationRetrieved.status).toEqual(200)
    expect(locationRetrieved.data).toHaveSamePropertiesAs(locationResult.data);
  })

  it('Gets a 404 when retrieving a non-existant location', async () => {
    //just call with default values 
    const locationRetrieved = await getLocationById(context, "00000000-0000-0000-0000-000000000000", undefined, true);

    expect(locationRetrieved.status).toEqual(404) 
  })

  it('Gets locations with ids filter', async () => {
    //just call with default values
    const locationResult1 = await createLocation(context, {}, entityMap)
    await createLocation(context, {}, entityMap)
    const locationResult3 = await createLocation(context, {}, entityMap) 

    const searchModel : LocationSearchModel = {
      Ids: [locationResult1.data.Id, locationResult3.data.Id]
    }

    const locationsRetrieved = await searchLocations(context, searchModel); 

    expect(locationsRetrieved.status).toEqual(200);

    expect(locationsRetrieved.data.Items.length).toEqual(2);

    expect(locationsRetrieved.data.Items[0]).toHaveSamePropertiesAs(locationResult1.data);
    expect(locationsRetrieved.data.Items[1]).toHaveSamePropertiesAs(locationResult3.data); 
  }) 
 
  it('Gets locations with paging', async () => {
    //just call with default values
    const locationResult1 = await createLocation(context, {}, entityMap)
    const locationResult2 = await createLocation(context, {}, entityMap)
    const locationResult3 = await createLocation(context, {}, entityMap) 
    const locationResult4 = await createLocation(context, {}, entityMap) 

    const searchModelPage1 : LocationSearchModel = {
      Ids: [locationResult1.data.Id, locationResult2.data.Id, locationResult3.data.Id, locationResult4.data.Id],
      Page: 1,
      PageLength: 2
    };
 
    const searchModelPage2 : LocationSearchModel = {
      Ids: [locationResult1.data.Id, locationResult2.data.Id, locationResult3.data.Id, locationResult4.data.Id],
      Page: 2,
      PageLength: 2
    };

    const locationsRetrievedPage1 = await searchLocations(context, searchModelPage1); 
    const locationsRetrievedPage2 = await searchLocations(context, searchModelPage2); 

    expect(locationsRetrievedPage1.status).toEqual(200);
    expect(locationsRetrievedPage2.status).toEqual(200);

    expect(locationsRetrievedPage1.data.Items.length).toEqual(2);
    expect(locationsRetrievedPage2.data.Items.length).toEqual(2);

    expect(locationsRetrievedPage1.data.Items[0]).toHaveSamePropertiesAs(locationResult1.data);
    expect(locationsRetrievedPage1.data.Items[1]).toHaveSamePropertiesAs(locationResult2.data); 
    expect(locationsRetrievedPage2.data.Items[0]).toHaveSamePropertiesAs(locationResult3.data);
    expect(locationsRetrievedPage2.data.Items[1]).toHaveSamePropertiesAs(locationResult4.data); 
  }) 

  it('Gets locations with CityOrStateLike filter', async () => {
    //just call with default values

    const randomstringToMatch = generate(12); 
    const randomstringToNotMatch = generate(12); 
    const locationResult1 = await createLocation(context, { City: "ATestCity:" + randomstringToMatch, State: "notMatchingState:" + randomstringToNotMatch }, entityMap)
    const locationResult2 = await createLocation(context, { City: "ANotMatchingCity:" + randomstringToNotMatch, State: "testState:" + randomstringToMatch }, entityMap)
    const locationResult3 = await createLocation(context, { City: "ANotMatchingCity:" + randomstringToNotMatch, State: "notMatchingState:" + randomstringToNotMatch }, entityMap) 

    const searchModel : LocationSearchModel = {
      Ids: [locationResult1.data.Id, locationResult2.data.Id, locationResult3.data.Id],
      CityOrStateLike: randomstringToMatch
    }

    const locationsRetrieved = await searchLocations(context, searchModel); 

    expect(locationsRetrieved.status).toEqual(200);

    expect(locationsRetrieved.data.Items.length).toEqual(2);

    expect(locationsRetrieved.data.Items[0]).toHaveSamePropertiesAs(locationResult1.data);
    expect(locationsRetrieved.data.Items[1]).toHaveSamePropertiesAs(locationResult2.data); 
  })  
})


