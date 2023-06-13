import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createLabel, deleteLabel, getLabelById } from '../../../QDK/operators/labels'
import { initConfig } from '../../../QDK/config'

describe('Delete Label Tests', () => {
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

  it('deletes a posted label by id', async () => {
    //just call with default values
    const labelResult = await createLabel(context, {}, entityMap)
    const labelDeleted = await deleteLabel(context, labelResult.data.id);
    const deletedLabelRetrieved = await getLabelById(context, labelResult.data.id, undefined, true);

    expect(labelDeleted.status).toEqual(200)
    expect(labelDeleted.data).toHaveSamePropertiesAs(labelResult.data);    
    expect(deletedLabelRetrieved.status).toEqual(404);
  }) 
})


