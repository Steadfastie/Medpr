import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentPlateComponent } from './appointment.plate.component';

describe('AppointmentPlateComponent', () => {
  let component: AppointmentPlateComponent;
  let fixture: ComponentFixture<AppointmentPlateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointmentPlateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppointmentPlateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
