import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IRol } from '../interface/irol';
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
  templateUrl: './rol.component.html',
  styleUrls: ['./rol.component.css']
})
export class RolComponent implements OnInit {
  rols: IRol[] = [];
  currentRol: IRol = this.getEmptyRol();
  showForm: boolean = false;
  isEditing: boolean = false;
  
  constructor(private rolService: ServiceGeneralService) {}

  ngOnInit(): void {
    this.loadRol();
  }

  // Helper para crear un formulario vacío
  getEmptyRol(): IRol {
    return {
      id: 0,
      name:  '',
      description: '',
      isDeleted: false
    };
  }

  loadRol(): void {
    this.rolService.get<IRol[]>('rol').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        // Solo formularios que NO estén eliminados (IsDeleted == false)
        this.rols = data.filter(rol => rol.isDeleted === false);
      },
      error: err => console.error('Error al cargar los roles', err),
    });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitRol(): void {
    if (this.isEditing) {
      this.updateRol();
    } else {
      this.addRol();
    }
  }

  addRol(): void {
    this.rolService.post<IRol>('rol', this.currentRol).subscribe({
      next: rol => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'rol creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.rols.push(rol);
        this.resetRol();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el rol'
        });
        console.error('Error al agregar rol', err);
      }
    });
  }
  

  editRol(rol: IRol): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentPermission);
    this.currentRol = { ...rol };
    this.showForm = true;
  }

  updateRol(): void {
    this.rolService.put<IRol>('Rol', this.currentRol).subscribe({
      next: () => {
        this.loadRol();
        this.resetRol();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el rol'
        });
        console.error('Error al actualizar el rol', err);
      }
    });
  }

  deleteRol(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el rol!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.rolService.delete<IRol>('rol', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El rol ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.rols = this.rols.filter(r => r.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el rol'
            });
            console.error('Error al eliminar rol', err);
          }
        });
      }
    });
  }

  deleteRolLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este rol se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.rolService.deleteLogic<IRol>('rol', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El rol ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.rols = this.rols.filter(r => r.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el rol'
            });
            console.error('Error al eliminar lógicamente', err);
          }
        });
      }
    });
  }

  togglePermission(rol: 'create' | 'edit'): void {
    if (rol === 'create') {
      this.isEditing = false;
      this.currentRol = this.getEmptyRol();
    }
    this.showForm = !this.showForm;
  }

  cancelRol(): void {
    if (this.currentRol.name || this.currentRol.description) {
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
          this.resetRol();
        }
      });
    } else {
      this.resetRol();
    }
  }

  resetRol(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentRol = this.getEmptyRol();
  }
}

