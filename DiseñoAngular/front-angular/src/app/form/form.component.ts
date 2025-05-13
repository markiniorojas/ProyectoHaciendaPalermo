import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IForm } from '../interface/iform';
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
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {
  forms: IForm[] = [];
  currentForm: IForm = this.getEmptyForm();
  showForm: boolean = false;
  isEditing: boolean = false;
  
  constructor(private formService: ServiceGeneralService) {}

  ngOnInit(): void {
    this.loadForms();
  }

  // Helper para crear un formulario vacío
  getEmptyForm(): IForm {
    return {
      id: 0,
      name:  '',
      description: '',
      url:'',
      isDeleted: false
    };
  }

  loadForms(): void {
    this.formService.get<IForm[]>('form').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        // Solo formularios que NO estén eliminados (IsDeleted == false)
        this.forms = data.filter(form => form.isDeleted === false);
      },
      error: err => console.error('Error al cargar los formularios', err),
    });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitForm(): void {
    if (this.isEditing) {
      this.updateForm();
    } else {
      this.addForm();
    }
  }

  addForm(): void {
    this.formService.post<IForm>('form', this.currentForm).subscribe({
      next: form => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Formulario creado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.forms.push(form);
        this.resetForm();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo crear el formulario'
        });
        console.error('Error al agregar formulario', err);
      }
    });
  }
  

  editForm(form: IForm): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentForm);
    this.currentForm = { ...form };
    this.showForm = true;
  }

  updateForm(): void {
   this.formService.put<IForm>('Form', this.currentForm).subscribe({
      next: () => {
        this.loadForms();
        this.resetForm();
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el formulario'
        });
        console.error('Error al actualizar formulario', err);
      }
    });
  }

  deleteForm(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente el formulario!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formService.delete<IForm>('form', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El formulario ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.forms = this.forms.filter(f => f.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo eliminar el formulario'
            });
            console.error('Error al eliminar formulario', err);
          }
        });
      }
    });
  }

  deleteFormLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Este formulario se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formService.deleteLogic<IForm>('form', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'El formulario ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.forms = this.forms.filter(f => f.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el formulario'
            });
            console.error('Error al eliminar lógicamente', err);
          }
        });
      }
    });
  }

  toggleForm(mode: 'create' | 'edit'): void {
    if (mode === 'create') {
      this.isEditing = false;
      this.currentForm = this.getEmptyForm();
    }
    this.showForm = !this.showForm;
  }

  cancelForm(): void {
    if (this.currentForm.name || this.currentForm.description || this.currentForm.url) {
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
          this.resetForm();
        }
      });
    } else {
      this.resetForm();
    }
  }

  resetForm(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentForm = this.getEmptyForm();
  }
}

