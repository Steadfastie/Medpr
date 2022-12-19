import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { Appointment } from 'src/app/models/appointment';
import { Doctor } from 'src/app/models/doctor';
import { User } from 'src/app/models/user';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { selectStateUser } from 'src/app/store/app.states';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'appointment-card',
  templateUrl: './appointment.card.component.html',
  styleUrls: ['./appointment.card.component.scss'],
  providers: [DoctorsActionsService],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class AppointmentCardComponent implements OnInit {
  @Input() appointment?: Appointment;
  user?: User;
  currentUserId?: string;
  doctor?: Doctor;
  selected: boolean;

  constructor(private doctorsService: DoctorsService,
    private doctorActions: DoctorsActionsService,
    private store: Store,
    ){
    this.selected = false;
  }

  ngOnInit(): void {
    this.store.select(selectStateUser).pipe().subscribe((authUser) => {
      this.currentUserId = authUser?.userId;
    });

    if (this.appointment && this.appointment.doctor) {
      this.doctorsService.getDoctorById(this.appointment.doctor.id)
        .subscribe(doctor => this.doctor = doctor);
    };

    if (this.appointment && this.appointment.user
        && this.appointment.user['id'] != this.currentUserId) {
          this.user = this.appointment.user;
    }

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
