import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Appointment } from 'src/app/models/appointment';

@Injectable({
  providedIn: 'root',
})
export class AppointmentsActionsService {
  private appointmentEvent = new Subject<Appointment>();
  private deleteAppointmentEvent = new Subject<string>();

  emitAppointmentResponse(appointment: Appointment) {
    this.appointmentEvent.next(appointment);
  }

  appointmentResponseListner() {
    return this.appointmentEvent.asObservable();
  }

  emitAppointmentDelete(id: string) {
    this.deleteAppointmentEvent.next(id);
  }

  appointmentDeleteListner() {
    return this.deleteAppointmentEvent.asObservable();
  }
}
