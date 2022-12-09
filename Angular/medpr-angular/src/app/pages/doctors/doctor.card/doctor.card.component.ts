import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Doctor } from 'src/app/models/doctor';

@Component({
  selector: 'doctor-card',
  templateUrl: './doctor.card.component.html',
  styleUrls: ['./doctor.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorCardComponent{
  @Input() doctor?: Doctor;

  selected: boolean;

  constructor() {
    this.selected = false;
  }

  selectToggle(){
    this.selected = !this.selected;
  }

  createFromBlank(){
    this.selectToggle();
  }
}
