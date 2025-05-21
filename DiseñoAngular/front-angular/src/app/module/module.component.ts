import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, NgForm, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IModule } from '../interface/imodule';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import Swal from 'sweetalert2';
import { AuthService } from '../service/acceso.service';



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
  userRole: number = 0;
  isEditing: boolean = false;
  displayedColumns: string [] = ['name', 'description','isDeleted', 'actions'];
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('formElement') formElement!: NgForm; 
  
constructor(private moduleService: ServiceGeneralService, private authService: AuthService ) {}

  ngOnInit(): void {
    this.getUserRoleFromToken();
    this.loadModule();
  }

   private getUserRoleFromToken(): void {
    const token = this.authService.getToken();
    if (token) {
      const decoded: any = this.authService.decodeToken(token);
      this.userRole = parseInt(decoded?.Role, 10) || 0;
      console.log('User role from token:', this.userRole);
    } else {
      console.warn('No token available');
    }
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

  private loadModule(): void {
      this.moduleService.get<IModule[]>('module').subscribe({
        next: data => {
          console.log('Datos recibidos:', data);
          this.modules = data;
        },
        error: (err) => {
          console.error('Error al cargar los modulos', err);
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudieron cargar los modulos'
          });
        },
      });
    }
  
  toggleModel(action: string): void {
    if (action === 'create') {
      this.isEditing = false;
      this.currentModule = this.getEmptyModel();
    }
    this.showForm = !this.showForm;
  }

  // Maneja tanto la creación como la actualización
  submitModel(): void {
      if (this.formElement.invalid) {
      
        Object.keys(this.formElement.controls).forEach(field => {
          const control = this.formElement.controls[field];
          control.markAsTouched({ onlySelf: true });
        });
        Swal.fire({
          icon: 'warning',
          title: 'Formulario inválido',
          text: 'Por favor, completa todos los campos requeridos correctamente.'
        });
        return;
      }
  
      this.isEditing ? this.updateModule() : this.addModel();
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
        this.loadModule();
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
   this.moduleService.put<IModule>('module', this.currentModule).subscribe({
     next: () => {
       Swal.fire({
             icon: 'success',
             title: '¡Éxito!',
             text: 'Formulario actualizado correctamente',
             timer: 1500,
             showConfirmButton: false
           });
           this.loadModule(); // Recargar la lista para ver los cambios
           this.resetModel(); // Limpiar el formulario
         },
         error: err => {
           Swal.fire({
             icon: 'error',
             title: 'Error',
             text: 'No se pudo actualizar el modulo'
           });
           console.error('Error al actualizar modulo', err);
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
        this.moduleService.delete<IModule>('module/permanent', id).subscribe({
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
    if (!id){
      Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'ID de modulo no válido'
      });
      return;
    }
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
        this.moduleService.put<void>(`module/logic/${id}`, {}).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El modulo ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
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

  // Método para reactivar un formulario lógicamente eliminado
    reactivateForm(form: IModule): void {
      if (!form || !form.id) {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'ID de modulo no válido'
        });
        return;
      }
      Swal.fire({
        title: '¿Estás seguro?',
        text: "¿Deseas reactivar este modulo?",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, reactivar',
        cancelButtonText: 'Cancelar'
      }).then((result) => {
        if (result.isConfirmed) {
          this.moduleService.patchRestore<void>('mudule', module.id, {}).subscribe({ // Ajusta el endpoint según tu API
            next: () => {
              this.loadModule(); // Recargar la lista para ver el formulario reactivado
              Swal.fire({
                icon: 'success',
                title: 'Reactivado',
                text: 'modulo reactivado correctamente',
                timer: 1500,
                showConfirmButton: false
              });
            },
            error: (err) => {
              Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo reactivar el formulario'
              });
              console.error('Error al reactivar formulario', err);
            }
          });
        }
      });
    }

  cancelModel(): void {
    const hasChanges =
          this.currentModule.name !== this.getEmptyModel().name ||
          this.currentModule.description !== this.getEmptyModel().description ||
          this.isEditing; // También considera si estamos en modo edición
    
        if (hasChanges) {
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
              this.showForm = false; // Ocultar el formulario después de cancelar
            }
          });
        } else {
          this.resetModel();
          this.showForm = false; // Ocultar el formulario si no hay cambios
        }
  }

  private resetModel(): void {
    this.currentModule = this.getEmptyModel();
    this.isEditing = false;
    if (this.formElement) {
      this.formElement.resetModel();
    }
  }
}

