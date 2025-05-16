import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../service/acceso.service';
import { Router } from '@angular/router';

declare const google: any;

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  hidePassword = true;
  
  constructor(
    private fb: FormBuilder,
    private router: Router, 
    private auth: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    google.accounts.id.initialize({
      client_id: '1087692292488-3d3el4h3nqei8ndmk8jgnt6b9sclf0i1.apps.googleusercontent.com',
      callback: (response: any) => {
        const idToken = response.credential;
        this.auth.loginWithGoogle(idToken).subscribe({
          next: (res) => {
            this.auth.saveToken(res.token);
            this.router.navigate(['/principal']);
          },
          error: (err) => {
            console.error('Error con Google Login:', err);
            alert('Error al iniciar sesión con Google.');
          }
        });
      }
    });

    // Asegúrate de renderizar el botón correctamente con el tamaño y tema adecuados
    setTimeout(() => {
      google.accounts.id.renderButton(
        document.getElementById("googleSignInDiv"),
        { 
          theme: "outline", 
          size: "large",
          width: 250,
          text: "signin_with" // Muestra "Iniciar sesión con Google"
        }
      );
    }, 100);
  }
  
  onSubmit() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;

      this.auth.login(email, password).subscribe({
        next: (res) => {
          if (res.isSucces && res.token) {
            this.auth.saveToken(res.token);
            console.log('Login exitoso. Token guardado.');
            this.router.navigate(['/principal']);
          }
        },
        error: (err) => {
          console.error('Error al iniciar sesión:', err);
          alert('Credenciales inválidas o error en el servidor.');
        }
      });
    }
  }
}