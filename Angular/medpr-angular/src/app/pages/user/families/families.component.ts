import { Component } from '@angular/core';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'families',
  templateUrl: './families.component.html',
  styleUrls: ['./families.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class FamiliesComponent {

}
