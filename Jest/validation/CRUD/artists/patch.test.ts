import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap } from '../../../QDK/common'
import { ArtistUpdateModel, createArtist, updateArtist } from '../../../QDK/operators/artists'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring'
import { createLabel } from '../../../QDK/operators/labels'
import { createLocation } from '../../../QDK/operators/locations'

describe('Patch Artist Tests', () => {
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

  it('Patches a valid artist', async () => {
    //just call with default values
    const createdArtist = await createArtist(context, {}, entityMap);

    const labelToUpdateTo = await createLabel(context, undefined, entityMap);
    const locationToUpdateTo = await createLocation(context, undefined, entityMap);

    const updateModel : ArtistUpdateModel = {
      Name: "updateName" + generate(12),
      CurrentLabelId: labelToUpdateTo.data.Id,
      CurrentLocationId: locationToUpdateTo.data.Id,
      DateFounded: "2003-03-03",
      DateDisbanded: "2004-04-04" 
    }

    const updatedArtist = await updateArtist(context, createdArtist.data.Id, updateModel, undefined, false)

    expect(updatedArtist.data).toHaveSamePropertiesAs(updateModel, ["Id", "CreatedAt", "UpdatedAt"]);
    expect(updatedArtist.data.Id).toEqual(createdArtist.data.Id);
    expect(updatedArtist.data.CreatedAt).toEqual(createdArtist.data.CreatedAt);
    expect(updatedArtist.data.UpdatedAt).toBeTruthy();
  }) 
})


