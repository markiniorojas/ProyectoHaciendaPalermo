import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { IResponseAcceso } from '../interface/iresponseacceso';
import { Observable } from 'rxjs';
import { IUser } from '../interface/iuser';
import { ILogin } from '../interface/login';

@Injectable({
  providedIn: 'root'
})
export class AccesoService {

  private http = inject(HttpClient);
  private baseUrl: string = environment.apiURL;
  constructor() { }

  registrarse(objeto:IUser): Observable<IResponseAcceso>{
    return this.http.post<IResponseAcceso>(`${this.baseUrl}Acceso/Registrar`, objeto);
  }

  login(objeto:ILogin): Observable<IResponseAcceso>{
    return this.http.post<IResponseAcceso>(`${this.baseUrl}Acceso/Login`, objeto);
  }
}
