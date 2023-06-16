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
    const labelRetrieved = await getLabelById(context, labelResult.data.Id);

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
      Ids: [labelResult1.data.Id, labelResult3.data.Id]
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
    const labelResult1 = await createLabel(context, {Name: "testVal_" + randomstring}, entityMap)
    const labelResult2 = await createLabel(context, {Name: "AnotherTest_" + randomstring}, entityMap)
    const labelResult3 = await createLabel(context, {Name: "NoMatch_" + randomstring}, entityMap) 

    const searchModel : LabelSearchModel = {
      Ids: [labelResult1.data.Id, labelResult2.data.Id, labelResult3.data.Id],
      NameLike: "test"
    }

    const labelsRetrieved = await searchLabels(context, searchModel); 

    expect(labelsRetrieved.status).toEqual(200);

    expect(labelsRetrieved.data.length).toEqual(2);

    expect(labelsRetrieved.data[0]).toHaveSamePropertiesAs(labelResult1.data);
    expect(labelsRetrieved.data[1]).toHaveSamePropertiesAs(labelResult2.data); 
  }) 

  it('Gets labels with city filter', async () => {
    //just call with default values
 
    const labelResult1 = await createLabel(context, {City: "BeefTown"}, entityMap)
    const labelResult2 = await createLabel(context, {City: "NOT BEEF TOWN"}, entityMap)
    const labelResult3 = await createLabel(context, {City: "beefTown"}, entityMap) 

    const searchModel : LabelSearchModel = {
      Ids: [labelResult1.data.Id, labelResult2.data.Id, labelResult3.data.Id],
      City: "BeefTown"
    }

    const labelsRetrieved = await searchLabels(context, searchModel); 

    expect(labelsRetrieved.status).toEqual(200);

    expect(labelsRetrieved.data.length).toEqual(2);

    expect(labelsRetrieved.data[0]).toHaveSamePropertiesAs(labelResult1.data);
    expect(labelsRetrieved.data[1]).toHaveSamePropertiesAs(labelResult3.data); 
  }) 

  it('Gets labels with state filter', async () => {
    //just call with default values
 
    const labelResult1 = await createLabel(context, {State: "The State Of Beef"}, entityMap)
    const labelResult2 = await createLabel(context, {State: "thE StaTe of BEEF"}, entityMap)
    const labelResult3 = await createLabel(context, {State: "THE STATE OF PORK"}, entityMap) 

    const searchModel : LabelSearchModel = {
      Ids: [labelResult1.data.Id, labelResult2.data.Id, labelResult3.data.Id],
      State: "thE StaTe of BEEF"
    }

    const labelsRetrieved = await searchLabels(context, searchModel); 

    expect(labelsRetrieved.status).toEqual(200);

    expect(labelsRetrieved.data.length).toEqual(2);

    expect(labelsRetrieved.data[0]).toHaveSamePropertiesAs(labelResult1.data);
    expect(labelsRetrieved.data[1]).toHaveSamePropertiesAs(labelResult2.data); 
  }) 
})


