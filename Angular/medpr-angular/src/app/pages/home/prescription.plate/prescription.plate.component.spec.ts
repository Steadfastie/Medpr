import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrescriptionPlateComponent } from './prescription.plate.component';

describe('PrescriptionPlateComponent', () => {
  let component: PrescriptionPlateComponent;
  let fixture: ComponentFixture<PrescriptionPlateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrescriptionPlateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrescriptionPlateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
