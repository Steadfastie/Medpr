import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Doctor } from 'src/app/models/doctor';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'doctor-info',
  templateUrl: './doctor.info.component.html',
  styleUrls: ['./doctor.info.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class DoctorInfoComponent {
  @Input() doctor?: Doctor;
}
