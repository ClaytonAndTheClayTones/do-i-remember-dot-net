import { EntityMapItem, PagingRequestInfo, TestEntityMap, convertObjectToQueryArgs } from '../../QDK/common';
import { MusixApiContext } from '../../QDK/contexts'; 
import { qadelete, qaget, qapatch, qapost } from '../../QDK/qaxios';
import { AxiosRequestConfig, AxiosResponse } from 'axios';
import { generate } from 'randomstring';
import { LocationCreateModel, createLocation } from './locations';


export type LabelCreateModel = {
    CurrentLocationId?: string,
    CurrentLocation?: Partial<LocationCreateModel>,
    Name: string
} 

export type LabelUpdateModel = {
    CurrentLocationId?: string,
    Name?: string
}
 
export type LabelSearchModel = PagingRequestInfo & {
    Ids?: string[],
    CurrentLocationIds?: string[],
    NameLike?: string
}  
 
export const mintDefaultLabel = async function (musixContext: MusixApiContext, overrides: Partial<LabelCreateModel> = {}, testEntityMap? : TestEntityMap, axiosConfig?: AxiosRequestConfig): Promise<Partial<LabelCreateModel>> {
    const defaultLabel: LabelCreateModel = {
        Name: "testName" + generate(12), 
    }
 
    if(!overrides.CurrentLocationId)
    {
        const newLocation = await createLocation(musixContext, overrides.CurrentLocation, testEntityMap, axiosConfig)

        overrides.CurrentLocationId = newLocation.data.Id; 
    }

    Object.assign(defaultLabel, overrides);

    return defaultLabel;
}

export const createLabel = async function (musixContext: MusixApiContext, overrides: Partial<LabelCreateModel> = {}, testEntityMap?: TestEntityMap, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string,any>>> {
     
    const labelToPost = await mintDefaultLabel(musixContext, overrides, testEntityMap, axiosConfig);

    const result = await qapost(musixContext.url + "/labels", labelToPost, axiosConfig);

    if(result.status === 201 && testEntityMap)
    {
        const entityMapItem : EntityMapItem = {
            context : musixContext,
            id: result.data.Id,
            deleteMethod : deleteLabel,
            entityName: "label"
        }; 

        testEntityMap.entities.push( entityMapItem );
    }
   
    if(!allowFailures)
    {
        expect(result.status).toEqual(201);
        expect(result.data).toHaveSamePropertiesAs(labelToPost, ["Id", "CreatedAt", "UpdatedAt"]);
        expect(result.data.Id).toBeTruthy(); 
        expect(result.data.CreatedAt).toBeTruthy();
    }

    return result;
}

export const getLabelById = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qaget(musixContext.url + "/labels/" + id, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id);
    }

    return result;
}

export const searchLabels = async function (musixContext: MusixApiContext, searchModel?: LabelSearchModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const queryArgs = convertObjectToQueryArgs(searchModel || {})

    const result = await qaget(musixContext.url + "/labels" + queryArgs, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
    }

    return result;
}

export const updateLabel = async function (musixContext: MusixApiContext, id: string, updateModel: LabelUpdateModel, axiosConfig?: AxiosRequestConfig, allowFailures = false) : Promise<AxiosResponse<Record<string,any>>> {
      
    const result = await qapatch(musixContext.url + "/labels/" + id, updateModel, axiosConfig);
 
    if(!allowFailures)
    {
        expect(result.status).toEqual(200);
        expect(result.data.Id).toEqual(id); 
    }

    return result;
}


export const deleteLabel = async function (musixContext: MusixApiContext, id: string, axiosConfig?: AxiosRequestConfig, allowFailures: boolean = false) : Promise<AxiosResponse<Record<string, any>>> {
      
    const url = musixContext.url + "/labels/" + id;

    const result = await qadelete(url, axiosConfig);
  
    if(!allowFailures)
    {
        expect(result.status).toEqual(200); 
        expect(result.data.Id).toEqual(id);
    }

    return result;
}