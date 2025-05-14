import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../service/acceso.service'; // ajusta la ruta según tu proyecto

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private auth: AuthService, private router: Router) {}

  canActivate(): boolean {
    const token = this.auth.getToken();
    if (!token || this.auth.isTokenExpired(token)) {
      this.auth.logout();
      return false;
    }
    return true;
  }
}
