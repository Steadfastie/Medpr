import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Vaccine } from 'src/app/models/vaccine';

@Injectable({
  providedIn: 'root',
})
export class VaccinesActionsService {

  private vaccineEvent = new Subject<Vaccine>();

  emitVaccineResponse(vaccine: Vaccine) {
    this.vaccineEvent.next(vaccine);
  }

  vaccineResponseListner() {
    return this.vaccineEvent.asObservable();
  }
}
