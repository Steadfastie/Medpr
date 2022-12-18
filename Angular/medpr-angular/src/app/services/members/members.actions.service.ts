import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Member } from 'src/app/models/member';

@Injectable({
  providedIn: 'root',
})
export class MembersActionsService {
  private responseMemberEvent = new Subject<Member>();
  private joinMemberEvent = new Subject<string>();
  private deleteMemberEvent = new Subject<string>();

  emitMemberResponse(member: Member) {
    this.responseMemberEvent.next(member);
  }

  memberResponseListner() {
    return this.responseMemberEvent.asObservable();
  }

  emitMemberDelete(id: string) {
    this.deleteMemberEvent.next(id);
  }

  memberDeleteListner() {
    return this.deleteMemberEvent.asObservable();
  }

  emitMemberJoinAction(familyId: string) {
    this.joinMemberEvent.next(familyId);
  }

  memberJoinActionListner() {
    return this.joinMemberEvent.asObservable();
  }
}
