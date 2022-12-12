import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';
import { Doctor } from 'src/app/models/doctor';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';

@Component({
  selector: 'appointment-card',
  templateUrl: './appointment.card.component.html',
  styleUrls: ['./appointment.card.component.scss'],
  providers: [DoctorsActionsService]
})
export class AppointmentCardComponent implements OnInit {
  @Input() appointment?: Appointment;
  doctor?: Doctor;
  selected: boolean;

  constructor(private doctorsService: DoctorsService,
    private doctorActions: DoctorsActionsService,
    ){
    this.selected = false;
  }

  ngOnInit(): void {
    if (this.appointment && this.appointment.doctor) {
      this.doctorsService.getDoctorById(this.appointment.doctor.id)
        .subscribe(doctor => this.doctor = doctor);
    };

    this.doctorActions.doctorSelectListner().subscribe((selectedDoctorId) => {
      if(selectedDoctorId !== ''){
        this.doctorsService.getDoctorById(selectedDoctorId).subscribe((doctor) => {
            this.doctor = doctor
          });
      }
      else{
        this.doctor = undefined;
      }
    });
  }

  selectToggle(){
    this.selected = !this.selected;
  }
}
