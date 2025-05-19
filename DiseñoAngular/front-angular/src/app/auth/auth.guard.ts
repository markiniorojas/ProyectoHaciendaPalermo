import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Router } from '@angular/router';
import { AuthService } from '../service/acceso.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild {

  constructor(private auth: AuthService, private router: Router) {}

  private checkAuth(): boolean {
    if (!this.auth.isAuthenticated()) {
      alert('Tu sesión ha expirado. Por favor inicia sesión nuevamente.');
      this.auth.logout();
      return false;
    }
    return true;
  }

  canActivate(): boolean {
    return this.checkAuth();
  }

  canActivateChild(): boolean {
    return this.checkAuth();
  }
}
