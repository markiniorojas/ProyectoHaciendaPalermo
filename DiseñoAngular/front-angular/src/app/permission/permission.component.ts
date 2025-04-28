import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IPermission } from '../interface/ipermission';
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
  templateUrl: './permission.component.html',
  styleUrls: ['./permission.component.css']
})
export class PermissionComponent implements OnInit {
  permissions: IPermission[] = [];
  currentPermission: IPermission = this.getEmptyPermission();
  showForm: boolean = false;
  isEditing: boolean = false;
  
  constructor(private permissionService: ServiceGeneralService) {}

  ngOnInit(): void {
    this.loadPermission();
  }

  // Helper para crear un formulario vacío
  getEmptyPermission(): IPermission {
    return {
      id: 0,
      name:  '',
      description: '',
      isDeleted: false
    };
  }

  loadPermission(): void {
    this.permissionService.get<IPermission[]>('permission').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        // Solo formularios que NO estén eliminados (IsDeleted == false)
        this.permissions = data.filter(permission => permission.isDeleted === false);
      },
      error: err => console.error('Error al cargar los permisos', err),
    });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitPermission(): void {
    if (this.isEditing) {
      this.updatePermission();
    } else {
      this.addPermission();
    }
  }

  addPermission(): void {
    this.permissionService.post<IPermission>('permission', this.currentPermission).subscribe({
      next: permission => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'permiso creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.permissions.push(permission);
        this.resetPermission();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el permiso'
        });
        console.error('Error al agregar permiso', err);
      }
    });
  }
  

  editPermission(permission: IPermission): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentPermission);
    this.currentPermission = { ...permission };
    this.showForm = true;
  }

  updatePermission(): void {
    console.log(this.currentPermission);
    this.permissionService.put<IPermission>('permission', this.currentPermission.id, this.currentPermission).subscribe({
      next: updatedPermission => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'permiso actualizado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        const index = this.permissions.findIndex(pr => pr.id === updatedPermission.id);
        if (index > -1) this.permissions[index] = updatedPermission;
        this.resetPermission();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el permiso'
        });
        console.error('Error al actualizar el permiso', err);
      }
    });
  }

  deletePermission(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el permiso!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.permissionService.delete<IPermission>('permission', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El permiso ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.permissions = this.permissions.filter(pe => pe.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el permiso'
            });
            console.error('Error al eliminar permiso', err);
          }
        });
      }
    });
  }

  deletePermissionLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este permiso se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.permissionService.deleteLogic<IPermission>('permission', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El permiso ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.permissions = this.permissions.filter(m => m.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el permiso'
            });
            console.error('Error al eliminar lógicamente', err);
          }
        });
      }
    });
  }

  togglePermission(permission: 'create' | 'edit'): void {
    if (permission === 'create') {
      this.isEditing = false;
      this.currentPermission = this.getEmptyPermission();
    }
    this.showForm = !this.showForm;
  }

  cancelPermission(): void {
    if (this.currentPermission.name || this.currentPermission.description) {
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
          this.resetPermission();
        }
      });
    } else {
      this.resetPermission();
    }
  }

  resetPermission(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentPermission = this.getEmptyPermission();
  }
}

