import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  @Output() destroyCard = new EventEmitter<Drug>();

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
