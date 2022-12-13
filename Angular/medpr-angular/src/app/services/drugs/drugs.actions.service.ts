import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Drug } from 'src/app/models/drug';

@Injectable({
  providedIn: 'root',
})
export class DrugsActionsService {
  private responseDrugEvent = new Subject<Drug>();
  private selectDrugEvent = new Subject<string>();
  private deleteDrugEvent = new Subject<string>();

  emitDrugResponse(drug: Drug) {
    this.responseDrugEvent.next(drug);
  }

  drugResponseListner() {
    return this.responseDrugEvent.asObservable();
  }

  emitDrugDelete(id: string) {
    this.deleteDrugEvent.next(id);
  }

  drugDeleteListner() {
    return this.deleteDrugEvent.asObservable();
  }

  emitDrugSelect(drugId: string) {
    this.selectDrugEvent.next(drugId);
  }

  drugSelectListner() {
    return this.selectDrugEvent.asObservable();
  }
}
