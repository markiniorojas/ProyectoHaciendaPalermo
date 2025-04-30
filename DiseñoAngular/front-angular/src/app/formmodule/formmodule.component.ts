import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IFormModule } from '../interface/iformmodule';
import { ServiceGeneralService } from '../service-general.service';
import { FormsModule } from '@angular/forms';
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
// Importa el módulo MatSelectModule
import { MatSelectModule } from '@angular/material/select';

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
    MatSelectModule // Asegúrate de importar MatSelectModule aquí
  ],
  templateUrl: './formmodule.component.html',
  styleUrls: ['./formmodule.component.css']
})
export class FormmoduleComponent implements OnInit {
  formmodules: IFormModule[] = [];
  dataSource: MatTableDataSource<any>;
  currentFormModule: IFormModule = this.getEmptyFormModule();
  showForm: boolean = false;
  isEditing: boolean = false;
  forms: any[] = [];
  modules: any[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private formmoduleService: ServiceGeneralService) {
    this.dataSource = new MatTableDataSource<any>(this.formmodules);
  }

  ngOnInit(): void {
    this.loadFormModule();
    this.loadForms();
    this.loadModules();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
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

  loadFormModule(): void {
    this.formmoduleService.get<IFormModule[]>('formmodule').subscribe({
      next: data => {
        console.log('Datos de FormModule recibidos:', data);
        this.formmodules = data.filter(fm => !fm.isDeleted);
        this.mapFormModuleData();
      },
      error: err => console.error('Error al cargar los form module', err),
    });
  }

  loadForms(): void {
    this.formmoduleService.get<any[]>('Form').subscribe({
      next: data => {
        console.log('Datos de Forms recibidos:', data);
        this.forms = data.filter(form => !form.isDeleted);
        this.mapFormModuleData();
      },
      error: err => console.error('Error al cargar los forms', err),
    });
  }

  loadModules(): void {
    this.formmoduleService.get<any[]>('Module').subscribe({
      next: data => {
        console.log('Datos de Modules recibidos:', data);
        this.modules = data.filter(module => !module.isDeleted);
        this.mapFormModuleData();
      },
      error: err => console.error('Error al cargar los modules', err),
    });
  }

  mapFormModuleData(): void {
    if (this.formmodules && this.forms && this.modules) {
      this.dataSource.data = this.formmodules.map(fm => {
        const form = this.forms.find(f => f.id === fm.formId);
        const module = this.modules.find(m => m.id === fm.moduleId);
        return {
          ...fm,
          formName: form ? form.name : 'Desconocido',
          moduleName: module ? module.moduleName : 'Desconocido',
        };
      });
    }
  }

  submitFormModule(): void {
    if (this.isEditing) {
      this.updateFormModule();
    } else {
      this.addFormModule();
    }
  }

  addFormModule(): void {
    this.formmoduleService.post<IFormModule>('formmodule', this.currentFormModule).subscribe({
      next: formmodule => {
        Swal.fire({ icon: 'success', title: '¡Éxito!', text: 'Form Module creado correctamente', timer: 1500, showConfirmButton: false });
        this.formmodules.push(formmodule);
        this.resetFormModule();
      },
      error: err => {
        Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo crear el form Module' });
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
    this.formmoduleService.put<IFormModule>('formmodule', this.currentFormModule.id, this.currentFormModule).subscribe({
      next: updatedFormModule => {
        Swal.fire({ icon: 'success', title: '¡Éxito!', text: 'form module actualizado correctamente', timer: 1500, showConfirmButton: false });
        const index = this.formmodules.findIndex(fm => fm.id === updatedFormModule.id);
        if (index > -1) this.formmodules[index] = updatedFormModule;
        this.resetFormModule();
      },
      error: err => {
        Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo actualizar el form modules' });
        console.error('Error al actualizar el form module', err);
      }
    });
  }

  deleteFormModule(id: number): void {
    Swal.fire({ title: '¿Estás seguro?', text: "¡Esta acción eliminará permanentemente el form module!", icon: 'warning', showCancelButton: true, confirmButtonColor: '#d33', cancelButtonColor: '#3085d6', confirmButtonText: 'Sí, eliminar', cancelButtonText: 'Cancelar' }).then((result) => {
      if (result.isConfirmed) {
        this.formmoduleService.delete<IFormModule>('formmodule', id).subscribe({
          next: () => {
            Swal.fire({ icon: 'success', title: 'Eliminado', text: 'El form module ha sido eliminado permanentemente', timer: 1500, showConfirmButton: false });
            this.formmodules = this.formmodules.filter(fm => fm.id !== id);
          },
          error: err => {
            Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo eliminar el form module' });
            console.error('Error al eliminar form module', err);
          }
        });
      }
    });
  }

  deleteFormModuleLogic(id: number): void {
    Swal.fire({ title: '¿Estás seguro?', text: "Este form module se desactivará pero no se eliminará permanentemente", icon: 'question', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Sí, desactivar', cancelButtonText: 'Cancelar' }).then((result) => {
      if (result.isConfirmed) {
        this.formmoduleService.deleteLogic<IFormModule>('formmodule', id).subscribe({
          next: () => {
            Swal.fire({ icon: 'success', title: 'Desactivado', text: 'El form module ha sido desactivado correctamente', timer: 1500, showConfirmButton: false });
            this.formmodules = this.formmodules.filter(fm => fm.id !== id);
          },
          error: err => {
            Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo desactivar el form module' });
            console.error('Error al eliminar lógicamente', err);
          }
        });
      }
    });
  }

  toggleFormModule(formmodule: 'create' | 'edit'): void {
    if (formmodule === 'create') {
      this.isEditing = false;
      this.currentFormModule = this.getEmptyFormModule();
    }
    this.showForm = !this.showForm;
  }

  cancelFormModule(): void {
    if (
      this.currentFormModule.formId ||
      this.currentFormModule.moduleName ||
      this.currentFormModule.formName ||
      this.currentFormModule.moduleId
    ) {
      Swal.fire({ title: '¿Estás seguro?', text: 'Perderás los cambios no guardados', icon: 'question', showCancelButton: true, confirmButtonColor: '#3085d6', cancelButtonColor: '#d33', confirmButtonText: 'Sí, cancelar', cancelButtonText: 'Seguir editando' }).then((result) => {
        if (result.isConfirmed) {
          this.resetFormModule();
        }
      });
    } else {
      this.resetFormModule();
    }
  }

  resetFormModule(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentFormModule = this.getEmptyFormModule();
  }
}