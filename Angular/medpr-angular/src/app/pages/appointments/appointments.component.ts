import { AppointmentsService } from 'src/app/services/appointments/appointments.service';
import { Component } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.scss'],
})
export class AppointmentsComponent {
  appointments: Appointment[] = [];
  idProvided: boolean = false;

  constructor(private appointmentsService: AppointmentsService,
    private route: ActivatedRoute) {}

  ngOnInit() {
    const appointmentId  = this.route.snapshot.paramMap.get('id');
    if (appointmentId == null) {
      this.appointmentsService.getAllAppointments()
        .subscribe(appointments => this.appointments = appointments);
    }
    else{
      this.appointmentsService.getAppointmentById(appointmentId)
        .subscribe(appointment => {
          this.appointments.push(appointment),
          this.idProvided = true;
        });
    }

  }
}
