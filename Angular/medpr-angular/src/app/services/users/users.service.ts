import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { concatMap, map, mergeMap, Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { ApiService } from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private apiService: ApiService) { }

  getAllUsers(): Observable<User[]>{
    return this.apiService.get('users', {}).pipe();
  }

  getUserById(id: string): Observable<User>{
    return this.apiService.get(`users/${id}`, {}).pipe();
  }

  create(user: User): Observable<User> {
    return this.apiService.post('users', user).pipe();
  }

  patch(user: User): Observable<User> {
    return this.apiService.patch(`users/${user.userId!.toString()}`, user).pipe();
  }

  delete(id: string): Observable<User> {
    return this.apiService.delete(`users/${id}`).pipe();
  }
}
