import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IModule } from '../interface/imodule';
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
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit {
  modules: IModule[] = [];
  currentModule: IModule = this.getEmptyModel();
  showForm: boolean = false;
  isEditing: boolean = false;
  
  constructor(private moduleService: ServiceGeneralService) {}

  ngOnInit(): void {
    this.loadModel();
  }

  // Helper para crear un formulario vacío
  getEmptyModel(): IModule {
    return {
      id: 0,
      name:  '',
      description: '',
      isDeleted: false
    };
  }

  loadModel(): void {
    this.moduleService.get<IModule[]>('module').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        // Solo formularios que NO estén eliminados (IsDeleted == false)
        this.modules = data.filter(module => module.isDeleted === false);
      },
      error: err => console.error('Error al cargar los modulos', err),
    });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitModel(): void {
    if (this.isEditing) {
      this.updateModule();
    } else {
      this.addModel();
    }
  }

  addModel(): void {
    this.moduleService.post<IModule>('module', this.currentModule).subscribe({
      next: module => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'modulo creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.modules.push(module);
        this.resetModel();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el modulo'
        });
        console.error('Error al agregar modulo', err);
      }
    });
  }
  

  editModel(module: IModule): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentModule);
    this.currentModule = { ...module };
    this.showForm = true;
  }

  updateModule(): void {
        this.moduleService.put<IModule>('Module', this.currentModule).subscribe({
          next: () => {
            this.loadModel();
            this.resetModel();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el modulo'
        });
        console.error('Error al actualizar el modulo', err);
      }
    });
  }

  deleteModel(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el modulo!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.moduleService.delete<IModule>('module', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El modulo ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.modules = this.modules.filter(m => m.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el modulo'
            });
            console.error('Error al eliminar modulo', err);
          }
        });
      }
    });
  }

  deleteModelLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este modulo se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.moduleService.deleteLogic<IModule>('module', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El modulo ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.modules = this.modules.filter(m => m.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el modulo'
            });
            console.error('Error al eliminar lógicamente', err);
          }
        });
      }
    });
  }

  toggleModel(mode: 'create' | 'edit'): void {
    if (mode === 'create') {
      this.isEditing = false;
      this.currentModule = this.getEmptyModel();
    }
    this.showForm = !this.showForm;
  }

  cancelModel(): void {
    if (this.currentModule.name || this.currentModule.description) {
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
          this.resetModel();
        }
      });
    } else {
      this.resetModel();
    }
  }

  resetModel(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentModule = this.getEmptyModel();
  }
}

