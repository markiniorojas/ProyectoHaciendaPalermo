import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IFormModule } from '../interface/iformmodule';
import { ServiceGeneralService } from '../service-general.service';
import { FormsModule, NgForm } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
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
import { MatSelectModule } from '@angular/material/select';
import { AuthService } from '../service/acceso.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-form-example',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
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
    MatSelectModule
  ],
  templateUrl: './formmodule.component.html',
  styleUrls: ['./formmodule.component.css']
})
export class FormmoduleComponent implements OnInit, AfterViewInit {
  formmodules: IFormModule[] = [];
  dataSource: MatTableDataSource<IFormModule> = new MatTableDataSource<IFormModule>([]);
  currentFormModule: IFormModule = this.getEmptyFormModule();
  showForm: boolean = false;
  userRole: number = 0;
  isEditing: boolean = false;
  forms: any[] = [];
  modules: any[] = [];
  displayedColumns: string[] = ['id', 'formName', 'moduleName', 'isDeleted', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('formElement') formElement!: NgForm;

  constructor(private formmoduleService: ServiceGeneralService, private authService: AuthService) {}

  ngOnInit(): void {
    this.getUserRoleFromToken();
    // Llama a loadAllData aquí para que userRole esté disponible al mapear los datos
    this.loadAllData();
  }

  ngAfterViewInit(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  private getUserRoleFromToken(): void {
    const token = this.authService.getToken();
    if (token) {
      const decoded: any = this.authService.decodeToken(token);
      // Asigna el rol del usuario, asegurándote de que coincida con el rol de administrador en tu sistema
      this.userRole = parseInt(decoded?.Role, 10) || 0;
      console.log('User role from token:', this.userRole);
    } else {
      console.warn('No token available');
      this.userRole = 0; // O un valor predeterminado si no hay token
    }
  }

  getEmptyFormModule(): IFormModule {
    return {
      id: 0,
      moduleName: '',
      formName: '',
      isDeleted: false,
      formId: 0,
      moduleId: 0,
    };
  }

  loadAllData(): void {
    forkJoin({
      formmodules: this.formmoduleService.get<IFormModule[]>('formmodule'),
      forms: this.formmoduleService.get<any[]>('Form'),
      modules: this.formmoduleService.get<any[]>('Module')
    }).subscribe({
      next: ({ formmodules, forms, modules }) => {
        this.formmodules = formmodules;
        this.forms = forms.filter(form => !form.isDeleted);
        this.modules = modules.filter(module => !module.isDeleted);
        this.mapFormModuleData(); // Llama a mapFormModuleData después de cargar todos los datos
      },
      error: err => {
        console.error('Error al cargar datos iniciales', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudieron cargar los datos iniciales de Form Modules, Formularios o Módulos'
        });
      }
    });
  }

  mapFormModuleData(): void {
    if (this.formmodules && this.forms && this.modules) {
      const mappedData = this.formmodules.map(fm => {
        const form = this.forms.find(f => f.id === fm.formId);
        const module = this.modules.find(m => m.id === fm.moduleId);
        return {
          ...fm,
          formName: form ? form.name : 'Desconocido',
          moduleName: module ? module.moduleName : 'Desconocido',
        };
      });
      this.dataSource.data = mappedData;
      if (this.paginator) {
        this.dataSource.paginator = this.paginator;
      }
    } else {
      this.dataSource.data = [];
    }
  }

  // ... El resto de tus métodos (toggleFormModule, submitFormModule, addFormModule, etc.)
  // Ya están llamando a loadAllData() o no necesitan cambios para esta funcionalidad

  toggleFormModule(action: 'create' | 'edit' = 'create'): void {
    if (action === 'create') {
      this.currentFormModule = this.getEmptyFormModule();
      this.isEditing = false;
    }
    this.showForm = !this.showForm;
  }

  submitFormModule(): void {
    if (this.formElement && this.formElement.invalid) {
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

    if (!this.isEditing) {
      const existingFormModule = this.formmodules.find(fm =>
        fm.formId === this.currentFormModule.formId &&
        fm.moduleId === this.currentFormModule.moduleId &&
        !fm.isDeleted
      );
      if (existingFormModule) {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Esta combinación de Formulario y Módulo ya existe.'
        });
        return;
      }
    }

    this.isEditing ? this.updateFormModule() : this.addFormModule();
  }

  addFormModule(): void {
    this.formmoduleService.post<IFormModule>('formmodule', this.currentFormModule).subscribe({
      next: formmodule => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Form Module creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.loadAllData();
        this.resetFormModule();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el form Module'
        });
        console.error('Error al agregar form Module', err);
      }
    });
  }

  editFormModule(formmodule: IFormModule): void {
    this.isEditing = true;
    this.currentFormModule = { ...formmodule };
    this.showForm = true;
  }

  updateFormModule(): void {
    this.formmoduleService.put<IFormModule>('formmodule', this.currentFormModule).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Form Module actualizado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.loadAllData();
        this.resetFormModule();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el Form Module'
        });
        console.error('Error al actualizar Form Module', err);
      }
    });
  }

  deleteFormModule(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el Form Module!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formmoduleService.delete<IFormModule>('formmodule/permanent', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El Form Module ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.loadAllData();
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el Form Module'
            });
            console.error('Error al eliminar Form Module', err);
          }
        });
      }
    });
  }

  deleteFormModuleLogic(id: number): void {
    if (!id) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'ID de Form Module no válido'
      });
      return;
    }

    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este Form Module se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        // Asegúrate de que esta URL sea la correcta para tu backend
        this.formmoduleService.put<void>(`formmodule/Logico/${id}`, {}).subscribe({
          next: () => {
            this.loadAllData(); // Recarga todos los datos para ver los cambios
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'Form Module desactivado lógicamente',
              timer: 1500,
              showConfirmButton: false
            });
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el Form Module'
            });
            console.error('Error al desactivar Form Module', err);
          }
        });
      }
    });
  }

  reactivateFormModule(formmodule: IFormModule): void {
    if (!formmodule || !formmodule.id) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'ID de Form Module no válido'
      });
      return;
    }
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¿Deseas reactivar este Form Module?",
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí, reactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        // Asegúrate de que esta URL sea la correcta para tu backend
        this.formmoduleService.patchRestore<void>('formmodule', formmodule.id, {}).subscribe({
          next: () => {
            this.loadAllData(); // Recarga todos los datos para ver los cambios
            Swal.fire({
              icon: 'success',
              title: 'Reactivado',
              text: 'Formulario reactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo reactivar el Form Module'
            });
            console.error('Error al reactivar Form Module', err);
          }
        });
      }
    });
  }

  cancelFormModule(): void {
    const hasChanges =
      this.currentFormModule.formId !== 0 ||
      this.currentFormModule.moduleId !== 0;

    if (this.isEditing || hasChanges) {
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
          this.resetFormModule();
        }
      });
    } else {
      this.resetFormModule();
    }
  }

  resetFormModule(): void {
    this.currentFormModule = this.getEmptyFormModule();
    this.isEditing = false;
    this.showForm = false;
    if (this.formElement) {
      this.formElement.resetForm();
    }
  }
}