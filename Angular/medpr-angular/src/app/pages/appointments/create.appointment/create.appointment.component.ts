import { selectUserId } from './../../../store/app.states';
import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Appointment } from 'src/app/models/appointment';
import { AppointmentsService } from 'src/app/services/appointments/appointments.service';

import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { Doctor } from 'src/app/models/doctor';
import { Store } from '@ngrx/store';
import { AppointmentsActionsService } from 'src/app/services/appointments/appointments.actions.service';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'create-appointment',
  templateUrl: './create.appointment.component.html',
  styleUrls: ['./create.appointment.component.scss']
})
export class CreateAppointmentComponent implements OnInit {
  @Output() deselect = new EventEmitter<void>();
  doctors: Doctor[] = [];
  userId?: string;

  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(
    private fb: FormBuilder,
    private appointmentsService: AppointmentsService,
    private DoctorsService: DoctorsService,
    private store: Store,
    private toastr: ToastrService,
    private appointmentActions: AppointmentsActionsService,
    private doctorActions: DoctorsActionsService,
  ) {}

  ngOnInit(): void {
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

  submit() {
    if (!this.showSpinner && this.appointmentForm.valid) {
      this.showSpinner = true;

      let date = new Date(this.appointmentForm.value.date!);
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();

      let dateTime = year +
        '-' + month.toLocaleString('en-US', {minimumIntegerDigits: 2}) +
        '-' + day.toLocaleString('en-US', {minimumIntegerDigits: 2})

      const appointment: Appointment = {
        id: Guid.createEmpty().toString(),
        date: dateTime,
        place: this.appointmentForm.value.place!,
        userId: this.userId!,
        doctorId: this.appointmentForm.value.doctorId!,
      };

      this.appointmentsService.create(appointment).pipe()
        .subscribe({
          next: (responseAppointment) => {
            this.showSpinner = false;
            this.appointmentActions.emitAppointmentResponse(responseAppointment);
            this.toastr.success(`Appointment on ${formatDate(responseAppointment.date, 'longDate', 'en-US')} is now in the app`, `Created`);
            this.closeCreate();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(err);
            this.toastr.error(`Appointment on ${formatDate(appointment.date, 'longDate', 'en-US')} is still in your dreams`, `Failed`);
            this.errorMessage = 'Could not create appointment';
          },
        });
    }
  }

  selectDoctor(doctorId: any) {
    this.doctorActions.emitDoctorSelect(doctorId.source.value)
  }

  cancel() {
    if (!this.showSpinner) {
      this.appointmentForm.reset();
      this.doctorActions.emitDoctorSelect('');
    }
  }

  closeCreate() {
    if (!this.showSpinner) {
      this.doctorActions.emitDoctorSelect('');
      this.deselect.emit();
    }
  }
}
