import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { CommonModule } from '@angular/common';
import { IPerson } from '../interface/iperson';
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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
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
    MatTooltipModule,
    MatDatepickerModule,
    MatNativeDateModule
   ],
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {
  persons: IPerson[] = [];
  currentPerson: IPerson = this.getEmptyPerson();
  showForm: boolean = false;
  isEditing: boolean = false;
  userRole: string = '';
  
  constructor(private personService: ServiceGeneralService, private authService: AuthService) {}

  ngOnInit(): void {
    const token = this.authService.getToken();
    if (token){
      const decoded: any = this.authService.decodeToken(token);
      this.userRole = decoded?.Role || '';
    }
    this.loadPersons();
  }

  // Helper para crear un formulario vacío
  getEmptyPerson(): IPerson {
    return {
      id: 0,
      firstName:  '',
      lastName:  '',
      documentType:  '',
      document:  '',
      dateBorn: new Date(),
      phoneNumber:  '',
      eps:  '',
      genero:  '',
      relatedPerson: false,
      isDeleted: false
    };
  }

  loadPersons(): void {
   this.personService.get<IPerson[]>('person').subscribe({
    next: data => {
      if (this.userRole === 'admin') {
        this.persons = data;
      } else {
        this.persons = data.filter(person => person.isDeleted === false);
      }
    },
    error: err => console.error('Error al cargar personas', err),
  });
  }
  
  

  // Maneja tanto la creación como la actualización
  submitPerson(): void {
    if (this.isEditing) {
      this.updatePerson();
    } else {
      this.addPerson();
    }
  }

  addPerson(): void {
    this.personService.post<IPerson>('person', this.currentPerson).subscribe({
      next: person => {
        Swal.fire({
          icon: 'success',
          title: '¡Éxito!',
          text: 'Persona creada correctamente',
          timer: 1500,
          showConfirmButton: false
        });
        this.persons.push(person);
        this.resetPerson();
      },
      error: err => {
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
    this.isEditing = true;
    // console.log('Datos enviados para actualizar:', this.currentForm);
    this.currentPerson = { ...person };
    this.showForm = true;
  }

  updatePerson(): void {
    this.currentPerson.phoneNumber = (this.currentPerson.phoneNumber);
    this.personService.put<IPerson>('person', this.currentPerson).subscribe({
      next:() => {
        this.loadPersons();
        this.resetPerson(); 
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar los datos de la persona'
        });
        console.error('Error al actualizar la persona', err);
      }
    })
      

  }

  deletePerson(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¡Esta acción eliminará permanentemente la persona!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.personService.delete<IPerson>('person', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Eliminado',
              text: 'La persona ha sido eliminada permanentemente',
              timer: 1500,
              showConfirmButton: false
            });
            this.persons = this.persons.filter(p => p.id !== id);
          },
          error: err => {
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

  deleteFormLogic(id: number): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "Esta persona se desactivará pero no se eliminará permanentemente",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.personService.deleteLogic<IPerson>('person', id).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Desactivado',
              text: 'La persona ha sido desactivado correctamente',
              timer: 1500,
              showConfirmButton: false
            });
            this.persons = this.persons.filter(p => p.id !== id);
          },
          error: err => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo desactivar la persona'
            });
            console.error('Error al eliminar persona', err);
          }
        });
      }
    });
  }

  togglePerson(mode: 'create' | 'edit'): void {
    if (mode === 'create') {
      this.isEditing = false;
      this.currentPerson = this.getEmptyPerson();
    }
    this.showForm = !this.showForm;
  }

  cancelPerson(): void {
    if (this.currentPerson.firstName || this.currentPerson.lastName || this.currentPerson.documentType || this.currentPerson.document || this.currentPerson.dateBorn || this.currentPerson.phoneNumber || this.currentPerson.eps || this.currentPerson.genero) {
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
          this.resetPerson();
        }
      });
    } else {
      this.resetPerson();
    }
  }

  resetPerson(): void {
    this.showForm = false;
    this.isEditing = false;
    this.currentPerson = this.getEmptyPerson();
  }
}

