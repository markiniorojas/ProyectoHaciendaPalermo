import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IForm } from '../interface/iform';

@Component({
  selector: 'app-form-example',
  standalone: true,
  imports: [CommonModule,FormsModule ],
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
      IsDeleted: true
    };
  }

  loadForms(): void {
    this.formService.get<IForm[]>('form').subscribe({
      next: data => {
        console.log('Formularios cargados:', data);
        this.forms = data;
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
        this.forms.push(form);
        this.resetForm();
      },
      error: err => console.error('Error al agregar formulario', err)
    });
  }

  editForm(form: IForm): void {
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentForm);
    this.currentForm = { ...form };
    this.showForm = true;
  }

  updateForm(): void {
    console.log(this.currentForm)
    this.formService.put<IForm>('form', this.currentForm.id, this.currentForm).subscribe({
      next: updatedForm => {
        const index = this.forms.findIndex(f => f.id === updatedForm.id);
        if (index > -1) this.forms[index] = updatedForm;
        this.resetForm();
      },
      error: err => console.error('Error al actualizar formulario', err)
    });
  }

  deleteForm(id: number): void {
    this.formService.delete<IForm>('form', id).subscribe({
      next: () => this.forms = this.forms.filter(f => f.id !== id),
      error: err => console.error('Error al eliminar formulario', err)
    });
  }

  deleteFormLogic(id: number): void {
    this.formService.deleteLogic<IForm>('form', id).subscribe({
      next: () => this.forms = this.forms.filter(f => f.id !== id),
      error: err => console.error('Error al eliminar lógicamente', err)
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
    this.resetForm();
  }

  resetForm(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentForm = this.getEmptyForm();
  }
}

