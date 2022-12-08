import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectStateAuthStatus, selectStateToken } from 'src/app/store/app.states';
import * as auth from 'src/app/store/reducers/auth.reducers';

import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthReverseGuardService implements CanActivate {
  isAuthentificated: boolean = false;
  constructor(
    public authService: AuthService,
    public router: Router,
    private store: Store
  ) {
    this.store.select(selectStateAuthStatus).pipe().subscribe((status) => this.isAuthentificated = status);
  }

  canActivate(): Observable<boolean|UrlTree>|Promise<boolean|UrlTree>|boolean|UrlTree {
    if (this.isAuthentificated){
      this.router.navigateByUrl('');
      return false;
    }
    return true;
  }
}
