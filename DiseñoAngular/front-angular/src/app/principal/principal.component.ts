import { Component, ViewChild } from '@angular/core';
import { RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenav } from '@angular/material/sidenav';

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
  @ViewChild('sidenav') sidenav!: MatSidenav;
  
  toggleSidenav() {
    this.sidenav.toggle();
  }
}
