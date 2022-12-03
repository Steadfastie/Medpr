import { User } from 'src/app/models/user';
import { Action, createAction, props } from '@ngrx/store';

export enum AuthActionTypes {
  signin = '[Auth] Signin',
  signin_success = '[Auth] Signin Success',
  signin_error = '[Auth] Signin Error',
  signup = '[Auth] Signup',
  signup_success = '[Auth] Signup Success',
  signup_error = '[Auth] Signup Error',
  logout = '[Auth] Logout',
}

export const signin = createAction(
  AuthActionTypes.signin,
  props<User>()
);

export const signin_success = createAction(
  AuthActionTypes.signin_success,
  props<User>()
);

export const signin_error = createAction(
  AuthActionTypes.signin_error,
  props<{message: string}>()
);

export const signup = createAction(AuthActionTypes.signup, props<User>());

export const signup_success = createAction(
  AuthActionTypes.signup_success,
  props<User>()
);

export const signup_error = createAction(
  AuthActionTypes.signup_error,
  props<{message: string}>()
);

export const logout = createAction(AuthActionTypes.logout);
