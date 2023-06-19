import { AxiosRequestConfig, AxiosResponse } from "axios"
export type PagingRequestInfo = {
    Page? : number,
    PageLength? : number,
    SortBy? : string,
    IsDescending? : boolean
}

export class TestEntityMap {
    public entities: EntityMapItem[] = []

    async cleanup() {
        for (let i = 0; i < this.entities.length; i++) {
            const item = this.entities[i];

            const response = await item.deleteMethod(item.context, item.id, undefined, true);

            if (response.status != 200) {
                console.log(`Cleanup of ${item.entityName} entity with id ${item.id} was unsuccessful with status ${response.status}.  ${response.data}`)
            }
        }
    }
}
 
export type EntityMapItem = {
    entityName: string,
    id: string,
    context: any,
    deleteMethod: (context: any, id: string, axiosConfig?: AxiosRequestConfig, allowFailures?: boolean) => Promise<AxiosResponse<Record<string, any>>>;
}

export const isUUIDArray = (arr: string[]) => {
    for (const string of arr) {
        if (!isUUID(string)) {
            return false
        }
    }
    return true
}

export const isUUID = (string: string) => {
    const v4 = new RegExp(/^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-4[0-9A-Fa-f]{3}-[89ABab][0-9A-Fa-f]{3}-[0-9A-Fa-f]{12}$/i)

    return v4.test(string)
}

export interface UpdateExpressionParameters {
    updateExpression: string
    expressionAttributeNames: Record<string, any>
    expressionAttributeValues: Record<string, any>
}
 

/**
 * Sorts an array of objects by the key provided
 * @param {Array<Object>} array - The object array
 * @param {string} key - the key to sort by
 * @param {boolean} desc - shall we sort descending?
 * @param {boolean} ignoreCase - shall we ignore case for string comparisons
 */
export const sortByKey = function (array: any[], key: string, desc = false, ignoreCase = false): void {
    const compare = function compare(a: any, b: any): number {
        const x = ignoreCase && a[key] && a[key].toLowerCase ? a[key].toLowerCase() : a[key]
        const y = ignoreCase && b[key] && b[key].toLowerCase ? b[key].toLowerCase() : b[key]

        if (!desc) {
            if (x < y) {
                return -1
            }
            if (x > y) {
                return 1
            }
            return 0
        } else {
            if (x > y) {
                return -1
            }
            if (x < y) {
                return 1
            }
            return 0
        }
    }

    array.sort(compare)
}

export const mergeUnique = function<T>(array1 : T[], array2 : T[]) : T[]
{
    const stackedArray = [
        array1, array2
    ];

    const mergedUniqueResult  = [...new Set(stackedArray.flat())];

    return mergedUniqueResult; 
} 

export const convertObjectToQueryArgs = function(object :  Record<string, any>) : string
{
    let output = "";
    const keys = Object.keys(object);

    keys.forEach((key: string) => {
        if(object[key] != null)
        {
            if(output.length === 0)
            {
                output += "?";
            }
            else
            {
                output += "&"
            }

            if(Array.isArray(object[key]))
            {  
                if(object[key].length > 0)
                {
                    let value = "";

                    object[key].forEach((subvalue: any, index: number) =>{
                        if(index !== 0)
                        {
                            value += ",";
                        } 
                        
                        value += encodeURIComponent(subvalue)
                    
                    })
                    
                    output += `${key}=${value}`;
                }
            }
            else
            {
                output += `${key}=${encodeURIComponent(object[key])}`;
            }
        }
    })

    return output;

}
