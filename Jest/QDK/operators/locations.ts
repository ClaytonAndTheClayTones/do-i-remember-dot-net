import { generate } from 'randomstring';
import { EntityMapItem, PagingRequestInfo, TestEntityMap, convertObjectToQueryArgs } from '../../QDK/common';
import { MusixApiContext } from '../../QDK/contexts'; 
import { qadelete, qaget, qapatch, qapost } from '../../QDK/qaxios';
import { AxiosRequestConfig, AxiosResponse } from 'axios'; 


export type LocationCreateModel = { 
    City: string,
    State: string
} 

export type LocationUpdateModel = { 
    City?: string,
    State?: string
}
 
export type LocationSearchModel = PagingRequestInfo & {
    Ids?: string[], 
    CityOrStateLike?: string
}
 
export const mintDefaultLocation = async function (overrides: Partial<LocationCreateModel> = {}): Promise<Partial<LocationCreateModel>> {
    const defaultLocation: LocationCreateModel = { 
        City: "testCity:" + generate(12),
        State: "testState"
    }

    Object.assign(defaultLocation, overrides);

    return defaultLocation;
}

export const createLocation = async function (musixContext: MusixApiContext, overrides: Partial<LocationCreateModel> = {}, testEntityMap?: TestEntityMap, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string,any>>> {
     
    const locationToPost = await mintDefaultLocation(overrides);

    const result = await qapost(musixContext.url + "/locations", locationToPost, axiosConfig);

    if(result.status === 201 && testEntityMap)
    {
        const entityMapItem : EntityMapItem = {
            context : musixContext,
            id: result.data.Id,
            deleteMethod : deleteLocation,
            entityName: "location"
        }; 

        testEntityMap.entities.push( entityMapItem );
    }
   
    if(!allowFailures)
    {
        expect(result.status).toEqual(201);
        expect(result.data).toHaveSamePropertiesAs(locationToPost, ["Id", "CreatedAt", "UpdatedAt"]);
        expect(result.data.Id).toBeTruthy(); 
        expect(result.data.CreatedAt).toBeTruthy();
    }

    return result;
}

export const getLocationById = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qaget(musixContext.url + "/locations/" + id, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id);
    }

    return result;
}

export const searchLocations = async function (musixContext: MusixApiContext, searchModel?: LocationSearchModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const queryArgs = convertObjectToQueryArgs(searchModel || {})

    const result = await qaget(musixContext.url + "/locations" + queryArgs, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
    }

    return result;
}

export const updateLocation = async function (musixContext: MusixApiContext, id: string, updateModel: LocationUpdateModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qapatch(musixContext.url + "/locations/" + id, updateModel, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id); 
    }

    return result;
}


export const deleteLocation = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string, any>>> {
      
    const url = musixContext.url + "/locations/" + id;

    const result = await qadelete(url, axiosConfig);
  
    if(!allowFailures)
    {
        expect(result.status).toEqual(200); 
        expect(result.data.Id).toEqual(id);
    }

    return result;
}