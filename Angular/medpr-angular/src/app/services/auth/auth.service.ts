import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { ApiService } from '../api.service';
import * as auth from 'src/app/store/reducers/auth.reducers';


@Injectable()
export class AuthService {
  private token?: string;

  constructor(private apiService: ApiService,
    private store: Store<auth.State>) {
      this.store.select('token').subscribe((token?: string) => this.token = token);
     }

  getToken(): string {
    return this.token ? this.token : '';
  }

  removeToken(): string {
    return this.token ? this.token : '';
  }

  signIn(user: User): Observable<User> {
    return this.apiService.post("singin", user);
  }

  signUp(user: User): Observable<User> {
    return this.apiService.post("singun", user);
  }
}
