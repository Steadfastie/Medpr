import { createReducer, on } from '@ngrx/store';
import { User } from '../../models/user';
import * as userActions from '../actions/auth.actions';

export interface State {
  isAuthenticated: boolean;
  user?: User;
  errorMessage?: string;
}

export const userFromLocalStorage =
  localStorage.getItem('user') != null ?
 (localStorage.getItem("user") as string) : undefined;

export const initialState: State = {
  isAuthenticated: userFromLocalStorage != undefined ? true : false,
  user: userFromLocalStorage != undefined ? JSON.parse(userFromLocalStorage) : undefined
};


export const authReducer = createReducer(
  initialState,
  on(userActions.signin_success, (state, user: User) => ({
    ...state,
    isAuthenticated: true,
    user: user,
    errorMessage: undefined
  })),
  on(userActions.signin_error, (state, {message}) => ({
    ...state,
    errorMessage: message})),

  on(userActions.signup_success, (state, user: User) => ({
    ...state,
    isAuthenticated: true,
    user: user,
    errorMessage: undefined
  })),
  on(userActions.signup_error, (state, {message}) => ({
    ...state,
    errorMessage: message})),

  on(userActions.logout, state => state = initialState),
);
