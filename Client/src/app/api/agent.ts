import axios, { AxiosError, AxiosRequestHeaders, AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import { Activity, ActivityFormValues } from '../models/activity';
import { toast } from 'react-toastify';
import { router } from '../router/Routes';
import { store } from '../stores/store';
import { User, UserFormValues } from '../models/user';

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    })
}

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.response.use(async (response) => {
    await sleep(1000);
    return response;
}, (error: AxiosError) => {
    if (!error.response) {
        return Promise.reject(error);
    }

    const { data, status, config }: AxiosResponse = error.response;

    switch (status) {
        case 400:
            if (config.method === "get" && data.errors.hasOwnProperty("id")) {
                router.navigate("/not-found");
            }

            if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key]);
                    }
                }
                throw modalStateErrors.flat();
            } else {
                toast.error(data);
            }
            break;
        case 401:
            toast.error("Unauthorized")
            break;
        case 403:
            toast.error("Forbidden")
            break;
        case 404:
            router.navigate('/not-found');
            toast.error("Not Found")
            break;
        case 500:
            store.commonStore.setServerError(data);
            router.navigate("/server-error");
            break;
    }

    return Promise.reject(error);
})

axios.interceptors.request.use((config: InternalAxiosRequestConfig) => {
    const token = store.commonStore.token;
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const request = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Activities = {
    list: () => request.get<Activity[]>('/activities'),
    details: (id: string) => request.get<Activity>(`/activities/${id}`),
    create: (activitiy: ActivityFormValues) => request.post<void>('/activities', activitiy),
    update: (activity: ActivityFormValues) => request.put<void>('/activities', activity),
    delete: (id: string) => request.delete<void>(`/activities/${id}`),
    attend: (id: string) => request.post<void>(`/activities/${id}/attend`, {})
}

const Accounts = {
    current: () => request.get<User>('/account'),
    login: (user: UserFormValues) => request.post<User>("/account/login", user),
    register: (user: UserFormValues) => request.post<User>("/account/register", user)
}

const agent = {
    Activities,
    Accounts
}

export default agent;