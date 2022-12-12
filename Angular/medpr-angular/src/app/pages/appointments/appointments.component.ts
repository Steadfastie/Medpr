import { AppointmentsService } from 'src/app/services/appointments/appointments.service';
import { Component } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';
import { ActivatedRoute } from '@angular/router';
import { AppointmentsActionsService } from 'src/app/services/appointments/appointments.actions.service';
import { DoctorsActionsService } from 'src/app/services/doctors/doctors.actions.service';

@Component({
  selector: 'appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.scss']
})
export class AppointmentsComponent {
  appointments: Appointment[] = [];
  idProvided: boolean = false;

  constructor(
    private appointmentsService: AppointmentsService,
    private actions: AppointmentsActionsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const appointmentId = this.route.snapshot.paramMap.get('id');
    if (appointmentId == null) {
      this.appointmentsService.getAllAppointments()
        .subscribe((appointments) => (this.appointments = appointments));
    } else {
      this.appointmentsService.getAppointmentById(appointmentId)
        .subscribe((appointment) => {
          this.appointments.push(appointment),
          this.idProvided = true;
        });
    }

    this.actions.appointmentResponseListner().subscribe((appointmentFromAction) => {
        const presentAppointment = this.appointments.find((presentAppointment) => {
            return presentAppointment.id === appointmentFromAction.id;
          });
        if (!presentAppointment) {
          this.appointments.push(appointmentFromAction);
        } else {
          this.appointments.splice(
            this.appointments.indexOf(presentAppointment), 1, appointmentFromAction
          );
        }
      });
  }
}
