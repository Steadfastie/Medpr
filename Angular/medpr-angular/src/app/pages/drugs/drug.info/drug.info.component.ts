import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Drug } from 'src/app/models/drug';

@Component({
  selector: 'drug-info',
  templateUrl: './drug.info.component.html',
  styleUrls: ['./drug.info.component.scss']
})
export class DrugInfoComponent {
  @Input() drug?: Drug;
}
