import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FamilyControlComponent } from './family.control.component';

describe('FamilyControlComponent', () => {
  let component: FamilyControlComponent;
  let fixture: ComponentFixture<FamilyControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FamilyControlComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FamilyControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
