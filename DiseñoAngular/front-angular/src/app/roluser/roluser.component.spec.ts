import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoluserComponent } from './roluser.component';

describe('RoluserComponent', () => {
  let component: RoluserComponent;
  let fixture: ComponentFixture<RoluserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoluserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoluserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
