import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';

@Component({
  selector: 'appointment-card',
  templateUrl: './appointment.card.component.html',
  styleUrls: ['./appointment.card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppointmentCardComponent implements OnInit {
  @Input() appointment?: Appointment;
  doctor?: Doctor;

  selected: boolean;

  constructor(private doctorsService: DoctorsService) {
    this.selected = false;
  }

  ngOnInit(): void {
    if(this.appointment) {
      this.doctorsService.getDoctorById(this.appointment.doctorId).subscribe(
        doctor => this.doctor = doctor
      )
    }
  }

  selectToggle(){
    this.selected = !this.selected;
  }

  selectDoctor(doctorId:string){
    this.doctorsService.getDoctorById(doctorId).subscribe(
      doctor => this.doctor = doctor
    );
  }

  createFromBlank(){
    this.selectToggle();
  }
}
