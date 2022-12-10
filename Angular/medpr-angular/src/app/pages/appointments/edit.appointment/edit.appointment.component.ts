import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Appointment } from 'src/app/models/appointment';
import { Doctor } from 'src/app/models/doctor';
import { AppointmentsService } from 'src/app/services/appointments/appointments.service';
import { DoctorsService } from 'src/app/services/doctors/doctors.service';
import { selectUserId } from 'src/app/store/app.states';


@Component({
  selector: 'edit-appointment',
  templateUrl: './edit.appointment.component.html',
  styleUrls: ['./edit.appointment.component.scss']
})
export class EditAppointmentComponent implements OnInit {
  @Input() appointment?: Appointment;
  @Output() deselect = new EventEmitter<void>();
  @Output() selectedDoctor = new EventEmitter<string>();
  showSpinner: boolean = false;
  errorMessage?: string;
  userId?: string;
  doctors?: Doctor[];

  constructor(private fb: FormBuilder,
    private AppointmentsService: AppointmentsService,
    private DoctorsService: DoctorsService,
    private store: Store,
    private router: Router) { }

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
      this.selectDoctor(this.appointment.doctor?.id!)
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
      }

      let date = new Date(this.appointmentForm.value.date!);
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();

      let dateTime = year + '-' + month + '-' + day + 'T21:00:00'

      const modifiedAppointment: Appointment = {
        id: this.appointment?.id!,
        date: dateTime,
        place: this.appointmentForm.value.place!,
        userId: this.userId!,
        doctorId: this.appointmentForm.value.doctorId!,
      }
      if (JSON.stringify(modifiedAppointment) !== JSON.stringify(initialAppointment)){
        this.AppointmentsService.patch(modifiedAppointment).pipe().subscribe({
          next: (appointment) => {
            this.appointment = appointment;
            this.initialize();
            this.showSpinner = false;
            window.location.reload();
          },
          error: (err) => {
            this.showSpinner = false;
            console.log(`${err}`);
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
          window.location.reload();
        },
        error: (err) => console.log(`${err.message}`),
      });
    }
  }

  closeEdit(){
    if (!this.showSpinner){
      this.deselect.emit();
    }
  }

  selectDoctor(doctorId: any) {
    if (doctorId.source){
      this.selectedDoctor.emit(doctorId.source.value)
    }
    else{
      this.selectedDoctor.emit(doctorId)
    }
  }
}
