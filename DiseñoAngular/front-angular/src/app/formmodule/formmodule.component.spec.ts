import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormmoduleComponent } from './formmodule.component';

describe('FormmoduleComponent', () => {
  let component: FormmoduleComponent;
  let fixture: ComponentFixture<FormmoduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormmoduleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormmoduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
