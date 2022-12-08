import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { ApiService } from '../api.service';
import * as auth from 'src/app/store/reducers/auth.reducers';
import * as state from 'src/app/store/app.states';


@Injectable()
export class AuthService {
  private token?: string;
  private isAuthenticated: boolean = false;
  private user?: User;
  private role?: string;

  constructor(private apiService: ApiService,
    private store: Store) {
      this.store.select(state.selectStateToken)
        .subscribe((token?: string) => this.token = token);
      this.store.select(state.selectStateAuthStatus)
        .subscribe(isAuthenticated => this.isAuthenticated = isAuthenticated);
      this.store.select(state.selectStateUser)
        .subscribe(user => this.user = user);
      this.store.select(state.selectUserRole)
        .subscribe(role => this.role = role);
     }

  getToken(): string {
    return this.token ?? '';
  }

  getStatus(){
    return this.isAuthenticated;
  }

  getUser(){
    return this.user ?? undefined;
  }

  getRole(){
    return this.role ?? undefined;
  }

  removeToken(): string {
    return this.token ? this.token : '';
  }

  signIn(user: User): Observable<User> {
    return this.apiService.post("signin", user);
  }

  signUp(user: User): Observable<User> {
    return this.apiService.post("signup", user);
  }
}
