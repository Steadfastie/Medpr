import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FamilyFeedComponent } from './family.feed.component';

describe('FamilyFeedComponent', () => {
  let component: FamilyFeedComponent;
  let fixture: ComponentFixture<FamilyFeedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FamilyFeedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FamilyFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
