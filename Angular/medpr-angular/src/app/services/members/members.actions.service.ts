import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { Member } from 'src/app/models/member';

@Injectable({
  providedIn: 'root',
})
export class MembersActionsService {
  private responseMemberEvent = new Subject<Member>();
  private selectMemberEvent = new Subject<string>();
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

  emitMemberSelect(memberId: string) {
    this.selectMemberEvent.next(memberId);
  }

  memberSelectListner() {
    return this.selectMemberEvent.asObservable();
  }
}
