import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Doctor } from 'src/app/models/doctor';

@Injectable({
  providedIn: 'root',
})
export class DoctorsActionsService {

  private doctorEvent = new Subject<Doctor>();

  emitDoctorResponse(doctor: Doctor) {
    this.doctorEvent.next(doctor);
  }

  doctorResponseListner() {
    return this.doctorEvent.asObservable();
  }
}
