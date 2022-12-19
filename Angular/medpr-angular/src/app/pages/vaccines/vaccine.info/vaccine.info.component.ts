import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Vaccine } from 'src/app/models/vaccine';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'vaccine-info',
  templateUrl: './vaccine.info.component.html',
  styleUrls: ['./vaccine.info.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class VaccineInfoComponent {
  @Input() vaccine?: Vaccine;
}
