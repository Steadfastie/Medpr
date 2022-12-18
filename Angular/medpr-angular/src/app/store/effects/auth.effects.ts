import { logout } from './../actions/auth.actions';
import { Injectable } from '@angular/core';
import { catchError, exhaustMap, map, tap } from 'rxjs/operators';

import * as userActions from '../actions/auth.actions';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AuthService } from 'src/app/services/auth/auth.service';
import { of } from 'rxjs';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';
import { ConsoleLogger } from '@microsoft/signalr/dist/esm/Utils';

@Injectable()
export class AuthEffects {

  constructor(
    private actions$: Actions,
    private authService: AuthService,
    private router: Router
  ) {}

  signin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(userActions.signin),
      exhaustMap((user) =>
        this.authService.signIn(user).pipe(
          map((user:User) => userActions.signin_success(user)),
          catchError((error:string) => of(userActions.signin_error({message: error})))
        )
      )
    )
  );

  signin_success$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(userActions.signin_success),
        tap((user) => {
          localStorage.setItem(`user`, JSON.stringify(user));
          this.router.navigate(['']);
        })
      ),
    { dispatch: false }
  );

  signin_error$ = createEffect(
    () => this.actions$.pipe(ofType(userActions.signin_error)),
    { dispatch: false }
  );

  signup$ = createEffect(() =>
    this.actions$.pipe(
      ofType(userActions.signup),
      exhaustMap((user) =>
        this.authService.signUp(user).pipe(
          map((user) => userActions.signup_success(user)),
          catchError((error) => of(userActions.signup_error(error)))
        )
      )
    )
  );

  signup_success$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(userActions.signup_success),
        tap((user) => {
          localStorage.setItem(`user`, JSON.stringify(user));
          this.router.navigate(['']);
        })
      ),
    { dispatch: false }
  );

  signup_error$ = createEffect(
    () => this.actions$.pipe(ofType(userActions.signup_error)),
    { dispatch: false }
  );

  logout$ = createEffect(
    () => this.actions$.pipe(
      ofType(userActions.logout),
      tap(() => {
        this.router.navigate(['/signin']);
        console.log("logout effect");
        localStorage.removeItem(`user`);
      })),
      {dispatch: false}
  );
}
