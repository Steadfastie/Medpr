import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DrugCardComponent } from './drug.card.component';

describe('DrugCardComponent', () => {
  let component: DrugCardComponent;
  let fixture: ComponentFixture<DrugCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DrugCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DrugCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
