import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator'; // Importa MatPaginator
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import Swal from 'sweetalert2';

import { ServiceGeneralService } from '../service-general.service';
import { IFormModule } from '../interface/iformmodule';

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
    MatTooltipModule
  ],
  templateUrl: './formmodule.component.html',
  styleUrls: ['./formmodule.component.css']
})
export class FormmoduleComponent implements OnInit, AfterViewInit {
  formmodules: IFormModule[] = [];
  dataSource: MatTableDataSource<IFormModule>;
  currentFormModule: IFormModule = this.getEmptyFormModule();
  showForm: boolean = false;
  isEditing: boolean = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator; // Declara paginator como MatPaginator

  constructor(private formmoduleService: ServiceGeneralService) {
    this.dataSource = new MatTableDataSource<IFormModule>(this.formmodules);
  }

  ngOnInit(): void {
    this.loadFormModule();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator; // Asigna el paginador después de la inicialización de la vista
  }

  getEmptyFormModule(): IFormModule {
    return {
      id: 0,
      formId: 0,
      moduleName: '',
      formName: '',
      moduleId: 0,
      isDeleted: false
    };
  }

  loadFormModule(): void {
    this.formmoduleService.get<IFormModule[]>('formmodule').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        this.formmodules = data.filter(formmodule => formmodule.isDeleted === false);
        this.dataSource.data = this.formmodules;
      },
      error: err => console.error('Error al cargar los form module', err),
    });
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
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Form Module creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.formmodules.push(formmodule);
        this.dataSource.data = [...this.formmodules];
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
    this.formmoduleService.put<IFormModule>('formmodule', this.currentFormModule.id, this.currentFormModule).subscribe({
      next: updatedFormModule => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'form module actualizado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        const index = this.formmodules.findIndex(fm => fm.id === updatedFormModule.id);
        if (index > -1) this.formmodules[index] = updatedFormModule;
        this.dataSource.data = [...this.formmodules];
        this.resetFormModule();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el form modules'
        });
        console.error('Error al actualizar el form module', err);
      }
    });
  }

  deleteFormModule(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el form module!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formmoduleService.delete<IFormModule>('formmodule', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El form module ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.formmodules = this.formmodules.filter(fm => fm.id !== id);
            this.dataSource.data = [...this.formmodules];
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el form module'
            });
            console.error('Error al eliminar form module', err);
          }
        });
      }
    });
  }

  deleteFormModuleLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este form module se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formmoduleService.deleteLogic<IFormModule>('formmodule', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El form module ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.formmodules = this.formmodules.filter(fm => fm.id !== id);
            this.dataSource.data = [...this.formmodules];
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el form module'
            });
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
    this.showForm = false;
    this.isEditing = false;
    this.currentFormModule = this.getEmptyFormModule();
  }
}