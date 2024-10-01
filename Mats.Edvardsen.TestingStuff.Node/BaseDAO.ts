import {useFetch} from 'nuxt/app';
import type {UseFetchOptions} from 'nuxt/app';

function getRequestOptions<T>(method: 'GET' | 'POST' | 'PUT' | 'DELETE', body?: T): UseFetchOptions<any> {
    const options: UseFetchOptions<any> = {
        method,
        headers: {
            'Content-Type': 'application/json',
        },
    };

    if (body) {
        options.body = JSON.stringify(body);
    }
    return options;
}

class BaseDAO<T extends object, RequestT> {
    protected baseUrl: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    async GetAll(): Promise<T[]> {
        const result = await useFetch<T[]>(this.baseUrl, getRequestOptions('GET'));
        return result.data.value ?? [];
    }
    async GetById(id: string): Promise<T | null> {
        const result = await useFetch<T | null>(`${this.baseUrl}/${id}`, getRequestOptions('GET'));
        return result.data.value as (T | null);
    }

    async Add(item: RequestT): Promise<T> {
        const {data} = await useFetch(this.baseUrl, getRequestOptions<RequestT>('POST', item));
        return data?.value;
    }

    async Update(id: string, item: R): Promise<void> {
        await useFetch(`${this.baseUrl}/${id}`, getRequestOptions('PUT', item));
    }

    async Delete(id: string): Promise<void> {
        await useFetch(`${this.baseUrl}/${id}`, getRequestOptions('DELETE'));
    }

}