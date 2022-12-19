import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Drug } from 'src/app/models/drug';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'drug-info',
  templateUrl: './drug.info.component.html',
  styleUrls: ['./drug.info.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class DrugInfoComponent {
  @Input() drug?: Drug;
}
