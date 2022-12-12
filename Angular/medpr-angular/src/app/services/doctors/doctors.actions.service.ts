import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Doctor } from 'src/app/models/doctor';

@Injectable({
  providedIn: 'root',
})
export class DoctorsActionsService {
  private responseDoctorEvent = new Subject<Doctor>();
  private selectDoctorEvent = new Subject<string>();

  emitDoctorResponse(doctor: Doctor) {
    this.responseDoctorEvent.next(doctor);
  }

  doctorResponseListner() {
    return this.responseDoctorEvent.asObservable();
  }

  emitDoctorSelect(doctorId: string) {
    this.selectDoctorEvent.next(doctorId);
  }

  doctorSelectListner() {
    return this.selectDoctorEvent.asObservable();
  }
}
