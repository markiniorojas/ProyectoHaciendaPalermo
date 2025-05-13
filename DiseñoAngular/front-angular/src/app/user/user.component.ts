import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IUser } from '../interface/iuser';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import Swal from 'sweetalert2';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';



@Component({
  selector: 'app-form-example',
  standalone: true,
  imports: [CommonModule,FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSlideToggleModule,
    MatChipsModule,
    MatTooltipModule,
    MatDatepickerModule,
    MatNativeDateModule
   ],
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  users: IUser[] = [];
  currentUser: IUser = this.getEmptyUser();
  showForm: boolean = false;
  isEditing: boolean = false;
  
  constructor(private userService: ServiceGeneralService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  // Helper para crear un formulario vacío
  getEmptyUser(): IUser {
    return {
      id: 0,
      email:  '',
      password:  '',
      isDeleted:  false,
      registrationDate: new Date(),
      namePerson:  ''
    };
  }

  loadUser(): void {
    this.userService.get<IUser[]>('user').subscribe({
      next: data => {

        // Solo formularios que NO estén eliminados (IsDeleted == false)
        this.users = data.filter(user => user.isDeleted === false);
      },
      error: err => console.error('Error al cargar los usuarios', err),
    });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitUser(): void {
    if (this.isEditing) {
      this.updateUser();
    } else {
      this.addUser();
    }
  }

  addUser(): void {
    this.userService.post<IUser>('user', this.currentUser).subscribe({
      next: user => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Usuario creada correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.users.push(user);
        this.resetUser();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el usuario'
        });
        console.error('Error al agregar el usuario', err);
      }
    });
  }
  

  editUser(user: IUser): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentForm);
    this.currentUser = { ...user };
    this.showForm = true;
  }

  updateUser(): void {
    this.userService.put<IUser>('User', this.currentUser).subscribe({
      next: () => {
        this.loadUser(); // Recargar todo para mantener la consistencia
        this.resetUser();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar los datos del usuario'
        });
        console.error('Error al actualizar el usuario', err);
      }
    });
  }

  deleteUser(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el usuario!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.userService.delete<IUser>('user', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El usuario ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.users = this.users.filter(u => u.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el usuario'
            });
            console.error('Error al eliminar el usuario', err);
          }
        });
      }
    });
  }

  deleteUserLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este usuario se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.userService.deleteLogic<IUser>('user', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El usuario ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.users = this.users.filter(u => u.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el usuario'
            });
            console.error('Error al eliminar el usuario', err);
          }
        });
      }
    });
  }

  toggleUser(user: 'create' | 'edit'): void {
    if (user === 'create') {
      this.isEditing = false;
      this.currentUser = this.getEmptyUser();
    }
    this.showForm = !this.showForm;
  }

  cancelUser(): void {
    if (this.currentUser.email || this.currentUser.password ) {
      Swal.fire({
        title: '¿Estás seguro?',
        text: 'Perderás los cambios no guardados',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, cancelar',
        cancelButtonText: 'Seguir editando'
      }).then((result) => {
        if (result.isConfirmed) {
          this.resetUser();
        }
      });
    } else {
      this.resetUser();
    }
  }

  resetUser(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentUser = this.getEmptyUser();
  }
}

