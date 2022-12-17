import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberControlComponent } from './member.control.component';

describe('MemberControlComponent', () => {
  let component: MemberControlComponent;
  let fixture: ComponentFixture<MemberControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MemberControlComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MemberControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
