 import { Injectable } from "@angular/core";
import { environment } from "../environments/environment.development";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ServiceGeneralService {
  private baseUrl = environment.apiURL;

  constructor(private http: HttpClient) {}

  get<T>(endpoint: string) {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`);
  }

  getById<T>(endpoint: string, id: number | string) {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}/${id}`);
  }

  post<T>(endpoint: string, data: any) {
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, data);
  }

  put<T>(endpoint: string, id: number | string, data: any) {
    return this.http.put<T>(`${this.baseUrl}/${endpoint}/${id}`, data);
  }

  delete<T>(endpoint: string, id: number | string) {
    return this.http.delete<T>(`${this.baseUrl}/${endpoint}/permanent/${id}`);
  }


  deleteLogic<T>(endpoint: string, id: number | string) {
    return this.http.put<T>(`${this.baseUrl}/${endpoint}/Logico/${id}`, {});
  }

  patchRestore<T>(endpoint: string, id: number | string, data: any) {
    return this.http.patch<T>(`${this.baseUrl}/${endpoint}/recuperarLogica/${id}`, data);
  }
}