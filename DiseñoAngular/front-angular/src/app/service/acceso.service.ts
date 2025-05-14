import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ILogin } from '../interface/login';
import { Router } from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = environment.apiURL;

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<any> {
     const url = `${this.baseUrl}/Acceso`; 
     return this.http.post(url, { email, password });
   }

  saveToken(token: string) {
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.router.navigate(['/login']);
  }

  isTokenExpired(token : string ): boolean {
    try{
       const decode:any = jwtDecode(token)
       const expiracion = decode.exp;
       return Date.now() < expiracion * 1000;

     } catch{
       return false
     }
  }
}

