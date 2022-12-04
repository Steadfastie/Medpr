import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { Store } from '@ngrx/store';
import * as auth from 'src/app/store/reducers/auth.reducers';

import { AuthService } from './auth.service';


@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(
    public auth: AuthService,
    public router: Router,
    private store: Store<auth.State>
  ) {}
  canActivate(): boolean {
    if (!this.store.select('token')) {
      this.router.navigateByUrl('/login');
      return false;
    }
    return true;
  }
}
