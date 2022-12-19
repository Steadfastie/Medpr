import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { ToastrService } from 'ngx-toastr';
import { Appointment } from 'src/app/models/appointment';
import { Doctor } from 'src/app/models/doctor';
import { AppointmentsActionsService } from 'src/app/services/appointments/appointments.actions.service';
import { AppointmentsService } from 'src/app/services/appointments/appointments.service';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { selectUserId } from 'src/app/store/app.states';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'edit-appointment',
  templateUrl: './edit.appointment.component.html',
  styleUrls: ['./edit.appointment.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class EditAppointmentComponent implements OnInit {
  @Input() appointment?: Appointment;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;
  userId?: string;
  doctors?: Doctor[];

  constructor(private fb: FormBuilder,
    private AppointmentsService: AppointmentsService,
    private actions: AppointmentsActionsService,
    private DoctorsService: DoctorsService,
    private doctorActions: DoctorsActionsService,
    private store: Store,
    private toastr: ToastrService,
    ) { }

  ngOnInit(): void {
    this.initialize();

    this.DoctorsService.getAllDoctors().subscribe(
      (doctors) => (this.doctors = doctors)
    );

    this.store.select(selectUserId).pipe()
      .subscribe((userId) => {
        this.userId = userId;
      });
  }

  appointmentForm = this.fb.group({
    date: ['', [Validators.required]],
    place: ['',[Validators.required, Validators.minLength(2), Validators.maxLength(30)],],
    doctorId: ['', [Validators.required]],
  });

  initialize() {
    if (this.appointment) {
      this.appointmentForm.setValue({
        date: this.appointment.date,
        place: this.appointment.place,
        doctorId: this.appointment.doctor?.id!,
      })
      this.doctorActions.emitDoctorSelect(this.appointment.doctor?.id!)
    }
  }

  edit(){
    if (!this.showSpinner && this.appointmentForm.valid){
      this.showSpinner = true;
      const initialAppointment = {
        id: this.appointment!.id,
        date: this.appointment!.date,
        place: this.appointment!.place,
        doctorId: this.appointment!.doctorId,
        userId: this.appointment!.user!['id']
      }

      let date = new Date(this.appointmentForm.value.date!);
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();

      let dateTime = year +
        '-' + month.toLocaleString('en-US', {minimumIntegerDigits: 2}) +
        '-' + day.toLocaleString('en-US', {minimumIntegerDigits: 2})

      const modifiedAppointment: Appointment = {
        id: this.appointment?.id!,
        date: dateTime,
        place: this.appointmentForm.value.place!,
        userId: this.appointment!.user!['id']!,
        doctorId: this.appointmentForm.value.doctorId!,
      }
      if (JSON.stringify(modifiedAppointment) !== JSON.stringify(initialAppointment)){
        this.AppointmentsService.patch(modifiedAppointment).pipe().subscribe({
          next: (appointment) => {
            this.showSpinner = false;
            this.actions.emitAppointmentResponse(appointment);
            this.toastr.info(`Appointment on ${formatDate(appointment.date, 'longDate', 'en-US')} updated`, `Updated`);
            this.closeEdit();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(`${err}`);
            this.toastr.error(`Appointment on ${formatDate(modifiedAppointment.date, 'longDate', 'en-US')} is still the same`, `Failed`);
            this.doctorActions.emitDoctorSelect('');
            this.errorMessage = "Could not modify appointment";
          },
        });
      }
    }

  }

  remove(){
    if (!this.showSpinner){
      this.showSpinner = true;
      this.AppointmentsService.delete(this.appointment!.id).pipe().subscribe({
        next: () => {
          this.showSpinner = false;
          this.toastr.success(`Appointment on ${formatDate(this.appointment?.date!, 'longDate', 'en-US')} removed`, `Success`);
          this.actions.emitAppointmentDelete(this.appointment!.id);
        },
        error: (err) => {
          this.toastr.warning(`Appointment on ${formatDate(this.appointment?.date!, 'longDate', 'en-US')} still persist`, `Failed`);
          console.log(`${err.message}`);
        },
    });
  }
}

  closeEdit(){
    if (!this.showSpinner){
      this.initialize();
      this.deselect.emit();
    }
  }

  selectDoctor(doctorId: any) {
    this.doctorActions.emitDoctorSelect(doctorId.source.value)
  }
}
