import { EntityMapItem, PagingRequestInfo, TestEntityMap, convertObjectToQueryArgs } from '../../QDK/common';
import { MusixApiContext } from '../../QDK/contexts'; 
import { qadelete, qaget, qapatch, qapost } from '../../QDK/qaxios';
import { AxiosRequestConfig, AxiosResponse } from 'axios';
import { generate } from 'randomstring';
import { LocationCreateModel, createLocation } from './locations';
import { LabelCreateModel, createLabel } from './labels';


export type ArtistCreateModel = {
    Name: string,
    CurrentLocationId?: string,
    CurrentLocation?: Partial<LocationCreateModel>
    CurrentLabelId?: string,
    CurrentLabel?: Partial<LabelCreateModel>
    DateFounded: string, 
    DateDisbanded?: string
} 

export type ArtistUpdateModel = {
    Name?: string,
    CurrentLocationId?: string,
    CurrentLabelId?: string,
    DateFounded?: string, 
    DateDisbanded?: string 
}
 
export type ArtistSearchModel = PagingRequestInfo & {
    Ids?: string[],
    NameLike?: string,
    City?: string,
    State?: string
}
 
export const mintDefaultArtist = async function (musixContext: MusixApiContext, overrides: Partial<ArtistCreateModel> = {}, testEntityMap? : TestEntityMap, axiosConfig?: AxiosRequestConfig): Promise<Partial<ArtistCreateModel>> {
    const defaultArtist: ArtistCreateModel = {
        Name: "testName:" + generate(12),
        DateFounded: "2001-01-01",
        DateDisbanded: "2002-02-02"
    }

    if(!overrides.CurrentLocationId)
    {
        const newLocation = await createLocation(musixContext, overrides.CurrentLocation, testEntityMap, axiosConfig)

        overrides.CurrentLocationId = newLocation.data.Id; 
    }

    if(!overrides.CurrentLabelId)
    {
        const newLabel = await createLabel(musixContext, overrides.CurrentLabel, testEntityMap, axiosConfig)

        overrides.CurrentLabelId = newLabel.data.Id; 
    }


    Object.assign(defaultArtist, overrides);

    return defaultArtist;
}

export const createArtist = async function (musixContext: MusixApiContext, overrides: Partial<ArtistCreateModel> = {}, testEntityMap?: TestEntityMap, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string,any>>> {
     
    const artistToPost = await mintDefaultArtist(musixContext, overrides, testEntityMap, axiosConfig);

    const result = await qapost(musixContext.url + "/artists", artistToPost, axiosConfig);

    if(result.status === 201 && testEntityMap)
    {
        const entityMapItem : EntityMapItem = {
            context : musixContext,
            id: result.data.Id,
            deleteMethod : deleteArtist,
            entityName: "artist"
        }; 

        testEntityMap.entities.push( entityMapItem );
    }
   
    if(!allowFailures)
    {
        expect(result.status).toEqual(201);
        expect(result.data).toHaveSamePropertiesAs(artistToPost, ["Id", "CreatedAt", "UpdatedAt"]);
        expect(result.data.Id).toBeTruthy(); 
        expect(result.data.CreatedAt).toBeTruthy();
    }

    return result;
}

export const getArtistById = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qaget(musixContext.url + "/artists/" + id, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id);
    }

    return result;
}

export const searchArtists = async function (musixContext: MusixApiContext, searchModel?: ArtistSearchModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const queryArgs = convertObjectToQueryArgs(searchModel || {})

    const result = await qaget(musixContext.url + "/artists" + queryArgs, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
    }

    return result;
}

export const updateArtist = async function (musixContext: MusixApiContext, id: string, updateModel: ArtistUpdateModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qapatch(musixContext.url + "/artists/" + id, updateModel, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id); 
    }

    return result;
}


export const deleteArtist = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string, any>>> {
      
    const url = musixContext.url + "/artists/" + id;

    const result = await qadelete(url, axiosConfig);
  
    if(!allowFailures)
    {
        expect(result.status).toEqual(200); 
        expect(result.data.Id).toEqual(id);
    }

    return result;
}