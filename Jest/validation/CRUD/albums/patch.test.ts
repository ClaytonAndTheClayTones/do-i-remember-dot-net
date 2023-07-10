import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { AlbumUpdateModel, createAlbum, updateAlbum } from '../../../QDK/operators/albums'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring'
import { createLabel } from '../../../QDK/operators/labels' 

describe('Patch Album Tests', () => {
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

  it('Patches a valid album', async () => {
    //just call with default values
    const createdAlbum = await createAlbum(context, {}, entityMap);

    const labelToUpdateTo = await createLabel(context, undefined, entityMap); 

    const updateModel : AlbumUpdateModel = {
      Name: "updateName" + generate(12),
      LabelId: labelToUpdateTo.data.Id, 
      DateReleased: "2003-03-03",  
    }

    const updatedAlbum = await updateAlbum(context, createdAlbum.data.Id, updateModel, undefined, false)

    expect(updatedAlbum.data).toHaveSamePropertiesAs(updateModel, ["Id", "CreatedAt", "UpdatedAt"]);
    expect(updatedAlbum.data.Id).toEqual(createdAlbum.data.Id);
    expect(updatedAlbum.data.CreatedAt).toEqual(createdAlbum.data.CreatedAt);
    expect(updatedAlbum.data.UpdatedAt).toBeTruthy();
  }) 
})


