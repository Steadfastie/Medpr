import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { Appointment } from 'src/app/models/appointment';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class AppointmentsService {

  constructor(private apiService: ApiService) { }

  getAllAppointments(): Observable<Appointment[]>{
    return this.apiService.get('appointments', {}).pipe();
  }

  getAppointmentById(id: string): Observable<Appointment> {
    return this.apiService.get(`appointments/${id}`, {}).pipe();
  }

  create(appointment: Appointment): Observable<Appointment> {
    return this.apiService.post('appointments', appointment).pipe();
  }

  patch(appointment: Appointment): Observable<Appointment> {
    return this.apiService.patch(`appointments/${appointment.id.toString()}`, appointment).pipe();
  }

  delete(id: string): Observable<Appointment> {
    return this.apiService.delete(`appointments/${id}`).pipe();
  }
}
