import { EntityMapItem, PagingRequestInfo, TestEntityMap, convertObjectToQueryArgs } from '../../QDK/common';
import { MusixApiContext } from '../../QDK/contexts'; 
import { qadelete, qaget, qapatch, qapost } from '../../QDK/qaxios';
import { AxiosRequestConfig, AxiosResponse } from 'axios';
import { generate } from 'randomstring'; 
import { LabelCreateModel, createLabel } from './labels';


export type AlbumCreateModel = {
    Name: string, 
    LabelId?: string,
    Label?: Partial<LabelCreateModel>
    DateReleased: string 
} 

export type AlbumUpdateModel = {
    Name?: string, 
    LabelId?: string,
    DateReleased?: string   
}
 
export type AlbumSearchModel = PagingRequestInfo & {
    Ids?: string[], 
    LabelIds? : string[],
    
    NameLike?: string,

    DateReleasedMin?: string,
    DateReleasedMax?: string

}
 
export const mintDefaultAlbum = async function (musixContext: MusixApiContext, overrides: Partial<AlbumCreateModel> = {}, testEntityMap? : TestEntityMap, axiosConfig?: AxiosRequestConfig): Promise<Partial<AlbumCreateModel>> {
    const defaultAlbum: AlbumCreateModel = {
        Name: "testName:" + generate(12),
        DateReleased: "2001-01-01", 
    } 

    if(!overrides.LabelId)
    {
        const newLabel = await createLabel(musixContext, overrides.Label, testEntityMap, axiosConfig)

        overrides.LabelId = newLabel.data.Id; 
    }


    Object.assign(defaultAlbum, overrides);

    return defaultAlbum;
}

export const createAlbum = async function (musixContext: MusixApiContext, overrides: Partial<AlbumCreateModel> = {}, testEntityMap?: TestEntityMap, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string,any>>> {
     
    const albumToPost = await mintDefaultAlbum(musixContext, overrides, testEntityMap, axiosConfig);

    const result = await qapost(musixContext.url + "/albums", albumToPost, axiosConfig);

    if(result.status === 201 && testEntityMap)
    {
        const entityMapItem : EntityMapItem = {
            context : musixContext,
            id: result.data.Id,
            deleteMethod : deleteAlbum,
            entityName: "album"
        }; 

        testEntityMap.entities.push( entityMapItem );
    }
   
    if(!allowFailures)
    {
        expect(result.status).toEqual(201);
        expect(result.data).toHaveSamePropertiesAs(albumToPost, ["Id", "CreatedAt", "UpdatedAt"]);
        expect(result.data.Id).toBeTruthy(); 
        expect(result.data.CreatedAt).toBeTruthy();
    }

    return result;
}

export const getAlbumById = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qaget(musixContext.url + "/albums/" + id, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id);
    }

    return result;
}

export const searchAlbums = async function (musixContext: MusixApiContext, searchModel?: AlbumSearchModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const queryArgs = convertObjectToQueryArgs(searchModel || {})

    const result = await qaget(musixContext.url + "/albums" + queryArgs, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
    }

    return result;
}

export const updateAlbum = async function (musixContext: MusixApiContext, id: string, updateModel: AlbumUpdateModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qapatch(musixContext.url + "/albums/" + id, updateModel, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id); 
    }

    return result;
}


export const deleteAlbum = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string, any>>> {
      
    const url = musixContext.url + "/albums/" + id;

    const result = await qadelete(url, axiosConfig);
  
    if(!allowFailures)
    {
        expect(result.status).toEqual(200); 
        expect(result.data.Id).toEqual(id);
    }

    return result;
}