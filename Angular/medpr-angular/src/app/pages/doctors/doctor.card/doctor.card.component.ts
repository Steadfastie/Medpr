import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Doctor } from 'src/app/models/doctor';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'doctor-card',
  templateUrl: './doctor.card.component.html',
  styleUrls: ['./doctor.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
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
