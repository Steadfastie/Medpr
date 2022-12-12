import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Vaccine } from 'src/app/models/vaccine';

@Component({
  selector: 'vaccine-info',
  templateUrl: './vaccine.info.component.html',
  styleUrls: ['./vaccine.info.component.scss']
})
export class VaccineInfoComponent {
  @Input() vaccine?: Vaccine;
}
