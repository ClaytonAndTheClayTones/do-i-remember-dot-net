import { EntityMapItem, PagingRequestInfo, TestEntityMap, convertObjectToQueryArgs } from '../../QDK/common';
import { MusixApiContext } from '../../QDK/contexts'; 
import { qadelete, qaget, qapatch, qapost } from '../../QDK/qaxios';
import { AxiosRequestConfig, AxiosResponse } from 'axios'; 
import { AlbumCreateModel, createAlbum } from './albums';
import { ArtistCreateModel, createArtist } from './artists';


export type AlbumArtistLinkCreateModel = { 
    AlbumId?: string,
    Album?: Partial<AlbumCreateModel>,
    ArtistId?: string,
    Artist?: Partial<ArtistCreateModel> 
} 

export type AlbumArtistLinkUpdateModel = {
    AlbumId?: string, 
    ArtistId?: string
}
 
export type AlbumArtistLinkSearchModel = PagingRequestInfo & {
    Ids?: string[],
    AlbumIds? : string[],
    ArtistIds? : string[],
}
 
export const mintDefaultAlbumArtistLink = async function (musixContext: MusixApiContext, overrides: Partial<AlbumArtistLinkCreateModel> = {}, testEntityMap? : TestEntityMap, axiosConfig?: AxiosRequestConfig): Promise<Partial<AlbumArtistLinkCreateModel>> {
    const defaultAlbumArtistLink: AlbumArtistLinkCreateModel = {
 
    }

    if(!overrides.AlbumId)
    {
        const newAlbum = await createAlbum(musixContext, overrides.Album, testEntityMap, axiosConfig)

        overrides.AlbumId = newAlbum.data.Id; 
    }

    if(!overrides.ArtistId)
    {
        const newArtist = await createArtist(musixContext, overrides.Artist, testEntityMap, axiosConfig)

        overrides.ArtistId = newArtist.data.Id; 
    }


    Object.assign(defaultAlbumArtistLink, overrides);

    return defaultAlbumArtistLink;
}

export const createAlbumArtistLink = async function (musixContext: MusixApiContext, overrides: Partial<AlbumArtistLinkCreateModel> = {}, testEntityMap?: TestEntityMap, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string,any>>> {
     
    const albumArtistLinkToPost = await mintDefaultAlbumArtistLink(musixContext, overrides, testEntityMap, axiosConfig);

    const result = await qapost(musixContext.url + "/albumArtistLinks", albumArtistLinkToPost, axiosConfig);

    if(result.status === 201 && testEntityMap)
    {
        const entityMapItem : EntityMapItem = {
            context : musixContext,
            id: result.data.Id,
            deleteMethod : deleteAlbumArtistLink,
            entityName: "albumArtistLink"
        }; 

        testEntityMap.entities.push( entityMapItem );
    }
   
    if(!allowFailures)
    {
        expect(result.status).toEqual(201);
        expect(result.data).toHaveSamePropertiesAs(albumArtistLinkToPost, ["Id", "CreatedAt", "UpdatedAt"]);
        expect(result.data.Id).toBeTruthy(); 
        expect(result.data.CreatedAt).toBeTruthy();
    }

    return result;
}

export const getAlbumArtistLinkById = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qaget(musixContext.url + "/albumArtistLinks/" + id, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id);
    }

    return result;
}

export const searchAlbumArtistLinks = async function (musixContext: MusixApiContext, searchModel?: AlbumArtistLinkSearchModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const queryArgs = convertObjectToQueryArgs(searchModel || {})

    const result = await qaget(musixContext.url + "/albumArtistLinks" + queryArgs, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
    }

    return result;
}

export const updateAlbumArtistLink = async function (musixContext: MusixApiContext, id: string, updateModel: AlbumArtistLinkUpdateModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qapatch(musixContext.url + "/albumArtistLinks/" + id, updateModel, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id); 
    }

    return result;
}


export const deleteAlbumArtistLink = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string, any>>> {
      
    const url = musixContext.url + "/albumArtistLinks/" + id;

    const result = await qadelete(url, axiosConfig);
  
    if(!allowFailures)
    {
        expect(result.status).toEqual(200); 
        expect(result.data.Id).toEqual(id);
    }

    return result;
}