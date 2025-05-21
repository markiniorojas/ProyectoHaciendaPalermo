import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import Swal from 'sweetalert2';

import { IPerson } from '../interface/iperson';
import { ServiceGeneralService } from '../service-general.service';
import { AuthService } from '../service/acceso.service';

@Component({
  selector: 'app-form-example',
  standalone: true,
  imports: [
    CommonModule, FormsModule,
    MatTableModule, MatPaginatorModule, MatCardModule,
    MatButtonModule, MatIconModule, MatInputModule,
    MatFormFieldModule, MatSlideToggleModule,
    MatChipsModule, MatTooltipModule,
    MatDatepickerModule, MatNativeDateModule
  ],
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {
  persons: IPerson[] = [];
  currentPerson: IPerson = this.getEmptyPerson();
  isEditing: boolean = false;
  showForm: boolean = false;
  userRole: number = 0;
  displayedColumns: string[] = ['firstName', 'lastName', 'documentType', 'document', 'dateBorn', 'phoneNumber', 'eps', 'genero', 'isDeleted', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild('formElement') formElement!: NgForm;

  constructor(private personService: ServiceGeneralService, private authService: AuthService) {}

  ngOnInit(): void {
    this.getUserRoleFromToken();
    this.loadPersons();
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

  private getEmptyPerson(): IPerson {
    return {
      id: 0,
      firstName: '',
      lastName: '',
      documentType: '',
      document: '',
      dateBorn: new Date(),
      phoneNumber: '',
      eps: '',
      genero: '',
      relatedPerson: false,
      isDeleted: false
    };
  }

  private loadPersons(): void {
    this.personService.get<IPerson[]>('person').subscribe({
      next: (data) => {
        console.log('Persons recibidos:', data);
        this.persons = data; 
      },
      error: (err) => {
        console.error('Error al cargar personas', err);
        Swal.fire({ 
          icon: 'error', 
          title: 'Error', 
          text: 'No se pudieron cargar las personas' 
        });
      },
    });
  }

  togglePerson(action: string): void {
    if (action === 'create') {
      this.currentPerson = this.getEmptyPerson();
      this.isEditing = false;
    }
    this.showForm = !this.showForm;
  }

  submitPerson(): void {
    if (this.formElement.invalid) {
      return;
    }
    
    this.isEditing ? this.updatePerson() : this.addPerson();
  }

  addPerson(): void {
    this.personService.post<IPerson>('person', this.currentPerson).subscribe({
      next: (person) => {
        Swal.fire({ 
          icon: 'success', 
          title: '¡Éxito!', 
          text: 'Persona creada correctamente', 
          timer: 1500, 
          showConfirmButton: false 
        });
        this.loadPersons(); 
        this.resetForm();
      },
      error: (err) => {
        Swal.fire({ 
          icon: 'error', 
          title: 'Error', 
          text: 'No se pudo crear la persona' 
        });
        console.error('Error al agregar persona', err);
      }
    });
  }

  editPerson(person: IPerson): void {
    this.currentPerson = { ...person };
    if (this.currentPerson.dateBorn) {
      this.currentPerson.dateBorn = new Date(this.currentPerson.dateBorn);
    }
    this.isEditing = true;
    this.showForm = true;
  }

  updatePerson(): void {
    this.personService.put<IPerson>('person', this.currentPerson).subscribe({
      next: () => {
        Swal.fire({ 
          icon: 'success', 
          title: '¡Éxito!', 
          text: 'Persona actualizada correctamente', 
          timer: 1500, 
          showConfirmButton: false 
        });
        this.loadPersons();
        this.resetForm();
      },
      error: (err) => {
        Swal.fire({ 
          icon: 'error', 
          title: 'Error', 
          text: 'No se pudo actualizar los datos de la persona' 
        });
        console.error('Error al actualizar la persona', err);
      }
    });
  }

  deletePerson(person: IPerson): void {
    if (!person || !person.id) {
      Swal.fire({ 
        icon: 'error', 
        title: 'Error', 
        text: 'ID de persona no válido' 
      });
      return;
    }

    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente la persona!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.personService.delete<IPerson>('person/permanent', person.id).subscribe({
          next: () => {
            this.loadPersons();
            Swal.fire({ 
              icon: 'success', 
              title: 'Eliminado', 
              text: 'Persona eliminada permanentemente', 
              timer: 1500, 
              showConfirmButton: false 
            });
          },
          error: (err) => {
            Swal.fire({ 
              icon: 'error', 
              title: 'Error', 
              text: 'No se pudo eliminar la persona' 
            });
            console.error('Error al eliminar la persona', err);
          }
        });
      }
    });
}

  deleteFormLogic(person: IPerson): void {
    if (!person || !person.id) {
      Swal.fire({ 
        icon: 'error', 
        title: 'Error', 
        text: 'ID de persona no válido' 
      });
      return;
    }

    Swal.fire({
      title: '¿Estás seguro?',
      text: "La persona se marcará como eliminada lógicamente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.personService.put<void>(`person/Logico/${person.id}`, {}).subscribe({
          next: () => {
            this.loadPersons(); // Reload persons after logical deletion
            Swal.fire({ 
              icon: 'success', 
              title: 'Desactivado', 
              text: 'Persona desactivada lógicamente', 
              timer: 1500, 
              showConfirmButton: false 
            });
          },
          error: (err) => {
            Swal.fire({ 
              icon: 'error', 
              title: 'Error', 
              text: 'No se pudo desactivar la persona' 
            });
            console.error('Error al desactivar persona', err);
          }
        });
      }
    });
  }

  reactivatePerson(person: IPerson): void {
    if (!person || !person.id) {
          Swal.fire({ 
            icon: 'error', 
            title: 'Error', 
            text: 'ID de persona no válido' 
          });
          return;
        }
      Swal.fire({
      title: '¿Estás seguro?',
      text: "¿Deseas reactivar esta persona?",
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí, reactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.personService.patchRestore<void>('person', person.id, {}).subscribe({
          next: () => {
            this.loadPersons(); 
            Swal.fire({ 
              icon: 'success', 
              title: 'Reactivado', 
              text: 'Persona reactivada correctamente', 
              timer: 1500, 
              showConfirmButton: false 
            });
          },
          error: (err) => {
            Swal.fire({ 
              icon: 'error', 
              title: 'Error', 
              text: 'No se pudo reactivar la persona' 
            });
            console.error('Error al reactivar persona', err);
          }
        });
      }
    });
  }

  cancelPerson(): void {
    if (this.isEditing || Object.values(this.currentPerson).some(v => v !== '' && v !== 0 && v !== false)) {
      Swal.fire({
        title: '¿Estás seguro?',
        text: 'Perderás los cambios no guardados',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, cancelar',
        cancelButtonText: 'Seguir editando'
      }).then((result) => {
        if (result.isConfirmed) {
          this.resetForm();
          this.showForm = false;
        }
      });
    } else {
      this.resetForm();
      this.showForm = false;
    }
  }

  private resetForm(): void {
    this.currentPerson = this.getEmptyPerson();
    this.isEditing = false;
    if (this.formElement) {
      this.formElement.resetForm();
    }
  }
}