import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ServiceGeneralService } from '../service-general.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-form-example',
  templateUrl: '',
})
export class FormExampleComponent implements OnInit {
  form!: FormGroup;

  @Input() endpoint!: string;
  @Input() fields!: { name: string; label: string; required?: boolean }[];

  id!: number;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private service: ServiceGeneralService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const group: any = {};
    this.fields.forEach(field => {
      group[field.name] = field.required ? ['', Validators.required] : [''];
    });

    this.form = this.fb.group(group);

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.isEditMode = true;
      this.service.getById<any>(this.endpoint, this.id).subscribe(data => {
        this.form.patchValue(data);
      });
    }
  }

  onSubmit() {
    if (this.form.invalid) return;

    const request = this.isEditMode
      ? this.service.put(this.endpoint, this.id, this.form.value)
      : this.service.post(this.endpoint, this.form.value);

    request.subscribe(() => {
      this.router.navigate(['/' + this.endpoint]);
    });
  }
}

