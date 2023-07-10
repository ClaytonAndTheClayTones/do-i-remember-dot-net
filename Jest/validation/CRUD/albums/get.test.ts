import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap, sortByKey } from '../../../QDK/common'
import { AlbumSearchModel, createAlbum, getAlbumById, searchAlbums } from '../../../QDK/operators/albums'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring' 

describe('Get Album Tests', () => {
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

  it('Gets a posted album by id', async () => {
    //just call with default values
    const albumResult = await createAlbum(context, {}, entityMap)
    const albumRetrieved = await getAlbumById(context, albumResult.data.Id);

    expect(albumRetrieved.status).toEqual(200)
    expect(albumRetrieved.data).toHaveSamePropertiesAs(albumResult.data);
  })

  it('Gets a 404 when retrieving a non-existant album', async () => {
    //just call with default values 
    const albumRetrieved = await getAlbumById(context, "00000000-0000-0000-0000-000000000000", undefined, true);

    expect(albumRetrieved.status).toEqual(404) 
  })

  it('Gets albums with ids filter', async () => {
    //just call with default values
    const albumResult1 = await createAlbum(context, {}, entityMap)
    await createAlbum(context, {}, entityMap)
    const albumResult3 = await createAlbum(context, {}, entityMap) 

    const searchModel : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult3.data.Id]
    }

    const albumsRetrieved = await searchAlbums(context, searchModel); 

    expect(albumsRetrieved.status).toEqual(200);

    expect(albumsRetrieved.data.Items.length).toEqual(2);

    expect(albumsRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumResult1.data);
    expect(albumsRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumResult3.data); 
  }) 
 
  it('Gets albums with paging', async () => {
    //just call with default values
    const albumResult1 = await createAlbum(context, {}, entityMap)
    const albumResult2 = await createAlbum(context, {}, entityMap)
    const albumResult3 = await createAlbum(context, {}, entityMap) 
    const albumResult4 = await createAlbum(context, {}, entityMap) 

    const searchModelPage1 : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id, albumResult4.data.Id],
      Page: 1,
      PageLength: 2
    };
 
    const searchModelPage2 : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id, albumResult4.data.Id],
      Page: 2,
      PageLength: 2
    };

    const albumsRetrievedPage1 = await searchAlbums(context, searchModelPage1); 
    const albumsRetrievedPage2 = await searchAlbums(context, searchModelPage2); 

    expect(albumsRetrievedPage1.status).toEqual(200);
    expect(albumsRetrievedPage2.status).toEqual(200);

    expect(albumsRetrievedPage1.data.Items.length).toEqual(2);
    expect(albumsRetrievedPage2.data.Items.length).toEqual(2);

    expect(albumsRetrievedPage1.data.Items[0]).toHaveSamePropertiesAs(albumResult1.data);
    expect(albumsRetrievedPage1.data.Items[1]).toHaveSamePropertiesAs(albumResult2.data); 
    expect(albumsRetrievedPage2.data.Items[0]).toHaveSamePropertiesAs(albumResult3.data);
    expect(albumsRetrievedPage2.data.Items[1]).toHaveSamePropertiesAs(albumResult4.data); 
  }) 

  it('Gets albums with nameLike filter', async () => {
    //just call with default values

    const randomstring = generate(12);
    const albumResult1 = await createAlbum(context, {Name: "testVal_" + randomstring}, entityMap)
    const albumResult2 = await createAlbum(context, {Name: "AnotherTest_" + randomstring}, entityMap)
    const albumResult3 = await createAlbum(context, {Name: "NoMatch_" + randomstring}, entityMap) 

    const searchModel : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id],
      NameLike: "test"
    }

    const albumsRetrieved = await searchAlbums(context, searchModel); 

    expect(albumsRetrieved.status).toEqual(200);

    expect(albumsRetrieved.data.Items.length).toEqual(2);

    expect(albumsRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumResult1.data);
    expect(albumsRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumResult2.data);
  }) 
    
  it('Gets labels with labelIds filter', async () => {
    //just call with default valueateAlbum
   
    const albumResult1 = await createAlbum(context, {}, entityMap)
    const albumResult2 = await createAlbum(context, {}, entityMap)
    const albumResult3 = await createAlbum(context, {LabelId : albumResult1.data.LabelId}, entityMap) 
    const albumResult4 = await createAlbum(context, {}, entityMap) 

    const searchModel : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id, albumResult4.data.Id],
      LabelIds: [albumResult1.data.LabelId, albumResult4.data.LabelId]
    }

    const albumsRetrieved = await searchAlbums(context, searchModel); 

    expect(albumsRetrieved.status).toEqual(200);

    expect(albumsRetrieved.data.Items.length).toEqual(3);

    sortByKey(albumsRetrieved.data.Items, 'createdAt');
     
    expect(albumsRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumResult1.data);
    expect(albumsRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumResult3.data); 
    expect(albumsRetrieved.data.Items[2]).toHaveSamePropertiesAs(albumResult4.data); 
  })  
 
  it('Gets labels with DateReleasedMin filter', async () => {
    //just call with default valueateAlbum
   
    const albumResult1 = await createAlbum(context, {DateReleased: '2005-05-05'}, entityMap)
    const albumResult2 = await createAlbum(context, {DateReleased: '2005-05-06'}, entityMap)
    const albumResult3 = await createAlbum(context, {DateReleased: '2005-05-07'}, entityMap) 
    const albumResult4 = await createAlbum(context, {DateReleased: '2005-05-08'}, entityMap) 

    const searchModel : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id, albumResult4.data.Id],
      DateReleasedMin: albumResult3.data.DateReleased
    }
 
    const albumsRetrieved = await searchAlbums(context, searchModel); 

    expect(albumsRetrieved.status).toEqual(200);

    expect(albumsRetrieved.data.Items.length).toEqual(2);

    sortByKey(albumsRetrieved.data.Items, 'createdAt');
      
    expect(albumsRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumResult3.data); 
    expect(albumsRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumResult4.data); 
  })  

  it('Gets labels with DateReleasedMax filter', async () => {
    //just call with default valueateAlbum
   
    const albumResult1 = await createAlbum(context, {DateReleased: '2005-05-05'}, entityMap)
    const albumResult2 = await createAlbum(context, {DateReleased: '2005-05-06'}, entityMap)
    const albumResult3 = await createAlbum(context, {DateReleased: '2005-05-07'}, entityMap) 
    const albumResult4 = await createAlbum(context, {DateReleased: '2005-05-08'}, entityMap) 

    const searchModel : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id, albumResult4.data.Id],
      DateReleasedMax: albumResult3.data.DateReleased
    }
 
    const albumsRetrieved = await searchAlbums(context, searchModel); 

    expect(albumsRetrieved.status).toEqual(200);

    expect(albumsRetrieved.data.Items.length).toEqual(3);

    sortByKey(albumsRetrieved.data.Items, 'createdAt');
      
    expect(albumsRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumResult1.data);  
    expect(albumsRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumResult2.data);  
    expect(albumsRetrieved.data.Items[2]).toHaveSamePropertiesAs(albumResult3.data);  
  })  

  it('Gets labels with DateReleasedMax filter', async () => {
    //just call with default valueateAlbum
   
    const albumResult1 = await createAlbum(context, {DateReleased: '2005-05-05'}, entityMap)
    const albumResult2 = await createAlbum(context, {DateReleased: '2005-05-06'}, entityMap)
    const albumResult3 = await createAlbum(context, {DateReleased: '2005-05-07'}, entityMap) 
    const albumResult4 = await createAlbum(context, {DateReleased: '2005-05-08'}, entityMap) 

    const searchModel : AlbumSearchModel = {
      Ids: [albumResult1.data.Id, albumResult2.data.Id, albumResult3.data.Id, albumResult4.data.Id],
      DateReleasedMin: albumResult2.data.DateReleased,
      DateReleasedMax: albumResult3.data.DateReleased
    }
 
    const albumsRetrieved = await searchAlbums(context, searchModel); 

    expect(albumsRetrieved.status).toEqual(200);

    expect(albumsRetrieved.data.Items.length).toEqual(2);

    sortByKey(albumsRetrieved.data.Items, 'createdAt');
      
    expect(albumsRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumResult2.data);  
    expect(albumsRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumResult3.data);   
  })  
})


