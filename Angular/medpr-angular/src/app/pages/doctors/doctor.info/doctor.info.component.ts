import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Doctor } from 'src/app/models/doctor';

@Component({
  selector: 'doctor-info',
  templateUrl: './doctor.info.component.html',
  styleUrls: ['./doctor.info.component.scss']
})
export class DoctorInfoComponent {
  @Input() doctor?: Doctor;
}
