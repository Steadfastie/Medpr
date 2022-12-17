import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Family } from 'src/app/models/family';

@Injectable({
  providedIn: 'root',
})
export class FamiliesActionsService {
  private responseFamilyEvent = new Subject<Family>();
  private selectFamilyEvent = new Subject<string>();
  private deleteFamilyEvent = new Subject<string>();

  emitFamilyResponse(family: Family) {
    this.responseFamilyEvent.next(family);
  }

  familyResponseListner() {
    return this.responseFamilyEvent.asObservable();
  }

  emitFamilyDelete(id: string) {
    this.deleteFamilyEvent.next(id);
  }

  familyDeleteListner() {
    return this.deleteFamilyEvent.asObservable();
  }

  emitFamilieselect(familyId: string) {
    this.selectFamilyEvent.next(familyId);
  }

  familieselectListner() {
    return this.selectFamilyEvent.asObservable();
  }
}
