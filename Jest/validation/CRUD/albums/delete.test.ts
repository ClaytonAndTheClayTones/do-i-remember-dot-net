import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createAlbum, deleteAlbum, getAlbumById } from '../../../QDK/operators/albums'
import { initConfig } from '../../../QDK/config'

describe('Delete Album Tests', () => {
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

  it('deletes a posted album by id', async () => {
    //just call with default values
    const albumResult = await createAlbum(context, {}, entityMap)
    const albumDeleted = await deleteAlbum(context, albumResult.data.Id);
    const deletedAlbumRetrieved = await getAlbumById(context, albumResult.data.Id, undefined, true);

    expect(albumDeleted.status).toEqual(200)
    expect(albumDeleted.data).toHaveSamePropertiesAs(albumResult.data);    
    expect(deletedAlbumRetrieved.status).toEqual(404);
  }) 
})


