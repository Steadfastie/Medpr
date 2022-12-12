import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Vaccine } from 'src/app/models/vaccine';

@Component({
  selector: 'vaccine-card',
  templateUrl: './vaccine.card.component.html',
  styleUrls: ['./vaccine.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class VaccineCardComponent{
  @Input() vaccine?: Vaccine;

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
