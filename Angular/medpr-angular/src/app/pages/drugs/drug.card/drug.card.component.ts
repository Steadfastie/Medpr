import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'drug-card',
  templateUrl: './drug.card.component.html',
  styleUrls: ['./drug.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class DrugCardComponent{
  @Input() random?: Drug;
  @Input() drug?: Drug;

  selected: boolean;

  constructor() {
    this.selected = false;
  }

  selectToggle(){
    this.selected = !this.selected;
  }

  createFromRandom(){
    if(this.random){
      this.random.id = Guid.createEmpty().toString();
      this.random.price = Math.floor((Math.random() + 0.1) * 10);
    };
    this.selectToggle();
  }

  createFromBlank(){
    this.selectToggle();
  }
}
