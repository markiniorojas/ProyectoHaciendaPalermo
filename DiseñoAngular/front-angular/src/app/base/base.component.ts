import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-formulario-reutilizable',
  templateUrl: './formulario-reutilizable.component.html',
  styleUrls: ['./formulario-reutilizable.component.css']
})
export class FormularioReutilizableComponent {
  @Input() formFields: any[] = []; // Recibe la lista de campos
  @Input() formModel: any = {};    // Recibe el modelo de datos

  @Output() formSubmit = new EventEmitter<any>();

  onSubmit() {
    this.formSubmit.emit(this.formModel);
  }
}
