import { Component, ViewChild } from '@angular/core';
import { RouterLink, RouterOutlet, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenav } from '@angular/material/sidenav';
import { AuthService } from '../service/acceso.service';

@Component({
  selector: 'app-principal',
  imports: [CommonModule,RouterLink, 
    RouterOutlet,
    RouterLinkActive,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule

  ],
  templateUrl: './principal.component.html',
  styleUrl: './principal.component.css'
})
export class PrincipalComponent {
  constructor(private authService: AuthService, private router: Router) {}
  logout() {
    this.authService.logout(); // limpia el token

    // Navega y reemplaza la URL actual (evita volver con el botón atrás)
    this.router.navigate(['/login'], { replaceUrl: true });
  }
  @ViewChild('sidenav') sidenav!: MatSidenav;
  
  toggleSidenav() {
    this.sidenav.toggle();
  }
}
