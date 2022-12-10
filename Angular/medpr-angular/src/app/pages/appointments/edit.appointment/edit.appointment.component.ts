import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Appointment } from 'src/app/models/appointment';
import { AppointmentsService } from 'src/app/services/appointments/appointments.service';


@Component({
  selector: 'edit-appointment',
  templateUrl: './edit.appointment.component.html',
  styleUrls: ['./edit.appointment.component.scss']
})
export class EditAppointmentComponent implements OnInit {
  @Input() appointment?: Appointment;
  @Output() deselect = new EventEmitter<void>();
  showSpinner: boolean = false;
  errorMessage?: string;

  constructor(private fb: FormBuilder,
    private AppointmentsService: AppointmentsService,
    private router: Router) { }

  ngOnInit(): void {
    // this.initialize();
  }

  appointmentForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    experience: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
  });

  // initialize() {
  //   if (this.appointment) {
  //     this.appointmentForm.setValue({
  //       name: this.appointment.name,
  //       experience: this.appointment.experience.toString()
  //     })
  //   }
  // }

  // edit(){
  //   if (!this.showSpinner && this.appointmentForm.valid){
  //     this.showSpinner = true;
  //     const initialAppointment = {
  //       id: this.appointment!.id,
  //       name: this.appointment!.name,
  //       experience: this.appointment!.experience
  //     }
  //     const modifiedAppointment: Appointment = {
  //       id: this.appointment?.id!,
  //       name: this.appointmentForm.value.name!,
  //       experience: Number(this.appointmentForm.value.experience!)
  //     }
  //     if (JSON.stringify(modifiedAppointment) !== JSON.stringify(initialAppointment)){
  //       this.AppointmentsService.patch(modifiedAppointment).pipe().subscribe({
  //         next: (appointment) => {
  //           this.appointment = appointment;
  //           this.initialize();
  //           this.showSpinner = false;
  //           this.closeEdit();
  //         },
  //         error: (err) => {
  //           this.showSpinner = false;
  //           console.log(`${err}`);
  //           this.errorMessage = "Could not modify appointment";
  //         },
  //       });
  //     }
  //   }

  // }

  // remove(){
  //   if (!this.showSpinner){
  //     this.showSpinner = true;
  //     this.AppointmentsService.delete(this.appointment!.id).pipe().subscribe({
  //       next: () => {
  //         this.showSpinner = false;
  //         window.location.reload();
  //       },
  //       error: (err) => console.log(`${err.message}`),
  //     });
  //   }
  // }

  // closeEdit(){
  //   if (!this.showSpinner){
  //     this.deselect.emit();
  //   }
  // }
}
