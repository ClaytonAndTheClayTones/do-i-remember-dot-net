import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap, sortByKey } from '../../../QDK/common'
import { ArtistSearchModel, createArtist, getArtistById, searchArtists } from '../../../QDK/operators/artists'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring' 

describe('Get Artist Tests', () => {
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

  it('Gets a posted artist by id', async () => {
    //just call with default values
    const artistResult = await createArtist(context, {}, entityMap)
    const artistRetrieved = await getArtistById(context, artistResult.data.Id);

    expect(artistRetrieved.status).toEqual(200)
    expect(artistRetrieved.data).toHaveSamePropertiesAs(artistResult.data);
  })

  it('Gets a 404 when retrieving a non-existant artist', async () => {
    //just call with default values 
    const artistRetrieved = await getArtistById(context, "00000000-0000-0000-0000-000000000000", undefined, true);

    expect(artistRetrieved.status).toEqual(404) 
  })

  it('Gets artists with ids filter', async () => {
    //just call with default values
    const artistResult1 = await createArtist(context, {}, entityMap)
    await createArtist(context, {}, entityMap)
    const artistResult3 = await createArtist(context, {}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult3.data.Id]
    }

    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(2);

    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult1.data);
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult3.data); 
  }) 
 
  it('Gets artists with paging', async () => {
    //just call with default values
    const artistResult1 = await createArtist(context, {}, entityMap)
    const artistResult2 = await createArtist(context, {}, entityMap)
    const artistResult3 = await createArtist(context, {}, entityMap) 
    const artistResult4 = await createArtist(context, {}, entityMap) 

    const searchModelPage1 : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      Page: 1,
      PageLength: 2
    };
 
    const searchModelPage2 : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      Page: 2,
      PageLength: 2
    };

    const artistsRetrievedPage1 = await searchArtists(context, searchModelPage1); 
    const artistsRetrievedPage2 = await searchArtists(context, searchModelPage2); 

    expect(artistsRetrievedPage1.status).toEqual(200);
    expect(artistsRetrievedPage2.status).toEqual(200);

    expect(artistsRetrievedPage1.data.Items.length).toEqual(2);
    expect(artistsRetrievedPage2.data.Items.length).toEqual(2);

    expect(artistsRetrievedPage1.data.Items[0]).toHaveSamePropertiesAs(artistResult1.data);
    expect(artistsRetrievedPage1.data.Items[1]).toHaveSamePropertiesAs(artistResult2.data); 
    expect(artistsRetrievedPage2.data.Items[0]).toHaveSamePropertiesAs(artistResult3.data);
    expect(artistsRetrievedPage2.data.Items[1]).toHaveSamePropertiesAs(artistResult4.data); 
  }) 

  it('Gets artists with nameLike filter', async () => {
    //just call with default values

    const randomstring = generate(12);
    const artistResult1 = await createArtist(context, {Name: "testVal_" + randomstring}, entityMap)
    const artistResult2 = await createArtist(context, {Name: "AnotherTest_" + randomstring}, entityMap)
    const artistResult3 = await createArtist(context, {Name: "NoMatch_" + randomstring}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id],
      NameLike: "test"
    }

    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(2);

    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult1.data);
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult2.data);
  }) 
  
  it('Gets labels with currentLocationIds filter', async () => {
    //just call with default valueateArtist
   
    const artistResult1 = await createArtist(context, {}, entityMap)
    const artistResult2 = await createArtist(context, {}, entityMap)
    const artistResult3 = await createArtist(context, {CurrentLocationId : artistResult1.data.CurrentLocationId}, entityMap) 
    const artistResult4 = await createArtist(context, {}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      CurrentLocationIds: [artistResult1.data.CurrentLocationId, artistResult4.data.CurrentLocationId]
    }

    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(3);

    sortByKey(artistsRetrieved.data.Items, 'createdAt');
     
    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult1.data);
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult3.data); 
    expect(artistsRetrieved.data.Items[2]).toHaveSamePropertiesAs(artistResult4.data); 
  })  
 
  it('Gets labels with currentLabelIds filter', async () => {
    //just call with default valueateArtist
   
    const artistResult1 = await createArtist(context, {}, entityMap)
    const artistResult2 = await createArtist(context, {}, entityMap)
    const artistResult3 = await createArtist(context, {CurrentLabelId : artistResult1.data.CurrentLabelId}, entityMap) 
    const artistResult4 = await createArtist(context, {}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      CurrentLabelIds: [artistResult1.data.CurrentLabelId, artistResult4.data.CurrentLabelId]
    }

    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(3);

    sortByKey(artistsRetrieved.data.Items, 'createdAt');
     
    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult1.data);
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult3.data); 
    expect(artistsRetrieved.data.Items[2]).toHaveSamePropertiesAs(artistResult4.data); 
  })  
 
  it('Gets labels with DateFoundedMin filter', async () => {
    //just call with default valueateArtist
   
    const artistResult1 = await createArtist(context, {DateFounded: '2005-05-05'}, entityMap)
    const artistResult2 = await createArtist(context, {DateFounded: '2005-05-06'}, entityMap)
    const artistResult3 = await createArtist(context, {DateFounded: '2005-05-07'}, entityMap) 
    const artistResult4 = await createArtist(context, {DateFounded: '2005-05-08'}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      DateFoundedMin: artistResult3.data.DateFounded
    }
 
    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(2);

    sortByKey(artistsRetrieved.data.Items, 'createdAt');
      
    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult3.data); 
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult4.data); 
  })  

  it('Gets labels with DateFoundedMax filter', async () => {
    //just call with default valueateArtist
   
    const artistResult1 = await createArtist(context, {DateFounded: '2005-05-05'}, entityMap)
    const artistResult2 = await createArtist(context, {DateFounded: '2005-05-06'}, entityMap)
    const artistResult3 = await createArtist(context, {DateFounded: '2005-05-07'}, entityMap) 
    const artistResult4 = await createArtist(context, {DateFounded: '2005-05-08'}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      DateFoundedMax: artistResult3.data.DateFounded
    }
 
    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(3);

    sortByKey(artistsRetrieved.data.Items, 'createdAt');
      
    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult1.data);  
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult2.data);  
    expect(artistsRetrieved.data.Items[2]).toHaveSamePropertiesAs(artistResult3.data);  
  })  

  it('Gets labels with DateFoundedMax filter', async () => {
    //just call with default valueateArtist
   
    const artistResult1 = await createArtist(context, {DateFounded: '2005-05-05'}, entityMap)
    const artistResult2 = await createArtist(context, {DateFounded: '2005-05-06'}, entityMap)
    const artistResult3 = await createArtist(context, {DateFounded: '2005-05-07'}, entityMap) 
    const artistResult4 = await createArtist(context, {DateFounded: '2005-05-08'}, entityMap) 

    const searchModel : ArtistSearchModel = {
      Ids: [artistResult1.data.Id, artistResult2.data.Id, artistResult3.data.Id, artistResult4.data.Id],
      DateFoundedMin: artistResult2.data.DateFounded,
      DateFoundedMax: artistResult3.data.DateFounded
    }
 
    const artistsRetrieved = await searchArtists(context, searchModel); 

    expect(artistsRetrieved.status).toEqual(200);

    expect(artistsRetrieved.data.Items.length).toEqual(2);

    sortByKey(artistsRetrieved.data.Items, 'createdAt');
      
    expect(artistsRetrieved.data.Items[0]).toHaveSamePropertiesAs(artistResult2.data);  
    expect(artistsRetrieved.data.Items[1]).toHaveSamePropertiesAs(artistResult3.data);   
  })  
})


