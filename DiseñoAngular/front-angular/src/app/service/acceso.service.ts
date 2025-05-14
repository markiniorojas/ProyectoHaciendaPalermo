import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ILogin } from '../interface/login';
import { Router } from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import { environment } from '../../environments/environment.development';


@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = environment.apiURL;

  constructor(private http: HttpClient, private router: Router) {}

  login(dto: ILogin, password?: any) {
    return this.http.post<{ isSucces: boolean; token: string }>(this.baseUrl, dto);
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

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const { exp }: any = jwtDecode(token);
      const now = Math.floor(Date.now() / 1000);
      return now > exp;
    } catch (e) {
      return true;
    }
  }
}
