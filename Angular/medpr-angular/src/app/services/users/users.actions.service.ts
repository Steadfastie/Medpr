import { Injectable } from '@angular/core';
import { BehaviorSubject, concatMap, map, mergeMap, Observable, Subject } from 'rxjs';
import { User } from 'src/app/models/user';

@Injectable({
  providedIn: 'root',
})
export class UsersActionsService {
  private responseUserEvent = new Subject<User>();
  private selectUserEvent = new Subject<string>();
  private deleteUserEvent = new Subject<string>();

  emitUserResponse(user: User) {
    this.responseUserEvent.next(user);
  }

  userResponseListner() {
    return this.responseUserEvent.asObservable();
  }

  emitUserDelete(id: string) {
    this.deleteUserEvent.next(id);
  }

  userDeleteListner() {
    return this.deleteUserEvent.asObservable();
  }

  emitUserSelect(userId: string) {
    this.selectUserEvent.next(userId);
  }

  userSelectListner() {
    return this.selectUserEvent.asObservable();
  }
}
