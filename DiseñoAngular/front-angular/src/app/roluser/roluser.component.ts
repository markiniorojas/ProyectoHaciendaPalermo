import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IRolUser } from '../interface/iroluser';
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
    MatTooltipModule
   ],
  templateUrl: './roluser.component.html',
  styleUrls: ['./roluser.component.css']
})
export class RoluserComponent implements OnInit {
  rolusers: IRolUser[] = [];
  currentRolUser: IRolUser = this.getEmptyRolUser();
  showForm: boolean = false;
  isEditing: boolean = false;
  
  constructor(private roluserService: ServiceGeneralService) {}

  ngOnInit(): void {
    this.loadRolUser();
  }

  // Helper para crear un formulario vacío
  getEmptyRolUser(): IRolUser {
    return {
      id: 0,
      rolId:  0,
      userId: 0,
      rolName: '',
      isDeleted: false
    };
  }

  loadRolUser(): void {
    this.roluserService.get<IRolUser[]>('roluser').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        // Solo formularios que NO estén eliminados (IsDeleted == false)
        this.rolusers = data.filter(roluser => roluser.isDeleted === false);
      },
      error: err => console.error('Error al cargar los rolUsers', err),
    });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitRolUser(): void {
    if (this.isEditing) {
      this.updateRolUser();
    } else {
      this.addRolUser();
    }
  }

  addRolUser(): void {
    this.roluserService.post<IRolUser>('roluser', this.currentRolUser).subscribe({
      next: roluser => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'roluser creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.rolusers.push(roluser);
        this.resetRolUser();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el Rol User'
        });
        console.error('Error al agregar Rol User', err);
      }
    });
  }
  

  editRolUser(roluser: IRolUser): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentPermission);
    this.currentRolUser = { ...roluser };
    this.showForm = true;
  }

  updateRolUser(): void {
    console.log(this.currentRolUser);
    this.roluserService.put<IRolUser>('roluser', this.currentRolUser.id, this.currentRolUser).subscribe({
      next: updatedRolUser => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Rol User actualizado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        const index = this.rolusers.findIndex(ru => ru.id === updatedRolUser.id);
        if (index > -1) this.rolusers[index] = updatedRolUser;
        this.resetRolUser();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el Rol User'
        });
        console.error('Error al actualizar el Rol User', err);
      }
    });
  }

  deleteRolUser(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el Rol User!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.roluserService.delete<IRolUser>('roluser', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El Rol User ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.rolusers = this.rolusers.filter(ru => ru.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el Rol User'
            });
            console.error('Error al eliminar Rol User', err);
          }
        });
      }
    });
  }

  deleteRolUserLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este Rol User se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.roluserService.deleteLogic<IRolUser>('roluser', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El Rol User ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.rolusers = this.rolusers.filter(ru => ru.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el Rol User'
            });
            console.error('Error al eliminar lógicamente', err);
          }
        });
      }
    });
  }

  toggleRolUser(roluser: 'create' | 'edit'): void {
    if (roluser === 'create') {
      this.isEditing = false;
      this.currentRolUser = this.getEmptyRolUser();
    }
    this.showForm = !this.showForm;
  }

  cancelRolUser(): void {
    if (this.currentRolUser.rolId || this.currentRolUser.userId) {
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
          this.resetRolUser();
        }
      });
    } else {
      this.resetRolUser();
    }
  }

  resetRolUser(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentRolUser = this.getEmptyRolUser();
  }
}

