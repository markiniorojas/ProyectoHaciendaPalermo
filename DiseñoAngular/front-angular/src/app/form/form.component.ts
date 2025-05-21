import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, NgForm, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IForm } from '../interface/iform';
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
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {
  forms: IForm[] = [];
  currentForm: IForm = this.getEmptyForm();
  showForm: boolean = false;
  userRole: number = 0;
  isEditing: boolean = false;
  displayedColumns: string [] = ['name', 'description', 'url', 'isDeleted', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('formElement') formElement!: NgForm;

  constructor(private formService: ServiceGeneralService, private authService: AuthService) {}

  ngOnInit(): void {
    this.getUserRoleFromToken();
    this.loadForms();
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
  getEmptyForm(): IForm {
    return {
      id: 0,
      name: '',
      description: '',
      url: '',
      isDeleted: false
    };
  }

  private loadForms(): void {
    this.formService.get<IForm[]>('form').subscribe({
      next: data => {
        console.log('Datos recibidos:', data);
        this.forms = data;
      },
      error: (err) => {
        console.error('Error al cargar los Formularios', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudieron cargar los Formularios'
        });
      },
    });
  }

  toggleForm(action: string): void {
    if (action === 'create') {
      this.currentForm = this.getEmptyForm();
      this.isEditing = false;
    }
    this.showForm = !this.showForm;
  }

  // Maneja tanto la creación como la actualización
  submitForm(): void {
    if (this.formElement.invalid) {
      // Marcar todos los campos como 'touched' para mostrar los errores de validación
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

    this.isEditing ? this.updateForm() : this.addForm();
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
        this.loadForms();
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

  // Método para editar un formulario existente
  editForm(form: IForm): void {
    // Se crea una copia para evitar modificar directamente el objeto original
    this.currentForm = { ...form };
    this.isEditing = true;
    this.showForm = true;
  }

  // Método para actualizar un formulario
  updateForm(): void {
    this.formService.put<IForm>('form', this.currentForm).subscribe({ // Endpoint 'form' si tu PUT espera el objeto completo
      next: () => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Formulario actualizado correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.loadForms(); // Recargar la lista para ver los cambios
        this.resetForm(); // Limpiar el formulario
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

  // Método para eliminar un formulario de forma permanente
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
        // Asegúrate de que el endpoint 'form/permanent' coincide con tu backend
        this.formService.delete<IForm>('form/permanent', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'El formulario ha sido eliminado permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            // Quitar el formulario de la lista localmente para una actualización inmediata
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

  // Método para eliminar un formulario lógicamente (desactivar)
  deleteFormLogic(id: number): void {
    if (!id) { // Validación de ID
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'ID de formulario no válido'
      });
      return;
    }

    Swal.fire({
      title: '¿Estás seguro?',
      text: "El formulario se marcará como desactivado lógicamente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formService.put<void>(`form/logic/${id}`, {}).subscribe({ // Ajusta el endpoint según tu API
          next: () => {
            this.loadForms(); // Recargar formularios para reflejar el estado 'isDeleted'
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'Formulario desactivado lógicamente',
              timer: 1500,
              showConfirmButton: false
            });
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar el formulario'
            });
            console.error('Error al desactivar formulario', err);
          }
        });
      }
    });
  }

  // Método para reactivar un formulario lógicamente eliminado
  reactivateForm(form: IForm): void {
    if (!form || !form.id) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'ID de formulario no válido'
      });
      return;
    }
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¿Deseas reactivar este formulario?",
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí, reactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.formService.patchRestore<void>('form', form.id, {}).subscribe({ // Ajusta el endpoint según tu API
          next: () => {
            this.loadForms(); // Recargar la lista para ver el formulario reactivado
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
              text: 'No se pudo reactivar el formulario'
            });
            console.error('Error al reactivar formulario', err);
          }
        });
      }
    });
  }

  // Método para cancelar la edición o creación del formulario
  cancelForm(): void {
    // Verifica si hay cambios en el formulario actual (no solo si está en modo edición)
    const hasChanges =
      this.currentForm.name !== this.getEmptyForm().name ||
      this.currentForm.description !== this.getEmptyForm().description ||
      this.currentForm.url !== this.getEmptyForm().url ||
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
          this.resetForm();
          this.showForm = false; // Ocultar el formulario después de cancelar
        }
      });
    } else {
      this.resetForm();
      this.showForm = false; // Ocultar el formulario si no hay cambios
    }
  }

  // Método privado para resetear el estado del formulario
  private resetForm(): void {
    this.currentForm = this.getEmptyForm();
    this.isEditing = false;
    if (this.formElement) {
      this.formElement.resetForm();
    }
  }
}