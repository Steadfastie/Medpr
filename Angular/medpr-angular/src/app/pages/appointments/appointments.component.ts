import { AppointmentsService } from 'src/app/services/appointments/appointments.service';
import { Component } from '@angular/core';
import { Appointment } from 'src/app/models/appointment';

@Component({
  selector: 'appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.scss'],
})
export class AppointmentsComponent {
  appointments: Appointment[] = [];

  constructor(private appointmentsService: AppointmentsService) {}

  ngOnInit() {
    this.appointmentsService.getAllAppointments().subscribe(appointments => this.appointments = appointments);
  }
}
