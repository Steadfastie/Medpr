import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Vaccination } from 'src/app/models/vaccination';

@Injectable({
  providedIn: 'root',
})
export class VaccinationsActionsService {
  private vaccinationEvent = new Subject<Vaccination>();
  private deleteVaccinationEvent = new Subject<string>();

  emitVaccinationResponse(vaccination: Vaccination) {
    this.vaccinationEvent.next(vaccination);
  }

  vaccinationResponseListner() {
    return this.vaccinationEvent.asObservable();
  }

  emitVaccinationDelete(id: string) {
    this.deleteVaccinationEvent.next(id);
  }

  vaccinationDeleteListner() {
    return this.deleteVaccinationEvent.asObservable();
  }
}
