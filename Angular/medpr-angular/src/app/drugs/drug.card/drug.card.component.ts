import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Drug } from 'src/app/models/drug';

@Component({
  selector: 'drug-card',
  templateUrl: './drug.card.component.html',
  styleUrls: ['./drug.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DrugCardComponent {
  @Input() drug?: Drug;
  selected: boolean = false;

  select(){
    this.selected = !this.selected;
  }
}
