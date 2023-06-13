import { AxiosRequestConfig, AxiosResponse, AxiosError } from "axios"
import axios from 'axios'

export const qapost = async function (url: string, body: Record<string, any>, config?: AxiosRequestConfig): Promise<AxiosResponse> {

    let response;

    try {
        response = await axios.post(url, body, config);
    } catch (ex) {
        if (ex instanceof AxiosError && ex.response) {
            response = ex.response;
        }
        else {
            throw ex;
        }
    }

    return response;
}

export const qaget = async function (url: string, config?: AxiosRequestConfig): Promise<AxiosResponse> {

    let response;

    try {
        response = await axios.get(url, config);
    } catch (ex) {
        if (ex instanceof AxiosError && ex.response) {
            response = ex.response;
        }
        else {
            throw ex;
        }
    }

    return response;
}

export const qapatch = async function (url: string, body: Record<string, any>, config?: AxiosRequestConfig): Promise<AxiosResponse> {

    let response;

    try {
        response = await axios.patch(url, body, config);
    } catch (ex) {
        if (ex instanceof AxiosError && ex.response) {
            response = ex.response;
        }
        else {
            throw ex;
        }
    }

    return response;
}

export const qadelete = async function (url: string, config?: AxiosRequestConfig): Promise<AxiosResponse> {

    let response;

    try {
        response = await axios.delete(url, config);
    } catch (ex) {
        if (ex instanceof AxiosError && ex.response) {
            response = ex.response;
        }
        else {
            throw ex;
        }
    }

    return response;
}
