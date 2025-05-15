import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './service/acceso.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  
})
export class AppComponent implements OnInit{

  constructor(private auth: AuthService, private router: Router){}

  ngOnInit(): void {
      setInterval(()=>{
        const token = this.auth.getToken();
        if (token && this.auth.isTokenExpired(token) == false)return;

        if(token){
          alert('Sesion expirada. Vuelva a iniciar sesion');
          this.auth.logout();
          this.router.navigate(['/login']);
        }

      }, 10000);
  }
}