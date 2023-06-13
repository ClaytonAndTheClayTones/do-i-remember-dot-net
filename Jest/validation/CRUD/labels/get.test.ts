import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { LabelSearchModel, createLabel, getLabelById, searchLabels } from '../../../QDK/operators/labels'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring'

describe('Get Label Tests', () => {
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

  it('Gets a posted label by id', async () => {
    //just call with default values
    const labelResult = await createLabel(context, {}, entityMap)
    const labelRetrieved = await getLabelById(context, labelResult.data.id);

    expect(labelRetrieved.status).toEqual(200)
    expect(labelRetrieved.data).toHaveSamePropertiesAs(labelResult.data);
  })

  it('Gets a 404 when retrieving a non-existant label', async () => {
    //just call with default values 
    const labelRetrieved = await getLabelById(context, "00000000-0000-0000-0000-000000000000", undefined, true);

    expect(labelRetrieved.status).toEqual(404) 
  })

  it('Gets labels with ids filter', async () => {
    //just call with default values
    const labelResult1 = await createLabel(context, {}, entityMap)
    await createLabel(context, {}, entityMap)
    const labelResult3 = await createLabel(context, {}, entityMap) 

    const searchModel : LabelSearchModel = {
      ids: [labelResult1.data.id, labelResult3.data.id]
    }

    const labelsRetrieved = await searchLabels(context, searchModel); 

    expect(labelsRetrieved.status).toEqual(200);

    expect(labelsRetrieved.data.length).toEqual(2);

    expect(labelsRetrieved.data[0]).toHaveSamePropertiesAs(labelResult1.data);
    expect(labelsRetrieved.data[1]).toHaveSamePropertiesAs(labelResult3.data); 
  }) 

  it('Gets labels with nameLike filter', async () => {
    //just call with default values

    const randomstring = generate(10);
    const labelResult1 = await createLabel(context, {name: "testVal_" + randomstring}, entityMap)
    const labelResult2 = await createLabel(context, {name: "Anothertest_" + randomstring}, entityMap)
    const labelResult3 = await createLabel(context, {name: "NoMatch_" + randomstring}, entityMap) 

    const searchModel : LabelSearchModel = {
      ids: [labelResult1.data.id, labelResult2.data.id, labelResult3.data.id],
      nameLike: "test"
    }

    const labelsRetrieved = await searchLabels(context, searchModel); 

    expect(labelsRetrieved.status).toEqual(200);

    expect(labelsRetrieved.data.length).toEqual(2);

    expect(labelsRetrieved.data[0]).toHaveSamePropertiesAs(labelResult1.data);
    expect(labelsRetrieved.data[1]).toHaveSamePropertiesAs(labelResult2.data); 
  }) 
})


