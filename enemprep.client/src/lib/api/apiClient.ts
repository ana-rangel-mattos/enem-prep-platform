import axios, { AxiosInstance, AxiosResponse } from "axios";

export class ApiClient {
  protected readonly instance: AxiosInstance;

  constructor(baseURL: string) {
    this.instance = axios.create({
      baseURL,
      timeout: 10000,
      withCredentials: true,
    });
  }

  protected responseBody<T>(response: AxiosResponse<T>): T {
    return response.data;
  }

  protected async get<T>(url: string, params?: object): Promise<T> {
    return await this.instance.get<T>(url, { params }).then(this.responseBody);
  }

  protected async post<T>(url: string, body?: object): Promise<T> {
    return await this.instance.post<T>(url, body).then(this.responseBody);
  }

  protected async put<T>(url: string, body?: object): Promise<T> {
    return await this.instance.put<T>(url, body).then(this.responseBody);
  }

  protected async delete<T>(url: string, params?: object): Promise<T> {
    return await this.instance
      .delete<T>(url, { params })
      .then(this.responseBody);
  }
}
