import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Family } from 'src/app/models/family';

@Injectable({
  providedIn: 'root',
})
export class FamiliesActionsService {
  private responseFamilyEvent = new Subject<Family>();
  private getOutFamilyEvent = new Subject<string>();
  private joinedFamilyEvent = new Subject<string>();
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

  emitFamilyGetOut(familyId: string) {
    this.getOutFamilyEvent.next(familyId);
  }

  familyGetOutListner() {
    return this.getOutFamilyEvent.asObservable();
  }

  emitFamilyJoined(familyId: string) {
    this.joinedFamilyEvent.next(familyId);
  }

  familyJoinedListner() {
    return this.joinedFamilyEvent.asObservable();
  }
}
