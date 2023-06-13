import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { LabelUpdateModel, createLabel, updateLabel } from '../../../QDK/operators/labels'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring'

describe('Patch Label Tests', () => {
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

  it('Patches a valid label', async () => {
    //just call with default values
    const createdLabel = await createLabel(context, {}, entityMap)

    const updateModel : LabelUpdateModel = {
      name: "updateName" + generate(10),
      city: "updateCity",
      state: "updateState"
    }

    const updatedLabel = await updateLabel(context, createdLabel.data.id, updateModel, undefined, false)

    expect(updatedLabel.data).toHaveSamePropertiesAs(updateModel, ["id","createdAt", "updatedAt"]);
    expect(updatedLabel.data.id).toEqual(createdLabel.data.id);
    expect(updatedLabel.data.createdAt).toEqual(createdLabel.data.createdAt);
    expect(updatedLabel.data.updatedAt).toBeTruthy();
  }) 
})


