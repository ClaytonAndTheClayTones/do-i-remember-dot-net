import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { createArtist, deleteArtist, getArtistById } from '../../../QDK/operators/artists'
import { initConfig } from '../../../QDK/config'

describe('Delete Artist Tests', () => {
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

  it('deletes a posted artist by id', async () => {
    //just call with default values
    const artistResult = await createArtist(context, {}, entityMap)
    const artistDeleted = await deleteArtist(context, artistResult.data.Id);
    const deletedArtistRetrieved = await getArtistById(context, artistResult.data.Id, undefined, true);

    expect(artistDeleted.status).toEqual(200)
    expect(artistDeleted.data).toHaveSamePropertiesAs(artistResult.data);    
    expect(deletedArtistRetrieved.status).toEqual(404);
  }) 
})


