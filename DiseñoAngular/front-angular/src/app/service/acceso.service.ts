import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
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

  loginWithGoogle(idToken: string): Observable<any> {
    const url = `${this.baseUrl}/Acceso/google`; 
    return this.http.post(url, { token: idToken });
  }

  saveToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    const token = localStorage.getItem('auth_token');
    if (token && !this.isTokenExpired(token)) {
      return token;
    }
    return null;
  }

  isTokenExpired(token: string): boolean {
    try {
      const decode: any = jwtDecode(token);
      const expiration = decode.exp;
      return Date.now() > expiration * 1000;
    } catch {
      return true; 
    }
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token;
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  decodeToken(token: string): any {
  try {
    return jwtDecode(token);
  } catch (error) {
    console.error('Error al decodificar el token', error);
    return null;
  }
}
}
