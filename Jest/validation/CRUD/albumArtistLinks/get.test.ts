import { MusixApiContext } from '../../../QDK/contexts'
import { TestEntityMap, sortByKey } from '../../../QDK/common'
import { AlbumArtistLinkSearchModel, createAlbumArtistLink, getAlbumArtistLinkById, searchAlbumArtistLinks } from '../../../QDK/operators/albumArtistLinks'
import { initConfig } from '../../../QDK/config'
import { generate } from 'randomstring' 

describe('Get AlbumArtistLink Tests', () => {
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

  it('Gets a posted albumArtistLink by id', async () => {
    //just call with default values
    const albumArtistLinkResult = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkRetrieved = await getAlbumArtistLinkById(context, albumArtistLinkResult.data.Id);

    expect(albumArtistLinkRetrieved.status).toEqual(200)
    expect(albumArtistLinkRetrieved.data).toHaveSamePropertiesAs(albumArtistLinkResult.data);
  })

  it('Gets a 404 when retrieving a non-existant albumArtistLink', async () => {
    //just call with default values 
    const albumArtistLinkRetrieved = await getAlbumArtistLinkById(context, "00000000-0000-0000-0000-000000000000", undefined, true);

    expect(albumArtistLinkRetrieved.status).toEqual(404) 
  })

  it('Gets albumArtistLinks with ids filter', async () => {
    //just call with default values
    const albumArtistLinkResult1 = await createAlbumArtistLink(context, {}, entityMap)
    await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult3 = await createAlbumArtistLink(context, {}, entityMap) 

    const searchModel : AlbumArtistLinkSearchModel = {
      Ids: [albumArtistLinkResult1.data.Id, albumArtistLinkResult3.data.Id]
    }

    const albumArtistLinksRetrieved = await searchAlbumArtistLinks(context, searchModel); 

    expect(albumArtistLinksRetrieved.status).toEqual(200);

    expect(albumArtistLinksRetrieved.data.Items.length).toEqual(2);

    expect(albumArtistLinksRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumArtistLinkResult1.data);
    expect(albumArtistLinksRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumArtistLinkResult3.data); 
  }) 
 
  it('Gets albumArtistLinks with paging', async () => {
    //just call with default values
    const albumArtistLinkResult1 = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult2 = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult3 = await createAlbumArtistLink(context, {}, entityMap) 
    const albumArtistLinkResult4 = await createAlbumArtistLink(context, {}, entityMap) 

    const searchModelPage1 : AlbumArtistLinkSearchModel = {
      Ids: [albumArtistLinkResult1.data.Id, albumArtistLinkResult2.data.Id, albumArtistLinkResult3.data.Id, albumArtistLinkResult4.data.Id],
      Page: 1,
      PageLength: 2
    };
 
    const searchModelPage2 : AlbumArtistLinkSearchModel = {
      Ids: [albumArtistLinkResult1.data.Id, albumArtistLinkResult2.data.Id, albumArtistLinkResult3.data.Id, albumArtistLinkResult4.data.Id],
      Page: 2,
      PageLength: 2
    };

    const albumArtistLinksRetrievedPage1 = await searchAlbumArtistLinks(context, searchModelPage1); 
    const albumArtistLinksRetrievedPage2 = await searchAlbumArtistLinks(context, searchModelPage2); 

    expect(albumArtistLinksRetrievedPage1.status).toEqual(200);
    expect(albumArtistLinksRetrievedPage2.status).toEqual(200);

    expect(albumArtistLinksRetrievedPage1.data.Items.length).toEqual(2);
    expect(albumArtistLinksRetrievedPage2.data.Items.length).toEqual(2);

    expect(albumArtistLinksRetrievedPage1.data.Items[0]).toHaveSamePropertiesAs(albumArtistLinkResult1.data);
    expect(albumArtistLinksRetrievedPage1.data.Items[1]).toHaveSamePropertiesAs(albumArtistLinkResult2.data); 
    expect(albumArtistLinksRetrievedPage2.data.Items[0]).toHaveSamePropertiesAs(albumArtistLinkResult3.data);
    expect(albumArtistLinksRetrievedPage2.data.Items[1]).toHaveSamePropertiesAs(albumArtistLinkResult4.data); 
  }) 
 
  it('Gets labels with albumIds filter', async () => {
    //just call with default valueateAlbumArtistLink
   
    const albumArtistLinkResult1 = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult2 = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult3 = await createAlbumArtistLink(context, {AlbumId : albumArtistLinkResult1.data.AlbumId}, entityMap) 
    const albumArtistLinkResult4 = await createAlbumArtistLink(context, {}, entityMap) 

    const searchModel : AlbumArtistLinkSearchModel = {
      Ids: [albumArtistLinkResult1.data.Id, albumArtistLinkResult2.data.Id, albumArtistLinkResult3.data.Id, albumArtistLinkResult4.data.Id],
      AlbumIds: [albumArtistLinkResult1.data.AlbumId, albumArtistLinkResult4.data.AlbumId]
    }

    const albumArtistLinksRetrieved = await searchAlbumArtistLinks(context, searchModel); 

    expect(albumArtistLinksRetrieved.status).toEqual(200);

    expect(albumArtistLinksRetrieved.data.Items.length).toEqual(3);

    sortByKey(albumArtistLinksRetrieved.data.Items, 'createdAt');
     
    expect(albumArtistLinksRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumArtistLinkResult1.data);
    expect(albumArtistLinksRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumArtistLinkResult3.data); 
    expect(albumArtistLinksRetrieved.data.Items[2]).toHaveSamePropertiesAs(albumArtistLinkResult4.data); 
  })  
 
  it('Gets labels with ArtistIds filter', async () => {
    //just call with default valueateAlbumArtistLink
   
    const albumArtistLinkResult1 = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult2 = await createAlbumArtistLink(context, {}, entityMap)
    const albumArtistLinkResult3 = await createAlbumArtistLink(context, {ArtistId : albumArtistLinkResult1.data.ArtistId}, entityMap) 
    const albumArtistLinkResult4 = await createAlbumArtistLink(context, {}, entityMap) 

    const searchModel : AlbumArtistLinkSearchModel = {
      Ids: [albumArtistLinkResult1.data.Id, albumArtistLinkResult2.data.Id, albumArtistLinkResult3.data.Id, albumArtistLinkResult4.data.Id],
      ArtistIds: [albumArtistLinkResult1.data.ArtistId, albumArtistLinkResult4.data.ArtistId]
    }

    const albumArtistLinksRetrieved = await searchAlbumArtistLinks(context, searchModel); 

    expect(albumArtistLinksRetrieved.status).toEqual(200);

    expect(albumArtistLinksRetrieved.data.Items.length).toEqual(3);

    sortByKey(albumArtistLinksRetrieved.data.Items, 'createdAt');
     
    expect(albumArtistLinksRetrieved.data.Items[0]).toHaveSamePropertiesAs(albumArtistLinkResult1.data);
    expect(albumArtistLinksRetrieved.data.Items[1]).toHaveSamePropertiesAs(albumArtistLinkResult3.data); 
    expect(albumArtistLinksRetrieved.data.Items[2]).toHaveSamePropertiesAs(albumArtistLinkResult4.data); 
  })   
})


