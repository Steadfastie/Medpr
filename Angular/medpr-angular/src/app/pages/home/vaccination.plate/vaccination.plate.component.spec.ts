import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VaccinationPlateComponent } from './vaccination.plate.component';

describe('VaccinationPlateComponent', () => {
  let component: VaccinationPlateComponent;
  let fixture: ComponentFixture<VaccinationPlateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VaccinationPlateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VaccinationPlateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
