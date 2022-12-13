import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Prescription } from 'src/app/models/prescription';

@Injectable({
  providedIn: 'root',
})
export class PrescriptionsActionsService {
  private prescriptionEvent = new Subject<Prescription>();
  private deletePrescriptionEvent = new Subject<string>();

  emitPrescriptionResponse(prescription: Prescription) {
    this.prescriptionEvent.next(prescription);
  }

  prescriptionResponseListner() {
    return this.prescriptionEvent.asObservable();
  }

  emitPrescriptionDelete(id: string) {
    this.deletePrescriptionEvent.next(id);
  }

  prescriptionDeleteListner() {
    return this.deletePrescriptionEvent.asObservable();
  }
}
