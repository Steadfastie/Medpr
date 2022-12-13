import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Vaccine } from 'src/app/models/vaccine';

@Injectable({
  providedIn: 'root',
})
export class VaccinesActionsService {
  private responseVaccineEvent = new Subject<Vaccine>();
  private selectVaccineEvent = new Subject<string>();
  private deleteVaccineEvent = new Subject<string>();

  emitVaccineResponse(vaccine: Vaccine) {
    this.responseVaccineEvent.next(vaccine);
  }

  vaccineResponseListner() {
    return this.responseVaccineEvent.asObservable();
  }

  emitVaccineDelete(id: string) {
    this.deleteVaccineEvent.next(id);
  }

  vaccineDeleteListner() {
    return this.deleteVaccineEvent.asObservable();
  }

  emitVaccineSelect(vaccineId: string) {
    this.selectVaccineEvent.next(vaccineId);
  }

  vaccineSelectListner() {
    return this.selectVaccineEvent.asObservable();
  }
}
