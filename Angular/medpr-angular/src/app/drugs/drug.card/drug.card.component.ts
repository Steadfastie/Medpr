import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Drug } from 'src/app/models/drug';

@Component({
  selector: 'drug-card',
  templateUrl: './drug.card.component.html',
  styleUrls: ['./drug.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DrugCardComponent{
  @Input() random?: Drug;
  @Input() drug?: Drug;
  selected: boolean;

  selectToggle(){
    this.selected = !this.selected;
  }

  constructor() {
    this.selected = false;
  }

  createFromRandom(){
    if(this.random){
      this.random.id = Guid.createEmpty();
      this.random.price = Math.floor(Math.random() * 10);
    };
    this.selectToggle();
  }

  createFromBlank(){
    this.selectToggle();
  }
}
